using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;
using WebBurgers.DataBase;

namespace WebBurgers.Pages.Products
{
    public class CreateModel : PageModel
    {
        private readonly WebBurgers.Repository.BurgerContext db;

        public CreateModel(WebBurgers.Repository.BurgerContext context)
        {
            db = context;
        }
        public IActionResult OnGet()
        {
            ViewData["Unit"] = new SelectList(db.Units, "Id", "Name");
            return Page();
        }
        public IActionResult OnPost(string name, int unit, decimal count, decimal summa)
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_insertProduct", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@name", name);
            command.Parameters.AddWithValue("@unit", unit);
            command.Parameters.AddWithValue("@count", count);
            command.Parameters.AddWithValue("@summa", summa);

            SqlDataReader reader = command.ExecuteReader();
            reader.Close();
            return RedirectToPage("./Product");
        }
    }
}
