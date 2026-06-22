namespace backend.Models;
using backend.Constants;

public class Coupon
{
    public int Id { get; set; }
    public string Code { get; set; } = string.Empty;
    public CouponType Type { get; set; }
    public decimal Value { get; set; }
    public bool IsActive { get; set; } = true; 
}

