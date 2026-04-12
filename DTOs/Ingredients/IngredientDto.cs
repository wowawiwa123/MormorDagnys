namespace BakeryAPI.DTOs.Ingredients;

public class IngredientDto
{
    public int Id { get; set; }
    public string ArticleNumber { get; set; } 
    public string Name { get; set; } 
    public List<SupplierPriceDto> Suppliers { get; set; } = new();
}
