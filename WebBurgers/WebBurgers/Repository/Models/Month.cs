using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebBurgers.Repository.Models;

public partial class Month
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    [Column("month")]
    [StringLength(30)]
    public string? Month1 { get; set; }

    [InverseProperty("SalaryMonthNavigation")]
    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();
}
