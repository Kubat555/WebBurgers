using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using WebBurgers.DataBase;
using WebBurgers.Repository;
using WebBurgers.Repository.Models;

namespace WebBurgers.Pages.Salaries
{
    public class EditModel : PageModel
    {
        private readonly WebBurgers.Repository.BurgerContext _context;

        public EditModel(WebBurgers.Repository.BurgerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Salary Salary { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Salaries == null)
            {
                return NotFound();
            }

            var salary =  await _context.Salaries.FirstOrDefaultAsync(m => m.Id == id);
            if (salary == null)
            {
                return NotFound();
            }
            Salary = salary;
           ViewData["EmployeesId"] = new SelectList(_context.Employees, "Id", "Id");
           ViewData["SalaryMonth"] = new SelectList(_context.Months, "Id", "Id");
            return Page();
        }

        public IActionResult OnPost()
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_updateSalary", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", Salary.Id);
            command.Parameters.AddWithValue("@summa", Salary.TotalSalary);

            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            return RedirectToPage($"./Index", new { month = Salary.SalaryMonth, year = Salary.SalaryYear });
        }
    }
}
