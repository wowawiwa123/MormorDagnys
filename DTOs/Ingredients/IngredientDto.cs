namespace BakeryAPI.DTOs.Ingredients;

public class IngredientDto
{
    public string Id { get; set; } = string.Empty;
    public string ArticleNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public List<SupplierPriceDto> Suppliers { get; set; } = new();
}
