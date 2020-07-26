using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DbSystem
{
    public class Core
    {
        public string ConnectionString { set; private get; }
        public SqlConnection Con;
        public bool Connected = false;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="Cstring">Database Connection String</param>
        public void Connect(string Cstring )
        {
            try
            {
                ConnectionString = Cstring;
                Con = new SqlConnection(ConnectionString);
                if (Con.State != System.Data.ConnectionState.Open)
                {
                    Con.Open();
                    Debug.WriteLine("### Core Class , Connected ###");
                    Connected = true;
                }
            }
            catch (SqlException EX)
            {
                Debug.WriteLine("### Core Class ,Error at  Line 31 ###");
                Debug.WriteLine(EX.Message);
                throw EX;
            }
        }
        /// <summary>
        /// Disconnecting The Whole Data Base Connection.
        /// </summary>
        public void DisConnect()
        {
            try
            {
                if (Con.State != System.Data.ConnectionState.Closed)
                {
                    Con.Close();

                }
            }
            catch (SqlException EX)
            {
                Debug.WriteLine("### Core Class ,Error at  Line 54 ###");
                Debug.WriteLine(EX.Message);
                throw EX;
            }
        }
        /// <summary>
        /// Restarts All Connections Between DataBase Server
        /// </summary>
        public void Restart()
        {
            try
            {
                Connect(ConnectionString);
            }
            catch (SqlException EX)
            {
                Debug.WriteLine("### Core Class ,Error at  Line 67 ###");
                Debug.WriteLine(EX.Message);
                throw EX;
            }
        }

        /// Declare Classes
    }
}
