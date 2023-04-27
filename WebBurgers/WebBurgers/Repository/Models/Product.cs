using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebBurgers.Repository.Models;

public partial class Product
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string? Name { get; set; }

    public int? Unit { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Count { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Summa { get; set; }

    [InverseProperty("ProductNavigation")]
    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    [InverseProperty("ProductNavigation")]
    public virtual ICollection<ProductionOfProduct> ProductionOfProducts { get; set; } = new List<ProductionOfProduct>();

    [InverseProperty("ProductNavigation")]
    public virtual ICollection<SaleOfProduct> SaleOfProducts { get; set; } = new List<SaleOfProduct>();

    [ForeignKey("Unit")]
    [InverseProperty("Products")]
    public virtual Unit? UnitNavigation { get; set; }
}
