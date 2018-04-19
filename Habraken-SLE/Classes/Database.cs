using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data;
using System.Data.SqlClient;

namespace Habraken_SLE
{
    public class Database
    {
        private string connectionString;
        private string result = "";
        private SqlConnection con;

        public Database()
        {
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Habraken Label Editor Database\dbLabelEditor.mdf;Integrated Security=True;Connect Timeout=30";
            con = new SqlConnection(connectionString);

            
        }

        public string connection
        {
            get; set;

        }

        public string TestConnection()
        {
            try
            {
                OpenConnection();
                result = "Connection OK!";
            }
            catch 
            {
                result = "Connection ERROR";
            }           
            return result;        

        }

        public void OpenConnection()
        {
            con.Open();
        }

        public void CloseConnection()
        {
            con.Close();
        }

        public string Query(string type, string input)
        {
            switch (type)
            {
                case "Login":
                    result = Login(input);
                    break;
                default:
                    break;
            }

            return result;
        }

        private string Login(string input)
        {
            try
            {
                var db = new HLE_LinqtoSQLDataContext();
                tbl_User t = null;

                t = db.tbl_Users.Single(p => p.UserID == Convert.ToInt32(input));

                if (t != null)
                {
                    result = "OK";
                }
            }
            catch
            {
                result = "Fail";
            }

            return result;
        }
    }
}