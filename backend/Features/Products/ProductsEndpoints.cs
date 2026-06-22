using backend.DTOs;
using backend.Models;
using backend.Data;

using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;

namespace backend.Features.Products;

public static class ProductsEndpoints
{
    public static void MapProductsEndpoints(this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("api/products")
                       .WithTags("Products");

        group.MapGet("/", async (AppDbContext context) =>
        {
            var products = await context.Products
                .Select(p => new ProductResponseDto(p.Id, p.Name, p.Price, p.CategoryId))
                .ToListAsync();
            return Results.Ok(products);
        });

        group.MapGet("/{id:int}", async (int id, AppDbContext context) =>
        {
            var product = await context.Products
                .Where(p => p.Id == id)
                .Select(p => new ProductResponseDto(p.Id, p.Name, p.Price, p.CategoryId))
                .FirstOrDefaultAsync();

            return product is not null ? Results.Ok(product) : Results.NotFound(new { message = "Product not found" });
        });

        group.MapPost("/", async (ProductRequestDto request, AppDbContext context) =>
        {
            // Business Rule Validation
            if (request.Price <= 0) return Results.BadRequest(new { message = "Price must be greater than zero" });

            // Referential Integrity Check
            var categoryExists = await context.Categories.AnyAsync(c => c.CategoryId == request.CategoryId);
            if (!categoryExists) return Results.BadRequest(new { message = "Target CategoryId does not exist" });

            var product = new Product
            {
                Name = request.Name,
                Price = request.Price,
                CategoryId = request.CategoryId
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            var response = new ProductResponseDto(product.Id, product.Name, product.Price, product.CategoryId);
            return Results.Created($"/api/products/{product.Id}", response);
        });

        group.MapPut("/{id:int}", async (int id, ProductRequestDto request, AppDbContext context) =>
        {
            var product = await context.Products.FindAsync(id);
            if (product is null) return Results.NotFound(new { message = "Product not found" });

            var categoryExists = await context.Categories.AnyAsync(c => c.CategoryId == request.CategoryId);
            if (!categoryExists) return Results.BadRequest(new { message = "Target CategoryId does not exist" });

            product.Name = request.Name;
            product.Price = request.Price;
            product.CategoryId = request.CategoryId;

            await context.SaveChangesAsync();
            return Results.NoContent();
        });

        group.MapDelete("/{id:int}", async (int id, AppDbContext context) =>
        {
            var product = await context.Products.FindAsync(id);
            if (product is null) return Results.NotFound(new { message = "Product not found" });

            context.Products.Remove(product);
            await context.SaveChangesAsync();
            return Results.NoContent();
        });
    }
}

