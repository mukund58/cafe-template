namespace backend.DTOs;
using backend.Constants;
using System.ComponentModel.DataAnnotations;

public record CouponResponseDto(
    int Id,
    string Code,
    CouponType Type,
    decimal Value
);

// APPLICATION LAYER (DTO Request)
public record CouponRequestDto(
    [Required, StringLength(20), RegularExpression(@"^[A-Z0-9]+$", ErrorMessage = "Code must be alphanumeric uppercase")] string Code,
    [Required] CouponType Type,
    [Required, Range(0.01, 10000.00, ErrorMessage = "Value must be positive")] decimal Value
);


