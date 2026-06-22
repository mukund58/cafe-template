namespace backend.DTOs;
using System.ComponentModel.DataAnnotations;

public record FloorResponseDto(
    int Id,
    int FloorNumber,
    string Name
);

public record FloorRequestDto(
    [Required, Range(0, 100, ErrorMessage = "Floor must be between 0 and 100")] int FloorNumber,
    [Required, StringLength(50)] string Name
);
