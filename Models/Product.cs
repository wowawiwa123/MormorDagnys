namespace BakeryAPI.Models;

public class Product : BaseEntity
{
    public required string Name { get; set; }
    public decimal PricePerUnit { get; set; }
    public double WeightGrams { get; set; }
    public int UnitsPerPackage { get; set; }
    public DateTime BestBefore { get; set; }
    public DateTime ManufacturedDate { get; set; }

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
