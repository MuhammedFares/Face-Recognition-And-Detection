using LoginSystem.Users;
using SMARTY.Security;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LoginSystem.Data
{
    public class Reader
    {
        public SqlConnection connection;
        public string wt;

        public Reader(SqlConnection c)
        {
            connection = c;
            wt = "UsersData";

        }
      
        public List<User> Users()
        {
            List<User> users = new List<User>();

            string query = " select * from " + wt;

            using (SqlCommand command = new SqlCommand(query, connection))
            {
                if (connection.State != ConnectionState.Open)
                {
                    try
                    {
                        connection.Open();
                    }
                    catch (SqlException E) { Debug.WriteLine(E.Message); }
                }
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        User u = new User();
                        u.id = Convert.ToInt16(reader["id"].ToString());
                        u.Name = reader["userName"].ToString();
                        u.password = reader["PassHash"].ToString();
                        u.password = StringCipher.Decrypt(u.password, u.Name);
                        int a = Convert.ToInt16(reader["IsAdmin"].ToString());
                        Func<int, bool> Isadmin = x => x == 1;
                        u.IsAdmin = Isadmin(a);
                        users.Add(u);
                    }

                }

            }
            return users;
        }

    }
}
