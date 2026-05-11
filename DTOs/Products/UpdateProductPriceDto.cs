using System.ComponentModel.DataAnnotations;

namespace BakeryAPI.DTOs.Products;

public class UpdateProductPriceDto
{
    [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
    public decimal NewPricePerUnit { get; set; }
}
