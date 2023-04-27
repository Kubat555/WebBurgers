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

namespace WebBurgers.Pages.PurchaseMaterials
{
    public class EditModel : PageModel
    {
        private readonly WebBurgers.Repository.BurgerContext _context;

        public EditModel(WebBurgers.Repository.BurgerContext context)
        {
            _context = context;
        }

        [BindProperty]
        public PurchaseMaterial PurchaseMaterial { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null || _context.PurchaseMaterials == null)
            {
                return NotFound();
            }

            var purchasematerial =  await _context.PurchaseMaterials.FirstOrDefaultAsync(m => m.Id == id);
            if (purchasematerial == null)
            {
                return NotFound();
            }
            PurchaseMaterial = purchasematerial;
           ViewData["Employee"] = new SelectList(_context.Employees, "Id", "Name");
           ViewData["Material"] = new SelectList(_context.Materials, "Id", "Name");
            return Page();
        }

        public IActionResult OnPost()
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_updatePurchase", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", PurchaseMaterial.Id);
            command.Parameters.AddWithValue("@material", PurchaseMaterial.Material);
            command.Parameters.AddWithValue("@count", PurchaseMaterial.Count);
            command.Parameters.AddWithValue("@summa", PurchaseMaterial.Summa);
            command.Parameters.AddWithValue("@date", PurchaseMaterial.Date);
            command.Parameters.AddWithValue("@employee", PurchaseMaterial.Employee);
            
            SqlDataReader reader = command.ExecuteReader();
            reader.Close();

            return RedirectToPage($"./Index");
        }
    }
}
