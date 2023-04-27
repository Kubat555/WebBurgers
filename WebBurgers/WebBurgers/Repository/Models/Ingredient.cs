using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebBurgers.Repository.Models;

[Index("Material", "Product", Name = "IX_Ingredients", IsUnique = true)]
public partial class Ingredient
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int? Product { get; set; }

    public int? Material { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Count { get; set; }

    [ForeignKey("Material")]
    [InverseProperty("Ingredients")]
    public virtual Material? MaterialNavigation { get; set; }

    [ForeignKey("Product")]
    [InverseProperty("Ingredients")]
    public virtual Product? ProductNavigation { get; set; }
}
