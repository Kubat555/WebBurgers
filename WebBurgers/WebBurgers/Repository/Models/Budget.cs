using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebBurgers.Repository.Models;

[Table("Budget")]
public partial class Budget
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal? Summa { get; set; }

    public double? AddPercent { get; set; }

    public double? EmployeesPercent { get; set; }
}
