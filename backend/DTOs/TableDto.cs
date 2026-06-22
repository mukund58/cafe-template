namespace backend.DTOs;
using System.ComponentModel.DataAnnotations;

public record TableResponseDto(
    int Id,
    string TableNumber,
    int Seats,
    int FloorId
);

public record TableRequestDto(
    [Required, StringLength(10)] string TableNumber,
    [Required, Range(1, 50, ErrorMessage = "Tables must have between 1 and 50 seats")] int Seats,
    [Required] int FloorId
);
