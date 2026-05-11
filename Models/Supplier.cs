using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryAPI.Models;

public class Supplier : BaseEntity
{
    public required string Name { get; set; }
    public required string Address { get; set; }
    public required string ContactPerson { get; set; }
    public required string PhoneNumber { get; set; }
    public required string Email { get; set; }

    public ICollection<SupplierIngredient> SupplierIngredients { get; set; } = new List<SupplierIngredient>();
}
