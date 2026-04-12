namespace BakeryAPI.DTOs.Ingredients;

public class SupplierPriceDto
{
    public int SupplierId { get; set; }
    public string Name { get; set; }
    public string ContactPerson { get; set; } 
    public string PhoneNumber { get; set; } 
    public string Email { get; set; }
    public decimal PricePerKg { get; set; }
}
