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

        public void Query()
        {

        }

        private string Login(string input)
        {
            return "";
        }
    }
}