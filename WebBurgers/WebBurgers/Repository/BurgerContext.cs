using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using WebBurgers.Repository.Models;

namespace WebBurgers.Repository;

public partial class BurgerContext : DbContext
{
    public BurgerContext()
    {
    }

    public BurgerContext(DbContextOptions<BurgerContext> options)
        : base(options)
    {
    }

    public virtual DbSet<Budget> Budgets { get; set; }

    public virtual DbSet<Employee> Employees { get; set; }

    public virtual DbSet<IngMat> IngMats { get; set; }

    public virtual DbSet<Ingredient> Ingredients { get; set; }

    public virtual DbSet<IngredientsView> IngredientsViews { get; set; }

    public virtual DbSet<Job> Jobs { get; set; }

    public virtual DbSet<Material> Materials { get; set; }

    public virtual DbSet<Month> Months { get; set; }

    public virtual DbSet<Product> Products { get; set; }

    public virtual DbSet<ProductionOfProduct> ProductionOfProducts { get; set; }

    public virtual DbSet<PurchaseMaterial> PurchaseMaterials { get; set; }

    public virtual DbSet<Salary> Salaries { get; set; }

    public virtual DbSet<Sale> Sales { get; set; }

    public virtual DbSet<SaleOfProduct> SaleOfProducts { get; set; }

    public virtual DbSet<Unit> Units { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
#warning To protect potentially sensitive information in your connection string, you should move it out of source code. You can avoid scaffolding the connection string by using the Name= syntax to read it from configuration - see https://go.microsoft.com/fwlink/?linkid=2131148. For more guidance on storing connection strings, see http://go.microsoft.com/fwlink/?LinkId=723263.
        => optionsBuilder.UseSqlServer("Data Source=KUBAT;Initial Catalog=Burgers;Trust Server Certificate=True;Integrated Security=True;User ID = sa;Password=Kubat555;");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Employee>(entity =>
        {
            entity.HasOne(d => d.JobNavigation).WithMany(p => p.Employees).HasConstraintName("FK_Employees_Jobs");
        });

        modelBuilder.Entity<IngMat>(entity =>
        {
            entity.ToView("IngMat");
        });

        modelBuilder.Entity<Ingredient>(entity =>
        {
            entity.HasOne(d => d.MaterialNavigation).WithMany(p => p.Ingredients).HasConstraintName("FK_Ingredients_Materials");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.Ingredients).HasConstraintName("FK_Ingredients_Products");
        });

        modelBuilder.Entity<IngredientsView>(entity =>
        {
            entity.ToView("IngredientsView");
        });

        modelBuilder.Entity<Job>(entity =>
        {
            entity.HasKey(e => e.Id).HasName("PK_Job");
        });

        modelBuilder.Entity<Material>(entity =>
        {
            entity.HasOne(d => d.UnitNavigation).WithMany(p => p.Materials).HasConstraintName("FK_Materials_Units");
        });

        modelBuilder.Entity<Month>(entity =>
        {
            entity.Property(e => e.Month1).IsFixedLength();
        });

        modelBuilder.Entity<Product>(entity =>
        {
            entity.HasOne(d => d.UnitNavigation).WithMany(p => p.Products).HasConstraintName("FK_Products_Units");
        });

        modelBuilder.Entity<ProductionOfProduct>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("InsertOnProduction"));

            entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.ProductionOfProducts).HasConstraintName("FK_ProductionOfProducts_Employees");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.ProductionOfProducts).HasConstraintName("FK_ProductionOfProducts_Products");
        });

        modelBuilder.Entity<PurchaseMaterial>(entity =>
        {
            entity.ToTable("PurchaseMaterial", tb => tb.HasTrigger("PurchaseMaterialOnInsert"));

            entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.PurchaseMaterials).HasConstraintName("FK_PurchaseMaterial_Employees");

            entity.HasOne(d => d.MaterialNavigation).WithMany(p => p.PurchaseMaterials).HasConstraintName("FK_PurchaseMaterial_Materials");
        });

        modelBuilder.Entity<Salary>(entity =>
        {
            entity.Property(e => e.Issued).HasDefaultValueSql("((0))");

            entity.HasOne(d => d.Employees).WithMany(p => p.Salaries).HasConstraintName("FK_Salary_Employees");

            entity.HasOne(d => d.SalaryMonthNavigation).WithMany(p => p.Salaries).HasConstraintName("FK_Salary_Months");
        });

        modelBuilder.Entity<Sale>(entity =>
        {
            entity.ToView("Sales");
        });

        modelBuilder.Entity<SaleOfProduct>(entity =>
        {
            entity.ToTable(tb => tb.HasTrigger("Insertsale"));

            entity.HasOne(d => d.EmployeeNavigation).WithMany(p => p.SaleOfProducts).HasConstraintName("FK_SaleOfProducts_Employees");

            entity.HasOne(d => d.ProductNavigation).WithMany(p => p.SaleOfProducts).HasConstraintName("FK_SaleOfProducts_Products");
        });

        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
