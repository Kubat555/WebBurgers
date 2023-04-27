using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebBurgers.Repository.Models;

[Table("Salary")]
public partial class Salary
{
    [Key]
    [Column("id")]
    public int Id { get; set; }

    public int? EmployeesId { get; set; }

    public long? SalaryYear { get; set; }

    public int? SalaryMonth { get; set; }

    public int? Purchase { get; set; }

    public int? Production { get; set; }

    public int? Sale { get; set; }

    public int? TotalCount { get; set; }

    [Column("Salary")]
    public double? Salary1 { get; set; }

    public double? Bonus { get; set; }

    public double? TotalSalary { get; set; }

    public bool? Issued { get; set; }

    [ForeignKey("EmployeesId")]
    [InverseProperty("Salaries")]
    public virtual Employee? Employees { get; set; }

    [ForeignKey("SalaryMonth")]
    [InverseProperty("Salaries")]
    public virtual Month? SalaryMonthNavigation { get; set; }
}
