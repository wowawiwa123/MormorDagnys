using BakeryAPI.Data;
using BakeryAPI.DTOs.Products;
using BakeryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryAPI.Controllers;

[Route("api/products")]
[ApiController]
public class ProductsController(BakeryContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> ListAllProducts()
    {
        try
        {
            var products = await context.Products
                .Select(p => new ProductDto
                {
                    Id = p.Id,
                    Name = p.Name,
                    PricePerUnit = p.PricePerUnit,
                    WeightGrams = p.WeightGrams,
                    UnitsPerPackage = p.UnitsPerPackage,
                    BestBefore = p.BestBefore.ToShortDateString(),
                    ManufacturedDate = p.ManufacturedDate.ToShortDateString()
                })
                .ToListAsync();

            return Ok(new { Success = true, StatusCode = 200, Items = products.Count, Data = products });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while fetching products.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindProduct(string id)
    {
        try
        {
            Product product = await context.Products.FindAsync(id);
            if (product is null) return NotFound($"Product with id {id} was not found.");

            var data = new ProductDto
            {
                Id = product.Id,
                Name = product.Name,
                PricePerUnit = product.PricePerUnit,
                WeightGrams = product.WeightGrams,
                UnitsPerPackage = product.UnitsPerPackage,
                BestBefore = product.BestBefore.ToShortDateString(),
                ManufacturedDate = product.ManufacturedDate.ToShortDateString()
            };

            return Ok(new { Success = true, StatusCode = 200, Data = data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while fetching the product.");
        }
    }

    [HttpPost]
    public async Task<ActionResult> AddProduct(AddProductDto model)
    {
        try
        {
            Product product = new()
            {
                Name = model.Name,
                PricePerUnit = model.PricePerUnit,
                WeightGrams = model.WeightGrams,
                UnitsPerPackage = model.UnitsPerPackage,
                BestBefore = model.BestBefore,
                ManufacturedDate = model.ManufacturedDate
            };

            context.Products.Add(product);
            await context.SaveChangesAsync();

            return CreatedAtAction(nameof(FindProduct), new { id = product.Id },
                new { Success = true, Message = $"Product '{product.Name}' added.", Id = product.Id });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while adding the product.");
        }
    }

    [HttpPatch("{id}/price")]
    public async Task<ActionResult> UpdateProductPrice(string id, UpdateProductPriceDto model)
    {
        try
        {
            Product product = await context.Products.FindAsync(id);
            if (product is null) return NotFound($"Product with id {id} was not found.");

            var oldPrice = product.PricePerUnit;
            product.PricePerUnit = model.NewPricePerUnit;
            await context.SaveChangesAsync();

            return Ok(new { Success = true, Message = $"Price for '{product.Name}' updated.", OldPrice = oldPrice, NewPrice = model.NewPricePerUnit });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while updating the price.");
        }
    }
}
