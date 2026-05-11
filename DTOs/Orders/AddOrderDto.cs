using System.ComponentModel.DataAnnotations;

namespace BakeryAPI.DTOs.Orders;

public class AddOrderDto
{
    [Required]
    public string CustomerId { get; set; } = string.Empty;
    public List<AddOrderItemDto> Items { get; set; } = new();
}
