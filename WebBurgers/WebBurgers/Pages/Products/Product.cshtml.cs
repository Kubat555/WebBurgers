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
      
        }

    }
}
