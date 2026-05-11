namespace BakeryAPI.DTOs.Ingredients;

public class SupplierPriceDto
{
    public string SupplierId { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public decimal PricePerKg { get; set; }
}
