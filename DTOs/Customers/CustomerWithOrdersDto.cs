using BakeryAPI.DTOs.Orders;

namespace BakeryAPI.DTOs.Customers;

public class CustomerWithOrdersDto : CustomerDto
{
    public List<OrderSummaryDto> Orders { get; set; } = new();
}
