using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryAPI.Models;

public class SupplierIngredient
{
    public required string SupplierId { get; set; }
    [ForeignKey("SupplierId")]
    public Supplier Supplier { get; set; } = null!;

    public required string IngredientId { get; set; }
    [ForeignKey("IngredientId")]
    public Ingredient Ingredient { get; set; } = null!;

    public decimal PricePerKg { get; set; }
}
