namespace BakeryAPI.Models;

public class Ingredient
{
    public int Id { get; set; }
    public string ArticleNumber { get; set; }
    public string Name { get; set; }


    public ICollection<SupplierIngredient> SupplierIngredients { get; set; } = new List<SupplierIngredient>();
}
