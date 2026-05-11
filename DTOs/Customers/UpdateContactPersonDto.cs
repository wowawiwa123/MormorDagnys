using System.ComponentModel.DataAnnotations;

namespace BakeryAPI.DTOs.Customers;

public class UpdateContactPersonDto
{
    [Required]
    public string ContactPerson { get; set; } = string.Empty;
}
