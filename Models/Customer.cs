namespace BakeryAPI.Models;

public class Customer : BaseEntity
{
    public required string StoreName { get; set; }
    public required string Phone { get; set; }
    public required string Email { get; set; }
    public required string ContactPerson { get; set; }
    public required string DeliveryAddress { get; set; }
    public required string InvoiceAddress { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
