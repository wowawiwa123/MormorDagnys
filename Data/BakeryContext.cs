using BakeryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BakeryAPI.Data;

public class BakeryContext(DbContextOptions<BakeryContext> options) : DbContext(options)
{
    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<SupplierIngredient> SupplierIngredients { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<Product> Products { get; set; }
    public DbSet<Order> Orders { get; set; }
    public DbSet<OrderItem> OrderItems { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SupplierIngredient>()
            .HasKey(si => new { si.SupplierId, si.IngredientId });

        modelBuilder.Entity<SupplierIngredient>()
            .HasOne(si => si.Supplier)
            .WithMany(s => s.SupplierIngredients)
            .HasForeignKey(si => si.SupplierId);

        modelBuilder.Entity<SupplierIngredient>()
            .HasOne(si => si.Ingredient)
            .WithMany(i => i.SupplierIngredients)
            .HasForeignKey(si => si.IngredientId);

        modelBuilder.Entity<SupplierIngredient>()
            .Property(si => si.PricePerKg)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Customer)
            .WithMany(c => c.Orders)
            .HasForeignKey(o => o.CustomerId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Order)
            .WithMany(o => o.OrderItems)
            .HasForeignKey(oi => oi.OrderId);

        modelBuilder.Entity<OrderItem>()
            .HasOne(oi => oi.Product)
            .WithMany(p => p.OrderItems)
            .HasForeignKey(oi => oi.ProductId);

        modelBuilder.Entity<OrderItem>()
            .Property(oi => oi.UnitPrice)
            .HasColumnType("decimal(10,2)");

        modelBuilder.Entity<Product>()
            .Property(p => p.PricePerUnit)
            .HasColumnType("decimal(10,2)");
    }
}
