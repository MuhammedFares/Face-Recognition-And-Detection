using System;
using System.Collections.Generic;
using System.Data;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DbSystem.Controller
{
    public class Inserter
    {
        string table;
        SqlConnection connection;
        public Inserter(SqlConnection Conn)
        {
            connection = Conn;
            table = "Players";
        }
        public async Task InsertFaceElement
       (string Name, string playerteam, int playernumber)
        {
            string query = "INSERT into " + table + " (id,playername,playerteam,playernumber) VALUES (@id,@playername,@playerteam,@playernumber)";


            using (SqlCommand insertQuery = new SqlCommand(query))
            {
                //parameters add : to avoid Sql injection
                insertQuery.Connection = connection;
                insertQuery.Parameters.Add("@id", SqlDbType.Int, 30).Value = GetRows() + 1;
                insertQuery.Parameters.AddWithValue("playername", Name);
                insertQuery.Parameters.AddWithValue("playerteam", playerteam);
                insertQuery.Parameters.AddWithValue("playernumber", playernumber);
                await insertQuery.ExecuteNonQueryAsync();
                Debug.WriteLine(Name + " excuted");
            }
        }
        private int GetRows()
        {
            try
            {
                SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM "+table+"", connection);
                Int32 count = (Int32)comm.ExecuteScalar();
                return count;
            }
            catch (Exception any)
            {
                Debug.WriteLine("######" + any.Message);
                return 0;
            }
        }
    }

}
