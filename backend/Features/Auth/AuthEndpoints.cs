using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;
using backend.Models;
using backend.Data;
using backend.DTOs;

namespace backend.Features.Auth;

public static class AuthEndpoints
{
    public static RouteGroupBuilder MapAuthEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/auth");

        group.MapPost("/register", Register);

        group.MapPost("/login", Login);

        group.MapGet("/me", (
            ClaimsPrincipal user,
            AppDbContext db) =>
        {
            return GetCurrentUser(user, db);
        })
        .RequireAuthorization();

        group.MapGet("/admin", () =>
        {
            return Results.Ok("Admin only");
        })
        .RequireAuthorization(policy =>
            policy.RequireRole("admin"));

        return group;
    }

    private static async Task<IResult> GetCurrentUser(
        ClaimsPrincipal user,
        AppDbContext db)
    {
        if (!Guid.TryParse(
                user.FindFirstValue(
                    ClaimTypes.NameIdentifier),
                out var userId))
        {
            return Results.Unauthorized();
        }

        var currentUser = await db.Users
            .AsNoTracking()
            .FirstOrDefaultAsync(x => x.Id == userId);

        if (currentUser is null)
            return Results.Unauthorized();

        return Results.Ok(new UserResponse(
            currentUser.Id,
            currentUser.Name,
            currentUser.Email,
            currentUser.Role,
            currentUser.ProfileImagePath,
	    currentUser.CreatedAt
	    ));
    }

    private static async Task<IResult> Register(
        RegisterRequest request,
        AppDbContext db,
        JwtService jwtService)
    {
        var exists = await db.Users
            .AnyAsync(x => x.Email == request.Email);

        if (exists)
            return Results.BadRequest("Email already exists");

        var user = new User
        {
            Name = request.Name,
            Email = request.Email,
            PasswordHash =
                BCrypt.Net.BCrypt.HashPassword(
                    request.Password)
        };

        db.Users.Add(user);

        await db.SaveChangesAsync();

        var token = jwtService.GenerateToken(user);

        return Results.Ok(
            new AuthResponse(
                token,
                user.Name,
                user.Email,
                user.Role.ToString()));
    }

    private static async Task<IResult> Login(
        LoginRequest request,
        AppDbContext db,
        JwtService jwtService)
    {
        var user = await db.Users
            .FirstOrDefaultAsync(
                x => x.Email == request.Email);

        if (user is null)
            return Results.Unauthorized();

        var valid =
            BCrypt.Net.BCrypt.Verify(
                request.Password,
                user.PasswordHash);

        if (!valid)
            return Results.Unauthorized();

        var token = jwtService.GenerateToken(user);

        return Results.Ok(
            new AuthResponse(
                token,
                user.Name,
                user.Email,
                user.Role.ToString()));
    }
}
