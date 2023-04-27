using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using WebBurgers.DataBase;
using WebBurgers.Repository;
using WebBurgers.Repository.Models;

namespace WebBurgers.Pages.PurchaseMaterials
{
    public class CreateModel : PageModel
    {
        private readonly WebBurgers.Repository.BurgerContext _context;

        public CreateModel(WebBurgers.Repository.BurgerContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name");
        ViewData["Material"] = new SelectList(_context.Materials, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public PurchaseMaterial PurchaseMaterial { get; set; } = default!;


        public IActionResult OnPost()
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("SP_CheckBudget", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@summ", PurchaseMaterial.Summa);

            SqlParameter outputParam = new SqlParameter();
            outputParam.ParameterName = "@p";
            outputParam.SqlDbType = System.Data.SqlDbType.Int;
            outputParam.Direction = System.Data.ParameterDirection.Output;
            command.Parameters.Add(outputParam);

            command.ExecuteNonQuery();

            int res = Convert.ToInt32(outputParam.Value);
            if (res == 0)
            {
                TempData["Message"] = "Не достаточно бюджета!";
            }
            else
            {
                command = new SqlCommand("SP_PurchaseMaterial", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@MaterialID", PurchaseMaterial.Material);
                command.Parameters.AddWithValue("@MaterialCount", PurchaseMaterial.Count);
                command.Parameters.AddWithValue("@MaterialSumma", PurchaseMaterial.Summa);
                command.Parameters.AddWithValue("@MaterialDate", PurchaseMaterial.Date);
                command.Parameters.AddWithValue("@Employee", PurchaseMaterial.Employee);

                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
                return RedirectToPage("./Index");
            }
            return RedirectToPage();
        }
    }
}
