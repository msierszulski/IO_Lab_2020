using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ClassLibrary
{
    public class Database :IDisposable
    {
        SQLiteConnection sqlite_conn;

        public Database()
        {
            sqlite_conn = new SQLiteConnection("Data Source=:memory:");
            // Open the connection:
            try
            {
                sqlite_conn.Open();
                CreateTable();
            }
            catch (Exception ex)
            {

            }
        }

        public void CreateTable()
        {

            SQLiteCommand sqlite_cmd;
            string Createsql = "CREATE TABLE Users(" +
                "name TEXT NOT NULL" +
                ")";
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = Createsql;
            sqlite_cmd.ExecuteNonQuery();
        }

        public void InsertData(string name)
        {
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText = "INSERT INTO Users(name) VALUES(@name);";
            sqlite_cmd.Parameters.AddWithValue("@name", name);
            sqlite_cmd.Prepare();

            sqlite_cmd.ExecuteNonQuery();
        }

        public string ReadData()
        {
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            sqlite_cmd.CommandText = "SELECT * FROM Users";
            string msg = "";
            sqlite_datareader = sqlite_cmd.ExecuteReader();
            while (sqlite_datareader.Read())
            {
                string myreader = sqlite_datareader.GetString(0);
                msg += myreader + "\n";
                Console.WriteLine(myreader);
            }

         return msg;
        }

        public void Dispose()
        {
            sqlite_conn.Close();
            sqlite_conn.Dispose();
        }

    }
}