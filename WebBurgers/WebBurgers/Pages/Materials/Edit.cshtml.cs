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

namespace WebBurgers.Pages.Materials
{
    public class EditModel : PageModel
    {
        private readonly WebBurgers.Repository.BurgerContext _context;

        public EditModel(WebBurgers.Repository.BurgerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Material Material { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.Materials == null)
            {
                return NotFound();
            }

            var material =  await _context.Materials.FirstOrDefaultAsync(m => m.Id == id);
            if (material == null)
            {
                return NotFound();
            }
            Material = material;
           ViewData["Unit"] = new SelectList(_context.Units, "Id", "Name");
            return Page();
        }

        public IActionResult OnPost()
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_updateMaterial", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", Material.Id);
            command.Parameters.AddWithValue("@name", Material.Name);
            command.Parameters.AddWithValue("@count", Material.Count);
            command.Parameters.AddWithValue("@summa", Material.Summa);
            command.Parameters.AddWithValue("@unit", Material.Unit);

            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            return RedirectToPage($"./Index");
        }
    }
}
