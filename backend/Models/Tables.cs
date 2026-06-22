namespace backend.Models;

public class Table
{
    public int TableId { get; set; }
    public string TableNumber { get; set; } = string.Empty;
    public int Seats { get; set; }
    public int FloorId { get; set; }
    public Floor? Floor { get; set; }
    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
