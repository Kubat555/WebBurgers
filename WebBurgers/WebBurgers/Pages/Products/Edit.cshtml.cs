using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Data.SqlClient;
using System.Data;
using WebBurgers.DataBase;

namespace WebBurgers.Pages.Products
{
    public class EditModel : PageModel
    {
        public string productName = string.Empty;
        public int unit = 0;
        public decimal count = 0;
        public decimal summa = 0;
        public int ID = 0;
        public SqlDataReader reader;
        public SelectList Units { get; set; }
        public void OnGet(int id)
        {
            ID = id;
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_getProductById", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", id);
            reader = command.ExecuteReader();
            while (reader.Read())
            {
                // Получите данные из каждой строки результата выполнения хранимой процедуры
                productName = reader.GetString(0);
                unit = reader.GetInt32(1);
                count = reader.GetDecimal(2);
                summa = reader.GetDecimal(3);
                // и т.д.
            }
            reader.Close();

            //command = new SqlCommand("sp_getUnits", connection);
            //command.CommandType = CommandType.StoredProcedure;
            //reader = command.ExecuteReader();
            //List<SelectListItem> items = new List<SelectListItem>();
            //while (reader.Read())
            //{
            //    items.Add(new SelectListItem { Selected = , Text =  reader.GetString(1)});
            //}
            //Units = new SelectList(items, "Id", "Name", unit);
        }

        public IActionResult OnPost(int ID, string ProductName, int Unit, decimal Count, decimal Summa )
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("sp_updateProduct", connection);
            command.CommandType = CommandType.StoredProcedure;
            command.Parameters.AddWithValue("@id", ID);
            command.Parameters.AddWithValue("@Name", ProductName);
            command.Parameters.AddWithValue("@Unit", Unit);
            command.Parameters.AddWithValue("@Count", Count);
            command.Parameters.AddWithValue("@Summa", Summa);

            reader = command.ExecuteReader();
            reader.Close();
            return RedirectToPage("/Products/Product");
            
        }
    }
}
