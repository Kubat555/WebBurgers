using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;

namespace WebBurgers.Repository.Models;

public partial class Employee
{
    [Key]
    [Column("ID")]
    public int Id { get; set; }

    [StringLength(50)]
    public string? Name { get; set; }

    public int? Job { get; set; }

    [Column(TypeName = "decimal(10, 2)")]
    public decimal? Salary { get; set; }

    [StringLength(50)]
    public string? Address { get; set; }

    [StringLength(50)]
    public string? Phone { get; set; }

    [ForeignKey("Job")]
    [InverseProperty("Employees")]
    public virtual Job? JobNavigation { get; set; }

    [InverseProperty("EmployeeNavigation")]
    public virtual ICollection<ProductionOfProduct> ProductionOfProducts { get; set; } = new List<ProductionOfProduct>();

    [InverseProperty("EmployeeNavigation")]
    public virtual ICollection<PurchaseMaterial> PurchaseMaterials { get; set; } = new List<PurchaseMaterial>();

    [InverseProperty("Employees")]
    public virtual ICollection<Salary> Salaries { get; set; } = new List<Salary>();

    [InverseProperty("EmployeeNavigation")]
    public virtual ICollection<SaleOfProduct> SaleOfProducts { get; set; } = new List<SaleOfProduct>();
}
