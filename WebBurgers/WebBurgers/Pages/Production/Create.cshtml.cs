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

namespace WebBurgers.Pages.Production
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
        ViewData["Product"] = new SelectList(_context.Products, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public ProductionOfProduct ProductionOfProduct { get; set; } = default!;


        public IActionResult OnPost()
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("SP_CheckMaterials", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@product", ProductionOfProduct.Product);
            command.Parameters.AddWithValue("@s", ProductionOfProduct.Count);

            SqlParameter outputParam = new SqlParameter();
            outputParam.ParameterName = "@k";
            outputParam.SqlDbType = System.Data.SqlDbType.Int;
            outputParam.Direction = System.Data.ParameterDirection.Output;
            command.Parameters.Add(outputParam);

            command.ExecuteNonQuery();

            int res = Convert.ToInt32(outputParam.Value);
            if (res == 0)
            {
                TempData["Message"] = "Не достаточно ингредиентов!";
            }
            else
            {
                command = new SqlCommand("SP_ProductionOfProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@product", ProductionOfProduct.Product);
                command.Parameters.AddWithValue("@count", ProductionOfProduct.Count);
                command.Parameters.AddWithValue("@date", ProductionOfProduct.Date);
                command.Parameters.AddWithValue("@employee", ProductionOfProduct.Employee);

                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
                return RedirectToPage("./Index");
            }
            return RedirectToPage();
        }
    }
}
