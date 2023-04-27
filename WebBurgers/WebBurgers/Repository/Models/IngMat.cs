using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebBurgers.Repository.Models;

[Keyless]
public partial class IngMat
{
    public int? Product { get; set; }

    public int? Material { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? MaterialCount { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Summa { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? IngredientCount { get; set; }
}
