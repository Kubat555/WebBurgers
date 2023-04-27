using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebBurgers.Repository.Models;

[Keyless]
public partial class IngredientsView
{
    [Column("ID")]
    public int Id { get; set; }

    [Column("ProductID")]
    public int? ProductId { get; set; }

    [StringLength(50)]
    public string? Product { get; set; }

    [StringLength(50)]
    public string? Material { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Count { get; set; }
}
