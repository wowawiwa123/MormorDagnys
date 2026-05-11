using BakeryAPI.Data;
using BakeryAPI.DTOs.Ingredients;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryAPI.Controllers;

[Route("api/ingredients")]
[ApiController]
public class IngredientsController(BakeryContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> ListAllIngredients()
    {
        try
        {
            var ingredients = await context.Ingredients
                .Include(i => i.SupplierIngredients)
                    .ThenInclude(si => si.Supplier)
                .Select(i => new IngredientDto
                {
                    Id = i.Id,
                    ArticleNumber = i.ArticleNumber,
                    Name = i.Name,
                    Suppliers = i.SupplierIngredients
                        .OrderBy(si => si.PricePerKg)
                        .Select(si => new SupplierPriceDto
                        {
                            SupplierId = si.SupplierId,
                            Name = si.Supplier.Name,
                            ContactPerson = si.Supplier.ContactPerson,
                            PhoneNumber = si.Supplier.PhoneNumber,
                            Email = si.Supplier.Email,
                            PricePerKg = si.PricePerKg
                        }).ToList()
                })
                .ToListAsync();

            return Ok(new { Success = true, StatusCode = 200, Items = ingredients.Count, Data = ingredients });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while fetching ingredients.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindIngredient(string id)
    {
        try
        {
            var ingredient = await context.Ingredients
                .Include(i => i.SupplierIngredients)
                    .ThenInclude(si => si.Supplier)
                .SingleOrDefaultAsync(i => i.Id == id);

            if (ingredient is null) return NotFound($"Ingredient with id {id} was not found.");

            var data = new IngredientDto
            {
                Id = ingredient.Id,
                ArticleNumber = ingredient.ArticleNumber,
                Name = ingredient.Name,
                Suppliers = ingredient.SupplierIngredients
                    .OrderBy(si => si.PricePerKg)
                    .Select(si => new SupplierPriceDto
                    {
                        SupplierId = si.SupplierId,
                        Name = si.Supplier.Name,
                        ContactPerson = si.Supplier.ContactPerson,
                        PhoneNumber = si.Supplier.PhoneNumber,
                        Email = si.Supplier.Email,
                        PricePerKg = si.PricePerKg
                    }).ToList()
            };

            return Ok(new { Success = true, StatusCode = 200, Data = data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while fetching the ingredient.");
        }
    }

    [HttpGet("search")]
    public async Task<ActionResult> SearchIngredients([FromQuery] string q)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Please provide a search term via ?q=");

            var ingredients = await context.Ingredients
                .Include(i => i.SupplierIngredients)
                    .ThenInclude(si => si.Supplier)
                .Where(i => i.Name.ToLower().Contains(q.ToLower())
                         || i.ArticleNumber.ToLower().Contains(q.ToLower()))
                .Select(i => new IngredientDto
                {
                    Id = i.Id,
                    ArticleNumber = i.ArticleNumber,
                    Name = i.Name,
                    Suppliers = i.SupplierIngredients
                        .OrderBy(si => si.PricePerKg)
                        .Select(si => new SupplierPriceDto
                        {
                            SupplierId = si.SupplierId,
                            Name = si.Supplier.Name,
                            ContactPerson = si.Supplier.ContactPerson,
                            PhoneNumber = si.Supplier.PhoneNumber,
                            Email = si.Supplier.Email,
                            PricePerKg = si.PricePerKg
                        }).ToList()
                })
                .ToListAsync();

            if (!ingredients.Any())
                return NotFound($"No ingredients found matching '{q}'.");

            return Ok(new { Success = true, StatusCode = 200, Items = ingredients.Count, Data = ingredients });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while searching for ingredients.");
        }
    }
}
