using System.ComponentModel.DataAnnotations.Schema;

namespace BakeryAPI.Models;

public class Order : BaseEntity
{
    public required string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; } = DateTime.Now;

    public required string CustomerId { get; set; }
    [ForeignKey("CustomerId")]
    public Customer Customer { get; set; } = null!;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
