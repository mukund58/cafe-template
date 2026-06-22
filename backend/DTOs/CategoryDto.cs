namespace backend.DTOs;
using System.ComponentModel.DataAnnotations;

public record CategoryResponseDto(
    int Id,
    string Name,
    string? Color
);

public record CategoryRequestDto(
    [Required, StringLength(50)] string Name,
    [RegularExpression("^#([A-Fa-f0-9]{6}|[A-Fa-f0-9]{3})$", ErrorMessage = "Invalid Hex Color")] string? Color
);

