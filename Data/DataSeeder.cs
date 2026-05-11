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
            new Supplier { Name = "Nordic Flour Ltd", Address = "12 Industrial Road, Malmö", ContactPerson = "Erik Svensson", PhoneNumber = "040-123456", Email = "erik@nordicflour.com" },
            new Supplier { Name = "Swedish Raw Materials AB", Address = "8 Main Street, Stockholm", ContactPerson = "Anna Lindgren", PhoneNumber = "08-987654", Email = "anna@swedishrawmaterials.com" },
            new Supplier { Name = "Gothenburg Ingredients Co", Address = "3 Harbour Road, Gothenburg", ContactPerson = "Lars Johansson", PhoneNumber = "031-654321", Email = "lars@gbgingredients.com" }
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

        var products = new List<Product>
        {
            new Product { Name = "Sourdough Bread", PricePerUnit = 45.00m, WeightGrams = 800, UnitsPerPackage = 1, BestBefore = DateTime.Now.AddDays(5), ManufacturedDate = DateTime.Now },
            new Product { Name = "Cinnamon Buns (6-pack)", PricePerUnit = 59.00m, WeightGrams = 480, UnitsPerPackage = 6, BestBefore = DateTime.Now.AddDays(3), ManufacturedDate = DateTime.Now },
            new Product { Name = "Semla 2-pack", PricePerUnit = 49.00m, WeightGrams = 300, UnitsPerPackage = 2, BestBefore = DateTime.Now.AddDays(2), ManufacturedDate = DateTime.Now },
            new Product { Name = "Rye Crisp Bread", PricePerUnit = 35.00m, WeightGrams = 500, UnitsPerPackage = 1, BestBefore = DateTime.Now.AddDays(60), ManufacturedDate = DateTime.Now },
        };

        await context.Products.AddRangeAsync(products);
        await context.SaveChangesAsync();

        var customers = new List<Customer>
        {
            new Customer { StoreName = "ICA Maxi Helsingborg", Phone = "042-111222", Email = "ica.maxi@helsingborg.se", ContactPerson = "Maria Persson", DeliveryAddress = "Storgatan 1, Helsingborg", InvoiceAddress = "Box 100, Helsingborg" },
            new Customer { StoreName = "Coop Malmö City", Phone = "040-333444", Email = "coop.malmo@city.se", ContactPerson = "Johan Berg", DeliveryAddress = "Södergatan 5, Malmö", InvoiceAddress = "Box 200, Malmö" },
        };

        await context.Customers.AddRangeAsync(customers);
        await context.SaveChangesAsync();

        var orders = new List<Order>
        {
            new Order { OrderNumber = "ORD-2024-001", OrderDate = DateTime.Now.AddDays(-10), CustomerId = customers[0].Id },
            new Order { OrderNumber = "ORD-2024-002", OrderDate = DateTime.Now.AddDays(-5), CustomerId = customers[1].Id },
            new Order { OrderNumber = "ORD-2024-003", OrderDate = DateTime.Now.AddDays(-2), CustomerId = customers[0].Id },
        };

        await context.Orders.AddRangeAsync(orders);
        await context.SaveChangesAsync();

        var orderItems = new List<OrderItem>
        {
            new OrderItem { OrderId = orders[0].Id, ProductId = products[0].Id, Quantity = 10, UnitPrice = products[0].PricePerUnit },
            new OrderItem { OrderId = orders[0].Id, ProductId = products[1].Id, Quantity = 5, UnitPrice = products[1].PricePerUnit },
            new OrderItem { OrderId = orders[1].Id, ProductId = products[2].Id, Quantity = 20, UnitPrice = products[2].PricePerUnit },
            new OrderItem { OrderId = orders[2].Id, ProductId = products[0].Id, Quantity = 8, UnitPrice = products[0].PricePerUnit },
            new OrderItem { OrderId = orders[2].Id, ProductId = products[3].Id, Quantity = 15, UnitPrice = products[3].PricePerUnit },
        };

        await context.OrderItems.AddRangeAsync(orderItems);
        await context.SaveChangesAsync();
    }
}
