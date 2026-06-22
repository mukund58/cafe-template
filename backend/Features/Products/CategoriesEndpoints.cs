using backend.DTOs;
using backend.Models;
using backend.Data;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace backend.Features.Products;

public static class CategoriesEndpoints
{
    public static void MapCategoriesEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/categories")
                       .WithTags("Categories");

        group.MapGet("/", async (AppDbContext context) =>
        {
            var categories = await context.Categories
                .Select(c => new CategoryResponseDto(c.CategoryId, c.Name, c.Color))
                .ToListAsync();
            return Results.Ok(categories);
        });

        group.MapGet("/{id:int}", async (int id, AppDbContext context) =>
        {
            var category = await context.Categories
                .Where(c => c.CategoryId == id)
                .Select(c => new CategoryResponseDto(c.CategoryId, c.Name, c.Color))
                .FirstOrDefaultAsync();

            return category is not null ? Results.Ok(category) : Results.NotFound(new { message = "Category not found" });
        });

        group.MapPost("/", async (CategoryRequestDto request, AppDbContext context) =>
        {
            // Mini Validation check for minimalist pipelines
            if (string.IsNullOrWhiteSpace(request.Name)) 
                return Results.BadRequest(new { message = "Category name is required" });

            var category = new Category
            {
                Name = request.Name,
                Color = request.Color
            };

            context.Categories.Add(category);
            await context.SaveChangesAsync();

            var response = new CategoryResponseDto(category.CategoryId, category.Name, category.Color);
            return Results.Created($"/api/categories/{category.CategoryId}", response);
        });

        group.MapPut("/{id:int}", async (int id, CategoryRequestDto request, AppDbContext context) =>
        {
            var category = await context.Categories.FindAsync(id);
            if (category is null) return Results.NotFound(new { message = "Category not found" });

            category.Name = request.Name;
            category.Color = request.Color;

            await context.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, AppDbContext context) =>
        {
            var category = await context.Categories.FindAsync(id);
            if (category is null) return Results.NotFound(new { message = "Category not found" });

            context.Categories.Remove(category);
            await context.SaveChangesAsync(); // Restrict rule handles dependencies safely
            return Results.NoContent();
        });
    }
}

