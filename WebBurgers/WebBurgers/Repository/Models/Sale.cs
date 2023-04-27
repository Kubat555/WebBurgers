using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebBurgers.Repository.Models;

[Keyless]
public partial class Sale
{
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string? Name { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Summa { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? Date { get; set; }

    [StringLength(50)]
    public string? Employee { get; set; }
}
