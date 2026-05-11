using BakeryAPI.Data;
using BakeryAPI.DTOs.Customers;
using BakeryAPI.DTOs.Orders;
using BakeryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryAPI.Controllers;

[Route("api/customers")]
[ApiController]
public class CustomersController(BakeryContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> ListAllCustomers()
    {
        try
        {
            var customers = await context.Customers
                .Select(c => new CustomerDto
                {
                    Id = c.Id,
                    StoreName = c.StoreName,
                    Phone = c.Phone,
                    Email = c.Email,
                    ContactPerson = c.ContactPerson,
                    DeliveryAddress = c.DeliveryAddress,
                    InvoiceAddress = c.InvoiceAddress
                })
                .ToListAsync();

            return Ok(new { Success = true, StatusCode = 200, Items = customers.Count, Data = customers });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while fetching customers.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindCustomer(string id)
    {
        try
        {
            Customer customer = await context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                .SingleOrDefaultAsync(c => c.Id == id);

            if (customer is null) return NotFound($"Customer with id {id} was not found.");

            var data = new CustomerWithOrdersDto
            {
                Id = customer.Id,
                StoreName = customer.StoreName,
                Phone = customer.Phone,
                Email = customer.Email,
                ContactPerson = customer.ContactPerson,
                DeliveryAddress = customer.DeliveryAddress,
                InvoiceAddress = customer.InvoiceAddress,
                Orders = customer.Orders.Select(o => new OrderSummaryDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate.ToShortDateString(),
                    TotalOrderPrice = o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)
                }).OrderByDescending(o => o.OrderDate).ToList()
            };

            return Ok(new { Success = true, StatusCode = 200, Data = data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while fetching the customer.");
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddCustomer(AddCustomerDto model)
    {
        try
        {
            Customer customer = new()
            {
                StoreName = model.StoreName,
                Phone = model.Phone,
                Email = model.Email,
                ContactPerson = model.ContactPerson,
                DeliveryAddress = model.DeliveryAddress,
                InvoiceAddress = model.InvoiceAddress
            };

            context.Customers.Add(customer);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(FindCustomer), new { id = customer.Id },
                new { Success = true, Message = $"Customer '{customer.StoreName}' added.", Id = customer.Id });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while adding the customer.");
        }
    }

    [HttpPatch("{id}/contactperson")]
    public async Task<ActionResult> UpdateContactPerson(string id, UpdateContactPersonDto model)
    {
        try
        {
            Customer customer = await context.Customers.FindAsync(id);
            if (customer is null) return NotFound($"Customer with id {id} was not found.");

            var oldContact = customer.ContactPerson;
            customer.ContactPerson = model.ContactPerson;
            await context.SaveChangesAsync();

            return Ok(new { Success = true, Message = $"Contact person for '{customer.StoreName}' updated.", OldContactPerson = oldContact, NewContactPerson = model.ContactPerson });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while updating the contact person.");
        }
    }

    [HttpGet("product-purchases")]
    public async Task<ActionResult> GetCustomerProductPurchases()
    {
        try
        {
            var data = await context.Customers
                .Include(c => c.Orders)
                    .ThenInclude(o => o.OrderItems)
                        .ThenInclude(oi => oi.Product)
                .Select(c => new
                {
                    CustomerId = c.Id,
                    StoreName = c.StoreName,
                    ContactPerson = c.ContactPerson,
                    ProductsPurchased = c.Orders
                        .SelectMany(o => o.OrderItems)
                        .GroupBy(oi => new { oi.ProductId, oi.Product.Name, oi.Product.PricePerUnit })
                        .Select(g => new
                        {
                            ProductId = g.Key.ProductId,
                            ProductName = g.Key.Name,
                            PricePerUnit = g.Key.PricePerUnit,
                            TotalQuantityOrdered = g.Sum(oi => oi.Quantity)
                        }).ToList()
                })
                .ToListAsync();

            return Ok(new { Success = true, StatusCode = 200, Items = data.Count, Data = data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while fetching customer product purchases.");
        }
    }
}
