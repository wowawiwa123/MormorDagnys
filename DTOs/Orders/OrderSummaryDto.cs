namespace BakeryAPI.DTOs.Orders;

public class OrderSummaryDto
{
    public string Id { get; set; } = string.Empty;
    public string OrderNumber { get; set; } = string.Empty;
    public string OrderDate { get; set; } = string.Empty;
    public decimal TotalOrderPrice { get; set; }
}
