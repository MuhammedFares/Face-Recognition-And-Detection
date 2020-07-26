using LoginSystem.Data;
using LoginSystem.Users;
using SMARTY.Security;
using System;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Threading.Tasks;

namespace LoginSystem
{
    public class Sys
    {
        public Reader reader;
        public Writer writer;
        public Updater updater;
        public SqlConnection Con;
        
        
      
        public string ConnectionString { set; private get; }
        public Sys(string Cstring)
        {
            try
            {
                ConnectionString = Cstring;
                Con = new SqlConnection(ConnectionString);
                if (Con.State != System.Data.ConnectionState.Open)
                {
                    Con.Open();
                    Debug.WriteLine("### sYs Class , Connected ###");

                    //Define Classes

                    reader = new Reader(Con);
                    writer = new Writer(Con);
                    updater = new Updater(Con);
                }
            }
            catch (SqlException EX)
            {
                Debug.WriteLine("### sYs Class ,Error at  Line 31 ###");
                Debug.WriteLine(EX.Message);
                throw EX;
            }
        }
        
        public SignInResult SignIn(string user , string password)
        {
            var users = reader.Users();
            foreach (User item in users)
            {
                
                if (item.Name == user)
                {
                    if (password == item.password)
                    {

                        return new SignInResult() { LoggedIn = true, Reason = "no erros", UserData =item };
                    }
                    else
                    {
                        return new SignInResult() { LoggedIn = false, Reason = "Invalid Password" };

                    }
                }
                else
                {
                        continue;
                  //  return new SignInResult() { LoggedIn = false, Reason = "Incorrect UserName" };

                }
            }
            return new SignInResult() { LoggedIn = false, Reason = "Can't Find User Name" };
        }
        public async Task<bool> SignUp(string user, string pw, string email, int Isadmin = 0)
        {
            try
            {
                await writer.InsertUser(user, email, Isadmin, pw);
                return true;
            }catch(Exception e)
            {
                Debug.WriteLine(e.Message);
                return false;
            }
        }
    }
}
