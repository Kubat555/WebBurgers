using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Data.SqlClient;
using WebBurgers.DataBase;


namespace WebBurgers.Pages
{
    public class ProductModel : PageModel
    {
        public void OnGet()
        {
            SqlConnection connection = DB.instance.getConnection();

        }
        public string PrintTime() => DateTime.Now.ToString();

    }
}
