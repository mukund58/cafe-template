namespace backend.Models;

public class Floor
{
    public int FloorId { get; set; }
    public string FloorNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public ICollection<Table> Tables {get;set;}= new List<Table>();
}
