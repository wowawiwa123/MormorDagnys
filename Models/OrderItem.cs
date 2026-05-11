using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryAPI.Models;

public class OrderItem : BaseEntity
{
    public required string OrderId { get; set; }
    [ForeignKey("OrderId")]
    public Order Order { get; set; } = null!;

    public required string ProductId { get; set; }
    [ForeignKey("ProductId")]
    public Product Product { get; set; } = null!;

    public int Quantity { get; set; }
    public decimal UnitPrice { get; set; }
}
