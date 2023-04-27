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

namespace WebBurgers.Pages.Sale
{
    public class EditModel : PageModel
    {
        private readonly WebBurgers.Repository.BurgerContext _context;

        public EditModel(WebBurgers.Repository.BurgerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public SaleOfProduct SaleOfProduct { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.SaleOfProducts == null)
            {
                return NotFound();
            }

            var saleofproduct =  await _context.SaleOfProducts.FirstOrDefaultAsync(m => m.Id == id);
            if (saleofproduct == null)
            {
                return NotFound();
            }
            SaleOfProduct = saleofproduct;
           ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name");
           ViewData["Product"] = new SelectList(_context.Products, "Id", "Name");
            return Page();
        }
        public IActionResult OnPost()
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_updateSale", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", SaleOfProduct.Id);
            command.Parameters.AddWithValue("@product", SaleOfProduct.Product);
            command.Parameters.AddWithValue("@count", SaleOfProduct.Count);
            command.Parameters.AddWithValue("@summa", SaleOfProduct.Summa);
            command.Parameters.AddWithValue("@date", SaleOfProduct.Date);
            command.Parameters.AddWithValue("@employee", SaleOfProduct.Employee);

            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            return RedirectToPage($"./Index");
        }
    }
}
