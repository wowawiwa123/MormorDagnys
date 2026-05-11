namespace BakeryAPI.DTOs.Customers;

public class CustomerDto
{
    public string Id { get; set; } = string.Empty;
    public string StoreName { get; set; } = string.Empty;
    public string Phone { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string ContactPerson { get; set; } = string.Empty;
    public string DeliveryAddress { get; set; } = string.Empty;
    public string InvoiceAddress { get; set; } = string.Empty;
}
