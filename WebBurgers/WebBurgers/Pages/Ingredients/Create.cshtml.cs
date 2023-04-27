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

namespace WebBurgers.Pages.Ingredients
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
        ViewData["Material"] = new SelectList(_context.Materials, "Id", "Name");
        ViewData["Product"] = new SelectList(_context.Products, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Ingredient Ingredient { get; set; } = default!;
        
        public IActionResult OnPost(int product, int material, decimal count)
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_insertIngredient", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@product", product);
            command.Parameters.AddWithValue("@material", material);
            command.Parameters.AddWithValue("@count", count);

            SqlDataReader reader = command.ExecuteReader();
            reader.Close();
            return RedirectToPage("./Index");
        }
    }
}
