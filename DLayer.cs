using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Text;
namespace DataLayer
{
    public class DLayer
    {
        
        public List<MenuItem> GetMenuItems()
        {
            List<MenuItem> menuItems = new List<MenuItem>();
            using (SqlConnection sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RestaurantDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = "SELECT * FROM [MenuItems]";
                SqlDataReader sdr = sqlCommand.ExecuteReader();
                while (sdr.Read())
                {
                    MenuItem mi = new MenuItem();
                    mi.Id = sdr.GetInt32(0);
                    mi.Title = sdr.GetString(1);
                    mi.Description = sdr.GetString(2);
                    mi.Price =  sdr.GetDecimal(3);
                    menuItems.Add(mi);
                }
                sqlConnection.Close();
                return menuItems;
            }


        }

        public int InsertMenuItems(MenuItem mit)
        {

            using (SqlConnection sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RestaurantDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = string.Format(
                    "INSERT INTO [MenuItems] VALUES ('{0}','{1}','{2}')",
                     mit.Title,mit.Description,mit.Price);
               
                //sqlConnection.Close();
                return sqlCommand.ExecuteNonQuery();
            }
        }

        public int UpdateMenuItems(MenuItem mit)
        {

            using (SqlConnection sqlConnection = new SqlConnection())
            {
                sqlConnection.ConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=RestaurantDB;Integrated Security=True;Connect Timeout=30;Encrypt=False;TrustServerCertificate=False;ApplicationIntent=ReadWrite;MultiSubnetFailover=False";
                sqlConnection.Open();
                SqlCommand sqlCommand = new SqlCommand();
                sqlCommand.Connection = sqlConnection;
                sqlCommand.CommandText = string.Format(
                    "UPDATE [MenuItems] SET Title='{0}',Description='{1}',Price='{2}'" + "WHERE Id={3}",
                     mit.Title, mit.Description, mit.Price,mit.Id);

                //sqlConnection.Close();
                return (int) sqlCommand.ExecuteNonQuery();
            }
        }

    }
}
