using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using System.Data;
using WebBurgers.DataBase;


namespace WebBurgers.Pages.Products
{
    public class ProductModel : PageModel
    {
        public SqlDataReader reader;
        public void OnGet()
        {
            SqlConnection connection = DB.instance.getConnection();
            SqlCommand command = new SqlCommand("ShowProduct", connection);
            command.CommandType = CommandType.StoredProcedure;
            reader = command.ExecuteReader();
            //while (reader.Read())
            //{
            //    // Получите данные из каждой строки результата выполнения хранимой процедуры
            //    int productID = reader.GetInt32(0);
            //    string productName = reader.GetString(1);
            //    string unitName = reader.GetString(2);
            //    float productCount = reader.GetFloat(0);
            //    // и т.д.
            //}
            //reader.Close();
        }
        public string PrintTime() => DateTime.Now.ToString();

    }
}
