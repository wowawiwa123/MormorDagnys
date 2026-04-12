using BakeryAPI.DTOs.Ingredients;

namespace BakeryAPI.DTOs.Suppliers;

public class SupplierWithIngredientsDto : SupplierDto
{
    public List<IngredientInfoDto> Ingredients { get; set; } = new();
}
