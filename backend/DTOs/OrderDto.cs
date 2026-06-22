namespace backend.DTOs;
using backend.Constants;
using System.ComponentModel.DataAnnotations;

// --- Sub-components for Order Responses ---
public record OrderItemResponseDto(
    int Id,
    int ProductId,
    string ProductName,
    int Quantity,
    decimal UnitPrice,
    decimal TotalPrice // Calculated: Quantity * UnitPrice
);

public record OrderItemRequestDto(
    [Required] int ProductId,
    [Required, Range(1, 100, ErrorMessage = "Quantity must be between 1 and 100")] int Quantity
);

public record CreateOrderRequestDto(
    [Required] int TableId,
    [Required] int CustomerId,
    [Required] int EmployeeId,
    int? AppliedCouponId,
    [Required, MinLength(1, ErrorMessage = "Order must contain at least one item")] ICollection<OrderItemRequestDto> OrderItems
);


