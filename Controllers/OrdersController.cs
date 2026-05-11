using BakeryAPI.Data;
using BakeryAPI.DTOs.Customers;
using BakeryAPI.DTOs.Orders;
using BakeryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryAPI.Controllers;

[Route("api/orders")]
[ApiController]
public class OrdersController(BakeryContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> ListAllOrders()
    {
        try
        {
            var orders = await context.Orders
                .Include(o => o.OrderItems)
                .Select(o => new OrderSummaryDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate.ToShortDateString(),
                    TotalOrderPrice = o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)
                })
                .ToListAsync();

            return Ok(new { Success = true, StatusCode = 200, Items = orders.Count, Data = orders });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while fetching orders.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindOrder(string id)
    {
        try
        {
            Order order = await context.Orders
                .Include(o => o.Customer)
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .SingleOrDefaultAsync(o => o.Id == id);

            if (order is null) return NotFound($"Order with id {id} was not found.");

            var data = new OrderDetailDto
            {
                Id = order.Id,
                OrderNumber = order.OrderNumber,
                OrderDate = order.OrderDate.ToShortDateString(),
                Customer = new CustomerDto
                {
                    Id = order.Customer.Id,
                    StoreName = order.Customer.StoreName,
                    Phone = order.Customer.Phone,
                    Email = order.Customer.Email,
                    ContactPerson = order.Customer.ContactPerson,
                    DeliveryAddress = order.Customer.DeliveryAddress,
                    InvoiceAddress = order.Customer.InvoiceAddress
                },
                Items = order.OrderItems.Select(oi => new OrderItemDto
                {
                    ProductId = oi.ProductId,
                    ProductName = oi.Product.Name,
                    Quantity = oi.Quantity,
                    UnitPrice = oi.UnitPrice,
                    TotalPrice = oi.Quantity * oi.UnitPrice
                }).ToList(),
                TotalOrderPrice = order.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)
            };

            return Ok(new { Success = true, StatusCode = 200, Data = data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while fetching the order.");
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult> SearchOrders([FromQuery] string q)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Please provide a search term via ?q=");

            var orders = await context.Orders
                .Include(o => o.OrderItems)
                .Where(o => o.OrderNumber.ToLower().Contains(q.ToLower())
                         || o.OrderDate.ToString().Contains(q))
                .Select(o => new OrderSummaryDto
                {
                    Id = o.Id,
                    OrderNumber = o.OrderNumber,
                    OrderDate = o.OrderDate.ToShortDateString(),
                    TotalOrderPrice = o.OrderItems.Sum(oi => oi.Quantity * oi.UnitPrice)
                })
                .ToListAsync();

            if (!orders.Any())
                return NotFound($"No orders found matching '{q}'.");

            return Ok(new { Success = true, StatusCode = 200, Items = orders.Count, Data = orders });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while searching orders.");
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddOrder(AddOrderDto model)
    {
        try
        {
            Customer customer = await context.Customers.FindAsync(model.CustomerId);
            if (customer is null) return NotFound($"Customer with id {model.CustomerId} was not found.");

            if (model.Items is null || !model.Items.Any())
                return BadRequest("An order must contain at least one item.");

            string orderNumber = $"ORD-{DateTime.Now:yyyy}-{DateTime.Now:MMddHHmmss}";

            Order order = new()
            {
                OrderNumber = orderNumber,
                OrderDate = DateTime.Now,
                CustomerId = customer.Id
            };

            context.Orders.Add(order);
            await context.SaveChangesAsync();

            foreach (var item in model.Items)
            {
                Product product = await context.Products.FindAsync(item.ProductId);
                if (product is null) return NotFound($"Product with id {item.ProductId} was not found.");

                OrderItem orderItem = new()
                {
                    OrderId = order.Id,
                    ProductId = product.Id,
                    Quantity = item.Quantity,
                    UnitPrice = product.PricePerUnit
                };

                context.OrderItems.Add(orderItem);
            }

            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(FindOrder), new { id = order.Id },
                new { Success = true, Message = $"Order '{orderNumber}' created for '{customer.StoreName}'.", OrderId = order.Id, OrderNumber = orderNumber });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while creating the order.");
        }
    }
}
