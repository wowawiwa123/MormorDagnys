using System.ComponentModel.DataAnnotations;

namespace BakeryAPI.DTOs.Suppliers;

public class AddIngredientToSupplierDto
{
    [Required]
    public string IngredientId { get; set; } = string.Empty;
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal PricePerKg { get; set; }
}
