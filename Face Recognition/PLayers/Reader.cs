using DbSystem.Struct;
using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSystem.Controller
{
    public class Reader
    {
        SqlConnection connection;
        String tablename;
        public Reader(SqlConnection Conn)
        {
            connection = Conn;
            tablename = "Players";
        }

        public List<player> Players()
        {
            List<player> users = new List<player>();

            string query = " select * from " + tablename;

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
                        player u = new player();
                        u.Number = Convert.ToInt16(reader["playernumber"].ToString());
                        u.Name = reader["playername"].ToString();
                        u.Team = reader["playerteam"].ToString();
                        users.Add(u);
                    }

                }

            }
            return users;
        }
    }
}
