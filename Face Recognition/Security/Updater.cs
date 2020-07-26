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
    public class Updater
    {
        public SqlConnection connection;
        public string wt;

        public Updater(SqlConnection c)
        {
            connection = c;
            wt = "UsersData";

        }
        public async void ChangePassword(string UserName, string oldpassword, string newpassword)
        {
            string query = "UPDATE "+wt+" SET PassHash =@newpassword WHERE [userName] =@userName";

            oldpassword = StringCipher.Encrypt(oldpassword, UserName);
            newpassword = StringCipher.Encrypt(newpassword, UserName);

            string old = "";
            // reading old password , to check if it is correct
            using (SqlCommand command = new SqlCommand("Select PassHash From "+wt+" Where userName ='"+UserName+"'" , connection))
            {
                if (connection.State != ConnectionState.Open)
                {
                    connection.Open();

                }
                using (SqlDataReader reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        old = (reader["PassHash"].ToString());

                    }
                }
            }

            if (old != oldpassword)
            {
                Debug.WriteLine("inCorrect Password , You can't change it ");
                return;
            }
            else
            {
                using (SqlCommand insertQuery = new SqlCommand(query))
                {
                    //parameters add : to avoid Sql injection
                    insertQuery.Connection = connection;
                    insertQuery.Parameters.AddWithValue("@userName", UserName);
                    insertQuery.Parameters.AddWithValue("@oldpassword", oldpassword);
                    insertQuery.Parameters.AddWithValue("@newpassword", newpassword);

                    await insertQuery.ExecuteNonQueryAsync();
                    Debug.WriteLine("User With userName = " + UserName + "password changed");
                }
            }
        }
        public async void ChangeUserType(string UserName, int type)
        {
            string query = "UPDATE "+wt+ " SET IsAdmin =@IsAdmin WHERE userName =@userName";

            using (SqlCommand insertQuery = new SqlCommand(query))
            {
                //parameters add : to avoid Sql injection
                insertQuery.Connection = connection;
                insertQuery.Parameters.AddWithValue("@userName", UserName);
                insertQuery.Parameters.AddWithValue("@IsAdmin", type);

                await insertQuery.ExecuteNonQueryAsync();
                Debug.WriteLine("User With Name = " + UserName + "Type changed");

            }
        }
        public void DeleteUserByID(string id)
        {
            using (SqlCommand command
             = new SqlCommand("DELETE FROM "+wt+" where id=" + id, connection))
            {
                command.ExecuteNonQuery();
            }

        }
        public void DeleteUser(string User)
        {
            using (SqlCommand command
             = new SqlCommand("DELETE FROM "+wt+ " where userName=" + User, connection))
            {
                command.ExecuteNonQuery();
            }

        }
    }
}
