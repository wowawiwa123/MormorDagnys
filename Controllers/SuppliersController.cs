using BakeryAPI.Data;
using BakeryAPI.DTOs.Ingredients;
using BakeryAPI.DTOs.Suppliers;
using BakeryAPI.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BakeryAPI.Controllers;

[Route("api/suppliers")]
[ApiController]
public class SuppliersController(BakeryContext context) : ControllerBase
{
    [HttpGet]
    public async Task<ActionResult> ListAllSuppliers()
    {
        try
        {
            var suppliers = await context.Suppliers
                .Select(s => new SupplierDto
                {
                    Id = s.Id,
                    Name = s.Name,
                    Address = s.Address,
                    ContactPerson = s.ContactPerson,
                    PhoneNumber = s.PhoneNumber,
                    Email = s.Email
                })
                .ToListAsync();

            return Ok(new { Success = true, StatusCode = 200, Items = suppliers.Count, Data = suppliers });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while fetching suppliers.");
        }
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> FindSupplier(int id)
    {
        try
        {
            var supplier = await context.Suppliers
                .Include(s => s.SupplierIngredients)
                    .ThenInclude(si => si.Ingredient)
                .SingleOrDefaultAsync(s => s.Id == id);

            if (supplier is null) return NotFound($"Supplier with id {id} was not found.");

            var data = new SupplierWithIngredientsDto
            {
                Id = supplier.Id,
                Name = supplier.Name,
                Address = supplier.Address,
                ContactPerson = supplier.ContactPerson,
                PhoneNumber = supplier.PhoneNumber,
                Email = supplier.Email,
                Ingredients = supplier.SupplierIngredients
                    .OrderBy(si => si.Ingredient.Name)
                    .Select(si => new IngredientInfoDto
                    {
                        IngredientId = si.IngredientId,
                        ArticleNumber = si.Ingredient.ArticleNumber,
                        Name = si.Ingredient.Name,
                        PricePerKg = si.PricePerKg
                    }).ToList()
            };

            return Ok(new { Success = true, StatusCode = 200, Data = data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while fetching the supplier.");
        }
    }

    
    [HttpGet("search")]
    public async Task<ActionResult> SearchSuppliers([FromQuery] string q)
    {
        try
        {
            if (string.IsNullOrWhiteSpace(q))
                return BadRequest("Please provide a search term via ?q=");

            var suppliers = await context.Suppliers
                .Include(s => s.SupplierIngredients)
                    .ThenInclude(si => si.Ingredient)
                .Where(s => s.Name.ToLower().Contains(q.ToLower())
                        || s.ContactPerson.ToLower().Contains(q.ToLower()))
                .ToListAsync();

            if (!suppliers.Any())
                return NotFound($"No suppliers found matching '{q}'.");

            var data = suppliers.Select(s => new SupplierWithIngredientsDto
            {
                Id = s.Id,
                Name = s.Name,
                Address = s.Address,
                ContactPerson = s.ContactPerson,
                PhoneNumber = s.PhoneNumber,
                Email = s.Email,
                Ingredients = s.SupplierIngredients
                    .OrderBy(si => si.Ingredient.Name)
                    .Select(si => new IngredientInfoDto
                    {
                        IngredientId = si.IngredientId,
                        ArticleNumber = si.Ingredient.ArticleNumber,
                        Name = si.Ingredient.Name,
                        PricePerKg = si.PricePerKg
                    }).ToList()
            }).ToList();

            return Ok(new { Success = true, StatusCode = 200, Items = data.Count, Data = data });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while searching for suppliers.");
        }
    }


    [HttpPost("{supplierId}/ingredients")]
    public async Task<ActionResult> AddIngredientToSupplier(int supplierId, AddIngredientToSupplierDto model)
    {
        try
        {
            Supplier supplier = await context.Suppliers.FindAsync(supplierId);
            if (supplier is null) return NotFound($"Supplier with id {supplierId} was not found.");

            Ingredient ingredient = await context.Ingredients.FindAsync(model.IngredientId);
            if (ingredient is null) return NotFound($"Ingredient with id {model.IngredientId} was not found.");

            if (model.PricePerKg <= 0)
                return BadRequest("Price must be greater than 0.");

            bool alreadyExists = await context.SupplierIngredients
                .AnyAsync(si => si.SupplierId == supplierId && si.IngredientId == model.IngredientId);

            if (alreadyExists)
                return Conflict($"Supplier '{supplier.Name}' already carries '{ingredient.Name}'.");

            SupplierIngredient link = new()
            {
                SupplierId = supplierId,
                IngredientId = model.IngredientId,
                PricePerKg = model.PricePerKg
            };

            context.SupplierIngredients.Add(link);
            await context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = $"'{ingredient.Name}' added to supplier '{supplier.Name}' at {model.PricePerKg} kr/kg."
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while adding the ingredient.");
        }
    }


    [HttpPatch("{supplierId}/ingredients/{ingredientId}/price")]
    public async Task<ActionResult> UpdateIngredientPrice(int supplierId, int ingredientId, UpdatePriceDto model)
    {
        try
        {
            if (model.NewPricePerKg <= 0)
                return BadRequest("Price must be greater than 0.");

            SupplierIngredient link = await context.SupplierIngredients
                .Include(si => si.Supplier)
                .Include(si => si.Ingredient)
                .SingleOrDefaultAsync(si => si.SupplierId == supplierId && si.IngredientId == ingredientId);

            if (link is null)
                return NotFound($"Ingredient with id {ingredientId} was not found at supplier with id {supplierId}.");

            var oldPrice = link.PricePerKg;
            link.PricePerKg = model.NewPricePerKg;
            await context.SaveChangesAsync();

            return Ok(new
            {
                Success = true,
                Message = $"Price for '{link.Ingredient.Name}' at '{link.Supplier.Name}' updated.",
                OldPrice = oldPrice,
                NewPrice = model.NewPricePerKg
            });
        }
        catch (Exception ex)
        {
            Console.WriteLine(ex.Message);
            return StatusCode(500, "Something went wrong while updating the price.");
        }
    }
}
