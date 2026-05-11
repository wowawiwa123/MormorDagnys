namespace BakeryAPI.DTOs.Ingredients;

public class IngredientInfoDto
{
    public string IngredientId { get; set; } = string.Empty;
    public string ArticleNumber { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public decimal PricePerKg { get; set; }
}
