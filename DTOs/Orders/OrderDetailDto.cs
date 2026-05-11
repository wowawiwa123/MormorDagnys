using BakeryAPI.DTOs.Customers;

namespace BakeryAPI.DTOs.Orders;

public class OrderDetailDto
{
    public string Id { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string OrderDate { get; set; } = string.Empty;
    public CustomerDto Customer { get; set; } = null!;
    public List<OrderItemDto> Items { get; set; } = new();
    public decimal TotalOrderPrice { get; set; }
}
