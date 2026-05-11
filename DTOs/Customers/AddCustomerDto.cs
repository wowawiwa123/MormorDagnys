using System.ComponentModel.DataAnnotations;

namespace BakeryAPI.DTOs.Customers;

public class AddCustomerDto
{
    [Required]
    public string StoreName { get; set; } = string.Empty;
    [Required]
    public string Phone { get; set; } = string.Empty;
    [Required]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;
    [Required]
    public string ContactPerson { get; set; } = string.Empty;
    [Required]
    public string DeliveryAddress { get; set; } = string.Empty;
    [Required]
    public string InvoiceAddress { get; set; } = string.Empty;
}
