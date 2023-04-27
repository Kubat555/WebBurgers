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

namespace WebBurgers.Pages.Materials
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
        ViewData["Unit"] = new SelectList(_context.Units, "Id", "Name");
            return Page();
        }

        [BindProperty]
        public Material Material { get; set; } = default!;


        public IActionResult OnPost()
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_insertMaterial", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@name", Material.Name);
            command.Parameters.AddWithValue("@unit", Material.Unit);
            command.Parameters.AddWithValue("@count", Material.Count);
            command.Parameters.AddWithValue("@summa", Material.Summa);

            SqlDataReader reader = command.ExecuteReader();
            reader.Close();
            return RedirectToPage("./Index");
        }
    }
}
