namespace backend.DTOs;
using System.ComponentModel.DataAnnotations;

public record ProductResponseDto(
    int Id,
    string Name,
    decimal Price,
    int CategoryId
);

public record ProductRequestDto(
    [Required, StringLength(150)] string Name,
    [Required, Range(0.01, 100000.00, ErrorMessage = "Price must be greater than 0")] decimal Price,
    [Required] int CategoryId
);
