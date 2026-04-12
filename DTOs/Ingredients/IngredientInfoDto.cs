namespace BakeryAPI.DTOs.Ingredients;

public class IngredientInfoDto
{
    public int IngredientId { get; set; }
    public string ArticleNumber { get; set; } 
    public string Name { get; set; } 
    public decimal PricePerKg { get; set; }
}
