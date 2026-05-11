using System.ComponentModel.DataAnnotations;

namespace BakeryAPI.Models;

public abstract class BaseEntity
{
    [Key]
    public string Id { get; set; } =
        Guid.NewGuid().ToString().Replace("-", "");
}
