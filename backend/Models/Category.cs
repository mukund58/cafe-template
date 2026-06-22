namespace backend.Models;

public class Category
{
    public int CategoryId { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Color {get;set;}
    public ICollection<Product> Products { get; set; } = new List<Product>();
}
