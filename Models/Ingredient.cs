namespace BakeryAPI.Models;

public class Ingredient : BaseEntity
{
    public required string ArticleNumber { get; set; }
    public required string Name { get; set; }

    public ICollection<SupplierIngredient> SupplierIngredients { get; set; } = new List<SupplierIngredient>();
}
