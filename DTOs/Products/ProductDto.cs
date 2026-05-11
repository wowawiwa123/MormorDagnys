namespace BakeryAPI.DTOs.Products;

public class ProductDto
{
    public string Id { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal PricePerUnit { get; set; }
    public double WeightGrams { get; set; }
    public int UnitsPerPackage { get; set; }
    public string BestBefore { get; set; } = string.Empty;
    public string ManufacturedDate { get; set; } = string.Empty;
}
