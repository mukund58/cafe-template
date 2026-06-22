namespace backend.Models;
using backend.Constants;

public class Order
{
    public int Id { get; set; }
    public string OrderNumber { get; set; } = string.Empty;
    public OrderStatus Status { get; set; } = OrderStatus.Pending;
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public int TableId { get; set; }
    public Table? Table { get; set; }

    public int CustomerId { get; set; }
    public Customer? Customer { get; set; }

    public Guid EmployeeId { get; set; }
    public User? Employee { get; set; }

    public int? AppliedCouponId {get;set;}
    public Coupon? Coupon {get;set;}

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
