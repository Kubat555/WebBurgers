using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebBurgers.Repository.Models;

public partial class SaleOfProduct
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int? Product { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Count { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Summa { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? Date { get; set; }

    public int? Employee { get; set; }

    [ForeignKey("Employee")]
    [InverseProperty("SaleOfProducts")]
    public virtual Employee? EmployeeNavigation { get; set; }

    [ForeignKey("Product")]
    [InverseProperty("SaleOfProducts")]
    public virtual Product? ProductNavigation { get; set; }
}
