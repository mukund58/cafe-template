using System.Security.Claims;
using Microsoft.EntityFrameworkCore;
using backend.Data;
using backend.DTOs;
using backend.Models;

namespace backend.Features.Profile;

public static class ProfileEndpoints
{
    private const long MaxImageSizeBytes = 5 * 1024 * 1024;
    private static readonly HashSet<string> AllowedContentTypes = new(
        StringComparer.OrdinalIgnoreCase)
    {
        "image/jpeg",
        "image/jpg",
        "image/png",
        "image/webp"
    };

    public static RouteGroupBuilder MapProfileEndpoints(
        this IEndpointRouteBuilder app)
    {
        var group = app.MapGroup("/profile")
            .RequireAuthorization();

        group.MapGet("", GetProfile);
        group.MapPut("", UpdateProfile);
        group.MapPost("/image", UploadProfileImage);

        return group;
    }

    private static async Task<IResult> GetProfile(
        ClaimsPrincipal user,
        AppDbContext db)
    {
        var currentUser = await GetCurrentUserAsync(user, db);

        return currentUser is null
            ? Results.Unauthorized()
            : Results.Ok(ToResponse(currentUser));
    }

    private static async Task<IResult> UpdateProfile(
        ClaimsPrincipal user,
        AppDbContext db,
        ProfileUpdateRequest request)
    {
        var currentUser = await GetCurrentUserAsync(user, db);

        if (currentUser is null)
            return Results.Unauthorized();

        var name = request.Name?.Trim();

        if (string.IsNullOrWhiteSpace(name))
            return Results.BadRequest("Name is required");

        if (name.Length > 100)
            return Results.BadRequest("Name must be 100 characters or fewer");

        currentUser.Name = name;

        await db.SaveChangesAsync();

        return Results.Ok(ToResponse(currentUser));
    }

    private static async Task<IResult> UploadProfileImage(
        ClaimsPrincipal user,
        AppDbContext db,
        IWebHostEnvironment env,
        IFormFile? image)
    {
        var currentUser = await GetCurrentUserAsync(user, db);

        if (currentUser is null)
            return Results.Unauthorized();

        if (image is null || image.Length == 0)
            return Results.BadRequest("Image file is required");

        if (image.Length > MaxImageSizeBytes)
            return Results.BadRequest("Image must be 5 MB or smaller");

        if (!AllowedContentTypes.Contains(image.ContentType))
            return Results.BadRequest("Only JPEG, PNG, and WebP images are allowed");

        var extension = await DetectImageExtensionAsync(image);

        if (extension is null)
            return Results.BadRequest("The uploaded file is not a valid image");

        var webRoot = env.WebRootPath
            ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");

        var uploadDirectory = Path.Combine(
            webRoot,
            "uploads",
            "profiles");

        Directory.CreateDirectory(uploadDirectory);

        var fileName = $"{Guid.NewGuid():N}{extension}";
        var physicalPath = Path.Combine(uploadDirectory, fileName);

        await using (var stream = File.Create(physicalPath))
        {
            await image.CopyToAsync(stream);
        }

        if (!string.IsNullOrWhiteSpace(currentUser.ProfileImagePath))
        {
            var previousPhysicalPath = GetPhysicalPath(
                webRoot,
                currentUser.ProfileImagePath);

            if (File.Exists(previousPhysicalPath))
                File.Delete(previousPhysicalPath);
        }

        currentUser.ProfileImagePath = $"/uploads/profiles/{fileName}";

        await db.SaveChangesAsync();

        return Results.Ok(ToResponse(currentUser));
    }

    private static async Task<User?> GetCurrentUserAsync(
        ClaimsPrincipal user,
        AppDbContext db)
    {
        if (!Guid.TryParse(
                user.FindFirstValue(ClaimTypes.NameIdentifier),
                out var userId))
        {
            return null;
        }

        return await db.Users
            .FirstOrDefaultAsync(x => x.Id == userId);
    }

    private static UserResponse ToResponse(User user)
    {
        return new UserResponse(
            user.Id,
            user.Name,
            user.Email,
            user.Role,
            user.ProfileImagePath,
	    user.CreatedAt
	    );
    }

    private static async Task<string?> DetectImageExtensionAsync(
        IFormFile image)
    {
        byte[] header = new byte[12];

        await using var stream = image.OpenReadStream();
        var bytesRead = await stream.ReadAsync(header.AsMemory(0, header.Length));

        if (bytesRead < 8)
            return null;

        if (header[0] == 0xFF && header[1] == 0xD8 && header[2] == 0xFF)
            return ".jpg";

        if (header[0] == 0x89 &&
            header[1] == 0x50 &&
            header[2] == 0x4E &&
            header[3] == 0x47 &&
            header[4] == 0x0D &&
            header[5] == 0x0A &&
            header[6] == 0x1A &&
            header[7] == 0x0A)
        {
            return ".png";
        }

        if (bytesRead >= 12 &&
            header[0] == 0x52 &&
            header[1] == 0x49 &&
            header[2] == 0x46 &&
            header[3] == 0x46 &&
            header[8] == 0x57 &&
            header[9] == 0x45 &&
            header[10] == 0x42 &&
            header[11] == 0x50)
        {
            return ".webp";
        }

        return null;
    }

    private static string GetPhysicalPath(string webRoot, string relativePath)
    {
        var trimmed = relativePath.TrimStart('/', '\\');

        return Path.Combine(webRoot, trimmed.Replace('/', Path.DirectorySeparatorChar));
    }
}
