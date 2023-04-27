using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebBurgers.Repository.Models;

public partial class Unit
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(30)]
    public string? Name { get; set; }

    [InverseProperty("UnitNavigation")]
    public virtual ICollection<Material> Materials { get; set; } = new List<Material>();

    [InverseProperty("UnitNavigation")]
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
