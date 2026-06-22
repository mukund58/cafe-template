namespace backend.Models;

using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

public class Product
{
    public int Id { get; set; }

    [Required]
    public string Name { get; set; }= string.Empty;

    [Column(TypeName = "decimal(19, 4)")]
    public decimal Price { get; set; }


    public int CategoryId { get; set; }

    public Category Category { get; set; } = null!;
}
