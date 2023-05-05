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

namespace WebBurgers.Pages.Ingredients
{
    public class IndexModel : PageModel
    {
        private readonly WebBurgers.Repository.BurgerContext db;
        [BindProperty]
        public int SelectedValue { get; set; }
        public IndexModel(WebBurgers.Repository.BurgerContext context)
        {
            db = context;
        }

        public SqlDataReader reader;

       

        public SelectList ProductsList { get; set; }

        public async Task OnGetAsync(string selectedValue)
        {
            SelectedValue = Convert.ToInt32(selectedValue);
            ProductsList = new SelectList(await db.Products.ToListAsync(), "Id", "Name");
            if(selectedValue == null)
            {
                selectedValue = "1";
            }
            UpdatePage(Convert.ToInt32(selectedValue));
        }


        public void UpdatePage(int selectedValue)
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_getIngredientsByProduct", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@product", selectedValue);
            reader = command.ExecuteReader();
        }

        public IActionResult OnPost(int id)
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_deleteIngredient", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            reader = command.ExecuteReader();
            reader.Close();
            return RedirectToPage(new {selectedValue = SelectedValue});
        }
    }
}
