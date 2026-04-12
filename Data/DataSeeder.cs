using BakeryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BakeryAPI.Data;

public static class DataSeeder
{
    public static async Task SeedAsync(BakeryContext context)
    {
        
        if (await context.Suppliers.AnyAsync() || await context.Ingredients.AnyAsync())
            return;

        var suppliers = new List<Supplier>
        {
            new Supplier
            {
                Name = "Nordic Flour Ltd",
                Address = "12 Industrial Road, Malmö",
                ContactPerson = "Erik Svensson",
                PhoneNumber = "040-123456",
                Email = "erik@nordicflour.com"
            },
            new Supplier
            {
                Name = "Swedish Raw Materials AB",
                Address = "8 Main Street, Stockholm",
                ContactPerson = "Anna Lindgren",
                PhoneNumber = "08-987654",
                Email = "anna@swedishrawmaterials.com"
            },
            new Supplier
            {
                Name = "Gothenburg Ingredients Co",
                Address = "3 Harbour Road, Gothenburg",
                ContactPerson = "Lars Johansson",
                PhoneNumber = "031-654321",
                Email = "lars@gbgingredients.com"
            }
        };

        var ingredients = new List<Ingredient>
        {
            new Ingredient { ArticleNumber = "FL001", Name = "Wheat Flour" },
            new Ingredient { ArticleNumber = "SU002", Name = "Sugar" },
            new Ingredient { ArticleNumber = "BU003", Name = "Butter" },
            new Ingredient { ArticleNumber = "SA004", Name = "Salt" },
            new Ingredient { ArticleNumber = "YE005", Name = "Yeast" }
        };

        await context.Suppliers.AddRangeAsync(suppliers);
        await context.Ingredients.AddRangeAsync(ingredients);
        await context.SaveChangesAsync();

        
        var links = new List<SupplierIngredient>
        {
            
            new SupplierIngredient { SupplierId = suppliers[0].Id, IngredientId = ingredients[0].Id, PricePerKg = 8.50m },
            new SupplierIngredient { SupplierId = suppliers[1].Id, IngredientId = ingredients[0].Id, PricePerKg = 9.00m },
            new SupplierIngredient { SupplierId = suppliers[2].Id, IngredientId = ingredients[0].Id, PricePerKg = 8.75m },

            
            new SupplierIngredient { SupplierId = suppliers[0].Id, IngredientId = ingredients[1].Id, PricePerKg = 12.00m },
            new SupplierIngredient { SupplierId = suppliers[2].Id, IngredientId = ingredients[1].Id, PricePerKg = 11.50m },

           
            new SupplierIngredient { SupplierId = suppliers[1].Id, IngredientId = ingredients[2].Id, PricePerKg = 85.00m },
            new SupplierIngredient { SupplierId = suppliers[2].Id, IngredientId = ingredients[2].Id, PricePerKg = 82.50m },

          
            new SupplierIngredient { SupplierId = suppliers[0].Id, IngredientId = ingredients[3].Id, PricePerKg = 3.50m },
            new SupplierIngredient { SupplierId = suppliers[1].Id, IngredientId = ingredients[3].Id, PricePerKg = 4.00m },

           
            new SupplierIngredient { SupplierId = suppliers[1].Id, IngredientId = ingredients[4].Id, PricePerKg = 25.00m },
            new SupplierIngredient { SupplierId = suppliers[2].Id, IngredientId = ingredients[4].Id, PricePerKg = 23.50m },
        };

        await context.SupplierIngredients.AddRangeAsync(links);
        await context.SaveChangesAsync();
    }
}
