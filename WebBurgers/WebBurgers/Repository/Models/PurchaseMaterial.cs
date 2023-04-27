using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebBurgers.Repository.Models;

[Table("PurchaseMaterial")]
public partial class PurchaseMaterial
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    public int? Material { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Count { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Summa { get; set; }

    [Column(TypeName = "smalldatetime")]
    public DateTime? Date { get; set; }

    public int? Employee { get; set; }

    [ForeignKey("Employee")]
    [InverseProperty("PurchaseMaterials")]
    public virtual Employee? EmployeeNavigation { get; set; }

    [ForeignKey("Material")]
    [InverseProperty("PurchaseMaterials")]
    public virtual Material? MaterialNavigation { get; set; }
}
