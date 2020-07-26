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
    public class Writer
    {
        public SqlConnection con;
        public string wt;

        public Writer(SqlConnection c)
        {
            con = c;
            wt = "UsersData";
        }
        public async Task InsertUser (string userName, string RecoveryMail, int IsAdmin, string PassHash)
        {
            string query = "INSERT into " + wt + " (id,userName,PassHash,RecoveryMail,IsAdmin) VALUES (@id,@userName,@PassHash,@RecoveryMail,@IsAdmin)";
            PassHash = StringCipher.Encrypt(PassHash, userName);
            using (SqlCommand insertQuery = new SqlCommand(query))
            {
                //parameters add : to avoid Sql injection
                insertQuery.Connection = con;
                insertQuery.Parameters.Add("@id", SqlDbType.Int, 30).Value = GetRows() + 1;
                insertQuery.Parameters.AddWithValue("userName", userName);
                insertQuery.Parameters.AddWithValue("PassHash", PassHash);
                insertQuery.Parameters.AddWithValue("RecoveryMail", RecoveryMail);
                insertQuery.Parameters.AddWithValue("IsAdmin", IsAdmin);
              await  insertQuery.ExecuteNonQueryAsync();//
                Debug.WriteLine(userName + " excuted");
            }
           
        }
        private int GetRows()
        {
            try
            {
                SqlCommand comm = new SqlCommand("SELECT COUNT(*) FROM " + wt + "", con);
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
