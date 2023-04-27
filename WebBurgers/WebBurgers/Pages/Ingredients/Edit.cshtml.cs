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
    public class EditModel : PageModel
    {
        private readonly WebBurgers.Repository.BurgerContext db;
        public int product = 0;
        public int material = 0;
        public decimal count = 0;
        public int ID = 0;
        public SqlDataReader reader;
        public SelectList Materials { get; set; }
        public SelectList Products { get; set; }
        public EditModel(WebBurgers.Repository.BurgerContext context)
        {
            db = context;
        }

        public void OnGet(int id)
        {
            ID = id;
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_getIngredientsById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                // Получение данных из каждой строки результата выполнения хранимой процедуры
                product = material = reader.GetInt32(1);
                material = reader.GetInt32(2);
                count = reader.GetDecimal(3);
               
                // и т.д.
            }
            reader.Close();

            Materials = new SelectList(db.Materials.ToList(), "Id", "Name", material);
            Products = new SelectList(db.Products.ToList(), "Id", "Name", product);

        }
        public IActionResult OnPost(int ID, int product, int material, decimal count)
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_updateIngredients", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", ID);
            command.Parameters.AddWithValue("@product", product);
            command.Parameters.AddWithValue("@material", material);
            command.Parameters.AddWithValue("@count", count);

            reader = command.ExecuteReader();
            reader.Close();

            return RedirectToPage($"./Index", new { selectedValue = product });
        }
    }
}
