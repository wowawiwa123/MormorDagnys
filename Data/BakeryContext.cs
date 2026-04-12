using BakeryAPI.Models;
using Microsoft.EntityFrameworkCore;

namespace BakeryAPI.Data;

public class BakeryContext : DbContext
{
    public BakeryContext(DbContextOptions<BakeryContext> options) : base(options) { }

    public DbSet<Supplier> Suppliers { get; set; }
    public DbSet<Ingredient> Ingredients { get; set; }
    public DbSet<SupplierIngredient> SupplierIngredients { get; set; }

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
    }
}
