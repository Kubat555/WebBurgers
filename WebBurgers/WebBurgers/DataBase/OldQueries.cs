using Microsoft.Data.SqlClient;
using System.Data;

namespace WebBurgers.DataBase
{
    public class OldQueries
    {

            public static OldQueries instance = new OldQueries();
            DB db = new DB();

            public OldQueries()
            {
                if (instance == null)
                {
                    instance = this;
                }
            }

            public bool CheckBudjet(double sum)
            {
                db.OpenConnection();
                string output = "", query = $"Select Summa\r\n\tfrom Budget\r\n\twhere ID = 1";
                SqlCommand command = new SqlCommand(query, db.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    output = output + reader.GetValue(0);
                }
                reader.Close();
                db.CloseConnection();

                if (sum > Convert.ToDouble(output))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public void PurchaseMaterial(int mId, double mCount, double mSumma, string date, int empId)
            {
                db.OpenConnection();
                SqlConnection conn = db.getConnection();
                string query = $"insert into PurchaseMaterial(Material, [Count], Summa, [Date], Employee)\r\n\tvalues ({mId}, {mCount},{mSumma}, @date, {empId})";
                SqlCommand cmd;
                SqlDataAdapter adap = new SqlDataAdapter();

                cmd = new SqlCommand(query, conn);

                // associate the insert SQL
                // command to adapter object
                adap.InsertCommand = new SqlCommand(query, conn);
                adap.InsertCommand.Parameters.Add("@date", SqlDbType.SmallDateTime);
                adap.InsertCommand.Parameters["@date"].Value = date;

                // use to execute the DML statement against
                // our database
                adap.InsertCommand.ExecuteNonQuery();

                // closing all the objects
                cmd.Dispose();

                query = $"Update Budget\r\n\tset Summa = Summa - {mSumma}\r\n\r\n\tUpdate Materials\r\n\tset [Count] = [Count] + {mCount}, Summa = Summa + {mSumma}\r\n\twhere Materials.ID = {mId}";
                cmd = new SqlCommand(query, conn);
                adap.InsertCommand = new SqlCommand(query, conn);
                adap.InsertCommand.ExecuteNonQuery();


                cmd.Dispose();
                db.CloseConnection();
            }

            public bool CheckMaterials(int pId, double count)
            {
                db.OpenConnection();
                string output = "",
                    query = $"SELECT dbo.Ingredients.Material\r\n\tFROM dbo.Ingredients \r\n\tINNER JOIN \r\n\tdbo.Materials ON dbo.Ingredients.Material = dbo.Materials.ID \r\n\twhere dbo.Ingredients.Product = {pId}\r\n\tAND dbo.Ingredients.[Count] * {count} > dbo.Materials.[Count]";
                SqlCommand command = new SqlCommand(query, db.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    output = output + reader.GetValue(0);
                }
                reader.Close();
                db.CloseConnection();

                if (output != "")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }


            public void ProductionOfProduct(int pId, double pCount, string date, int empId)
            {
                db.OpenConnection();
                SqlConnection conn = db.getConnection();
                string query = $"insert into ProductionOfProducts(Product, [Count], [Date], Employee)\r\n\tvalues ({pId}, {pCount}, @date, {empId})";
                SqlCommand cmd;
                SqlDataAdapter adap = new SqlDataAdapter();

                cmd = new SqlCommand(query, conn);

                // associate the insert SQL
                // command to adapter object
                adap.InsertCommand = new SqlCommand(query, conn);
                adap.InsertCommand.Parameters.Add("@date", SqlDbType.SmallDateTime);
                adap.InsertCommand.Parameters["@date"].Value = date;

                // use to execute the DML statement against
                // our database
                adap.InsertCommand.ExecuteNonQuery();

                // closing all the objects
                cmd.Dispose();


                double pCost = 0;
                query = $"SELECT SUM(Materials.Summa / Materials.[Count] * Ingredients.[Count])\r\n\tFROM Ingredients INNER JOIN\r\nMaterials ON Ingredients.Material = Materials.ID INNER JOIN\r\nProducts ON Ingredients.Product = {pId}\r\n\twhere Products.ID = {pId}";
                cmd = new SqlCommand(query, conn);
                string output = "";
                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    output = output + reader.GetValue(0);
                }
                reader.Close();

                //pCost = Convert.ToDouble(output);

                query = $"update dbo.Products\r\n\tset Products.[Count] = Products.[Count] + {pCount}, Products.Summa = Products.Summa + {pCount} * @pCost\r\n\twhere Products.ID = {pId}";
                cmd = new SqlCommand(query, conn);
                adap.InsertCommand = new SqlCommand(query, conn);
                adap.InsertCommand.Parameters.Add("@pCost", SqlDbType.Float);
                adap.InsertCommand.Parameters["@pCost"].Value = output;
                adap.InsertCommand.ExecuteNonQuery();


                cmd.Dispose();


                /// Попробуй  через созданное представление 
                query = $"update IngMat\r\nset Summa = Summa - Summa / MaterialCount * ({pCount} * IngredientCount), MaterialCount = MaterialCount - IngredientCount*{pCount}\r\nwhere Product = {pId}";
                cmd = new SqlCommand(query, conn);
                adap.InsertCommand = new SqlCommand(query, conn);
                adap.InsertCommand.ExecuteNonQuery();

                cmd.Dispose();
                db.CloseConnection();
            }

            public double CheckProducts(int pId, double pCount)
            {
                db.OpenConnection();
                double summa = 0;
                string output = "", output2 = "",
                    query = $"select [Count], Summa\r\n\tfrom Products\r\n\twhere ID = {pId}";
                SqlCommand command = new SqlCommand(query, db.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    output = output + reader.GetValue(0);
                    output2 = output2 + reader.GetValue(1);
                }
                reader.Close();


                double count = Convert.ToDouble(output);
                summa = Convert.ToDouble(output2);

                if (pCount > count)
                {
                    return -1;
                }
                else
                {
                    output = "";
                    summa = summa / count * pCount;
                    query = $"select AddPercent\r\n\t\tfrom Budget\r\n\t\twhere ID = 1";

                    command = new SqlCommand(query, db.getConnection());

                    reader = command.ExecuteReader();

                    while (reader.Read())
                    {
                        output = output + reader.GetValue(0);
                    }
                    reader.Close();

                    summa = summa + (summa / 100 * Convert.ToDouble(output));
                    return summa;
                }
            }

            public void SaleOfProducts(int pId, double pCount, double pSumma, string date, int empId)
            {
                db.OpenConnection();
                SqlConnection conn = db.getConnection();
                string query = $"insert into SaleOfProducts(Product, [Count], Summa, [Date], Employee)\r\n\tvalues ({pId}, {pCount},{pSumma}, @date, {empId})";
                SqlCommand cmd;
                SqlDataAdapter adap = new SqlDataAdapter();

                cmd = new SqlCommand(query, conn);

                // associate the insert SQL
                // command to adapter object
                adap.InsertCommand = new SqlCommand(query, conn);
                adap.InsertCommand.Parameters.Add("@date", SqlDbType.SmallDateTime);
                adap.InsertCommand.Parameters["@date"].Value = date;

                // use to execute the DML statement against
                // our database
                adap.InsertCommand.ExecuteNonQuery();

                // closing all the objects
                cmd.Dispose();


                string output = "";
                query = $"select Summa / [Count]\r\n\tfrom Products\r\n\twhere ID = {pId}";
                cmd = new SqlCommand(query, db.getConnection());

                SqlDataReader reader = cmd.ExecuteReader();

                while (reader.Read())
                {
                    output = output + reader.GetValue(0);
                }
                reader.Close();


                double sred = Convert.ToDouble(output);



                query = $"update Products\r\n\tset [Count] = [Count] - {pCount}, Summa = Summa - ({sred} * {pCount})\r\n\twhere ID = {pId}\r\n\t\r\n\tupdate Budget\r\n\tset Summa = Summa + {pSumma}\r\n\twhere ID = 1";
                cmd = new SqlCommand(query, conn);
                adap.InsertCommand = new SqlCommand(query, conn);
                adap.InsertCommand.ExecuteNonQuery();


                cmd.Dispose();
                db.CloseConnection();
            }

            public double GetEmpPercent()
            {
                db.OpenConnection();
                string output = "",
                    query = $"select [EmployeesPercent]\r\nfrom Budget\r\nwhere id = 1";
                SqlCommand command = new SqlCommand(query, db.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    output = output + reader.GetValue(0);
                }
                reader.Close();

                db.CloseConnection();

                return Convert.ToDouble(output);
            }

            public bool CheckSalary(int month, int year, int emp)
            {
                db.OpenConnection();
                string output = "",
                    query = $"select *\r\nfrom Salary\r\nwhere SalaryMonth = {month} and SalaryYear = {year} and EmployeesId = {emp}";
                SqlCommand command = new SqlCommand(query, db.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    output = output + reader.GetValue(0);
                }
                reader.Close();

                db.CloseConnection();

                if (output != "")
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }

            public void CheckSalary(int month, int year, List<int> ids)
            {
                db.OpenConnection();
                string output = "",
                    query = $"select ID\r\nfrom Employees\r\nwhere not exists(select EmployeesId\r\n from Salary\r\n where SalaryMonth = {month} and SalaryYear = {year} and EmployeesId = Employees.ID)";
                SqlCommand command = new SqlCommand(query, db.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    ids.Add(Convert.ToInt32(reader.GetValue(0)));
                }
                reader.Close();


                db.CloseConnection();
            }

            public int GetCountOfPurchase(int month, int year, int emp)
            {
                db.OpenConnection();

                string output = "",
                    query = $"select count(ID)\r\nfrom PurchaseMaterial\r\nwhere Month([Date]) = {month} and Year([Date]) = {year} and Employee = {emp}";
                SqlCommand command = new SqlCommand(query, db.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    output = output + reader.GetValue(0);
                }
                reader.Close();

                db.CloseConnection();

                return Convert.ToInt32(output);

            }
            public int GetCountOfProduction(int month, int year, int emp)
            {
                db.OpenConnection();

                string output = "",
                    query = $"select count(ID)\r\nfrom ProductionOfProducts\r\nwhere Month([Date]) = {month} and Year([Date]) = {year} and Employee = {emp}";
                SqlCommand command = new SqlCommand(query, db.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    output = output + reader.GetValue(0);
                }
                reader.Close();

                db.CloseConnection();

                return Convert.ToInt32(output);

            }

            public int GetCountOfSale(int month, int year, int emp)
            {
                db.OpenConnection();

                string output = "",
                    query = $"select count(ID)\r\nfrom SaleOfProducts\r\nwhere Month([Date]) = {month} and Year([Date]) = {year} and Employee = {emp}";
                SqlCommand command = new SqlCommand(query, db.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    output = output + reader.GetValue(0);
                }
                reader.Close();

                db.CloseConnection();

                return Convert.ToInt32(output);

            }

            public double GetSalaryOfEmployee(int id)
            {
                db.OpenConnection();
                string output = "",
                    query = $"select Salary\r\nfrom Employees\r\nwhere ID = {id}";
                SqlCommand command = new SqlCommand(query, db.getConnection());

                SqlDataReader reader = command.ExecuteReader();

                while (reader.Read())
                {
                    output = output + reader.GetValue(0);
                }
                reader.Close();
                db.CloseConnection();

                return Convert.ToDouble(output);
            }

            public void IssueSalary(int emp, int month, int year, int purchase, int production, int sale, int total, double salary, double bonus)
            {
                db.OpenConnection();
                SqlConnection conn = db.getConnection();
                string query = $"insert into Salary(EmployeesId, SalaryMonth, SalaryYear, Purchase, Production, Sale, TotalCount, Salary, Bonus, TotalSalary, Issued)\r\n\tvalues ({emp}, {month}, {year},{purchase}, {production}, {sale}, {total}, @sal, @bonus, @sal + @bonus, 0)";
                SqlCommand cmd;
                SqlDataAdapter adap = new SqlDataAdapter();

                cmd = new SqlCommand(query, conn);

                // associate the insert SQL
                // command to adapter object
                adap.InsertCommand = new SqlCommand(query, conn);
                adap.InsertCommand.Parameters.Add("@sal", SqlDbType.Float);
                adap.InsertCommand.Parameters["@sal"].Value = salary;
                adap.InsertCommand.Parameters.Add("@bonus", SqlDbType.Float);
                adap.InsertCommand.Parameters["@bonus"].Value = bonus;

                // use to execute the DML statement against
                // our database
                adap.InsertCommand.ExecuteNonQuery();

                // closing all the objects
                cmd.Dispose();

                //query = $"update Budget\r\n\tset Summa = Summa - @sal - @bonus\r\n\twhere ID = 1";
                //cmd = new SqlCommand(query, conn);
                //adap.InsertCommand = new SqlCommand(query, conn);

                //adap.InsertCommand.Parameters.Add("@sal", SqlDbType.Float);
                //adap.InsertCommand.Parameters["@sal"].Value = salary;
                //adap.InsertCommand.Parameters.Add("@bonus", SqlDbType.Float);
                //adap.InsertCommand.Parameters["@bonus"].Value = bonus;
                //adap.InsertCommand.ExecuteNonQuery();


                //cmd.Dispose();
                db.CloseConnection();
            }

            public void DecreaseBudget(double summa)
            {
                db.OpenConnection();
                SqlConnection conn = db.getConnection();
                string query = "";
                SqlCommand cmd;
                SqlDataAdapter adap = new SqlDataAdapter();

                query = $"update Budget\r\n\tset Summa = Summa - @sal\r\n\twhere ID = 1";
                cmd = new SqlCommand(query, conn);
                adap.InsertCommand = new SqlCommand(query, conn);

                adap.InsertCommand.Parameters.Add("@sal", SqlDbType.Float);
                adap.InsertCommand.Parameters["@sal"].Value = summa;
                adap.InsertCommand.ExecuteNonQuery();


                cmd.Dispose();
                db.CloseConnection();
            }
      
    

}
}
