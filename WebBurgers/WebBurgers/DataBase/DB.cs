using Microsoft.Data.SqlClient;


namespace WebBurgers.DataBase
{
    public class DB
    {
        public static DB instance = new DB();
        SqlConnection sqlConnection = new SqlConnection(@"Data Source=KUBAT;Initial Catalog=Burgers;User ID=sa;Password=Kubat555;Integrated Security=True;TrustServerCertificate=True;MultiSubnetFailover=True");

        public DB()
        {
            if(instance == null)
            {
                instance = this;
            }
        }
        public void OpenConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Closed)
            {
                sqlConnection.Open();
            }
        }

        public void CloseConnection()
        {
            if (sqlConnection.State == System.Data.ConnectionState.Open)
            {
                sqlConnection.Close();
            }
        }

        public SqlConnection getConnection()
        {
            return sqlConnection;
        }
    }
}
