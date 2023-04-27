using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebBurgers.Repository.Models;

public partial class Material
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

    [InverseProperty("MaterialNavigation")]
    public virtual ICollection<Ingredient> Ingredients { get; set; } = new List<Ingredient>();

    [InverseProperty("MaterialNavigation")]
    public virtual ICollection<PurchaseMaterial> PurchaseMaterials { get; set; } = new List<PurchaseMaterial>();

    [ForeignKey("Unit")]
    [InverseProperty("Materials")]
    public virtual Unit? UnitNavigation { get; set; }
}
