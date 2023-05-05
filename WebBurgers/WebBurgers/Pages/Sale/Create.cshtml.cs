using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using WebBurgers.DataBase;
using WebBurgers.Repository;
using WebBurgers.Repository.Models;

namespace WebBurgers.Pages.Sale
{
    public class CreateModel : PageModel
    {
        private readonly WebBurgers.Repository.BurgerContext _context;

        public CreateModel(WebBurgers.Repository.BurgerContext context)
        {
            _context = context;
        }

        public double price = 0;

        [BindProperty]
        public SaleOfProduct SaleOfProduct { get; set; } = default!;
        public IActionResult OnGet(int id)
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_getProductCost", connection);
            command.CommandType = CommandType.StoredProcedure;


            command.Parameters.AddWithValue("@product", id);


            SqlParameter outputParam = new SqlParameter();
            outputParam.ParameterName = "@k";
            outputParam.SqlDbType = System.Data.SqlDbType.Float;
            outputParam.Direction = System.Data.ParameterDirection.Output;
            command.Parameters.Add(outputParam);

            command.ExecuteNonQuery();

            price = Convert.ToDouble(outputParam.Value);

            ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name");
            ViewData["Product"] = new SelectList(_context.Products, "Id", "Name", id);
          
            return Page();
        }

        


        public IActionResult OnPost()
        {

            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("SP_CheckProducts", connection);
            command.CommandType = CommandType.StoredProcedure;


            command.Parameters.AddWithValue("@productID", SaleOfProduct.Product);
            command.Parameters.AddWithValue("@productCount", SaleOfProduct.Count);

            SqlParameter outputParam = new SqlParameter();
            outputParam.ParameterName = "@k";
            outputParam.SqlDbType = System.Data.SqlDbType.Int;
            outputParam.Direction = System.Data.ParameterDirection.Output;
            command.Parameters.Add(outputParam);

            command.ExecuteNonQuery();

            int res = Convert.ToInt32(outputParam.Value);

            if (res == 0)
            {
                TempData["Message"] = "ERROR!";
            }
            else
            {
                command = new SqlCommand("SP_SaleOfProducts", connection);
                command.CommandType = CommandType.StoredProcedure;
                command.Parameters.AddWithValue("@ProductID", SaleOfProduct.Product);
                command.Parameters.AddWithValue("@ProductCount", SaleOfProduct.Count);
                command.Parameters.AddWithValue("@ProductSumma", SaleOfProduct.Summa);
                command.Parameters.AddWithValue("@ProductDate", SaleOfProduct.Date);
                command.Parameters.AddWithValue("@ProductEmployee", SaleOfProduct.Employee);

                SqlDataReader reader = command.ExecuteReader();
                reader.Close();
                return RedirectToPage("./Index");
            }

            return RedirectToPage(new { id = SaleOfProduct.Product });

        }      
        
    }
}
