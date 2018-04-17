using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.SqlClient;

namespace Habraken_SLE
{
    public class Database
    {
        private string connectionString;
        private SqlConnection con;

        public Database()
        {
            connectionString = @"Data Source=(LocalDB)\MSSQLLocalDB;AttachDbFilename=C:\Habraken Label Editor Database\dbLabelEditor.mdf;Integrated Security=True;Connect Timeout=30";
            con = new SqlConnection(connectionString);

            
        }

        public string connection
        {
            get => default(string);
            set
            {
            }
        }

        public string TestConnection()
        {
            string result = "";
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
            throw new System.NotImplementedException();
        }

        public void CloseConnection()
        {
            throw new System.NotImplementedException();
        }

        public void Query()
        {
            throw new System.NotImplementedException();
        }
    }
}