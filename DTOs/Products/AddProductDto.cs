using System.ComponentModel.DataAnnotations;

namespace BakeryAPI.DTOs.Products;

public class AddProductDto
{
    [Required]
    public string Name { get; set; } = string.Empty;
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal PricePerUnit { get; set; }
    [Range(1, double.MaxValue, ErrorMessage = "Weight must be greater than 0.")]
    public double WeightGrams { get; set; }
    [Range(1, int.MaxValue, ErrorMessage = "Units per package must be at least 1.")]
    public int UnitsPerPackage { get; set; }
    public DateTime BestBefore { get; set; }
    public DateTime ManufacturedDate { get; set; }
}
