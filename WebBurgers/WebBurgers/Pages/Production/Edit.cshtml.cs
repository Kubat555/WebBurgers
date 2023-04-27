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

namespace WebBurgers.Pages.Production
{
    public class EditModel : PageModel
    {
        private readonly WebBurgers.Repository.BurgerContext _context;

        public EditModel(WebBurgers.Repository.BurgerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public ProductionOfProduct ProductionOfProduct { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.ProductionOfProducts == null)
            {
                return NotFound();
            }

            var productionofproduct =  await _context.ProductionOfProducts.FirstOrDefaultAsync(m => m.Id == id);
            if (productionofproduct == null)
            {
                return NotFound();
            }
            ProductionOfProduct = productionofproduct;
           ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name");
           ViewData["Product"] = new SelectList(_context.Products, "Id", "Name");
            return Page();
        }

        public IActionResult OnPost()
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_updateProduction", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", ProductionOfProduct.Id);
            command.Parameters.AddWithValue("@product", ProductionOfProduct.Product);
            command.Parameters.AddWithValue("@count", ProductionOfProduct.Count);
            command.Parameters.AddWithValue("@date", ProductionOfProduct.Date);
            command.Parameters.AddWithValue("@employee", ProductionOfProduct.Employee);

            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            return RedirectToPage($"./Index");
        }
    }
}
