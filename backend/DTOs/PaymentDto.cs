namespace backend.DTOs;
using backend.Constants;
using System.ComponentModel.DataAnnotations;

public record PaymentResponseDto(
    int Id,
    PaymentMethod Method,
    decimal Amount,
    PaymentStatus Status
);

public record PaymentRequestDto(
    [Required] PaymentMethod Method,
    [Required, Range(0.01, 100000.00, ErrorMessage = "Amount must be greater than 0")] decimal Amount
); 
