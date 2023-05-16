using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using MessagePack.Formatters;
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
    public class IndexModel : PageModel
    {
        private readonly WebBurgers.Repository.BurgerContext _context;

        public IndexModel(WebBurgers.Repository.BurgerContext context)
        {
            _context = context;
        }
        public SqlDataReader reader;
        public SelectList Months { get; set; }
        public int? Year;
        public int? SelectedMonth;
        public double TotalSum = 0;

        public async Task OnGetAsync(int? month, int? year)
        {
            if(month == null || year == null)
            {
                month = DateTime.Today.Month;
                year = DateTime.Today.Year;
            }
            Months = new SelectList(await _context.Months.ToListAsync(), "Id", "Month1", month);
            Year = year;
            SelectedMonth = month;
            TotalSum = 0;

            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("ShowSalaries", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@month", month);
            command.Parameters.AddWithValue("@year", year);

            reader = command.ExecuteReader();

           
        }

        public IActionResult OnPost(double totalSum, double year, double month)
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("SP_IssueSalaryFull", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@month", month);
            command.Parameters.AddWithValue("@year", year);
            command.Parameters.AddWithValue("@totalSum", totalSum);

            SqlParameter outputParam = new SqlParameter();
            outputParam.ParameterName = "@k";
            outputParam.SqlDbType = System.Data.SqlDbType.Int;
            outputParam.Direction = System.Data.ParameterDirection.Output;
            command.Parameters.Add(outputParam);

            reader = command.ExecuteReader();

            int res = Convert.ToInt32(outputParam.Value);
            if (res == 2)
            {
                TempData["Message1"] = "Ошибка!";
            }
            else if (res == 0)
            {
                TempData["Message2"] = "Ошибка!";
            }

            reader.Close();

            return RedirectToPage($"./Index", new { month = month, year = year});
        }
    }
}
