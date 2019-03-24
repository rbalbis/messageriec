using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using SQLite;
using SQLiteConnection = System.Data.SQLite.SQLiteConnection;
using SQLiteCommand = System.Data.SQLite.SQLiteCommand;

namespace WpfApp1
{
    class Database
    {
        public SQLiteConnection conn;
        public Database()
        {
            string _dbPath = "MyDatabase.db3";


            // Instanciation de notre connexion
            SQLiteConnection sqlite_conn = new SQLiteConnection("Data Source=database.sqlite;Version=3;");
            // Utilisation de l'API en mode synchrone

            this.conn = sqlite_conn;

            sqlite_conn.Open();
           

            SQLiteCommand sqlite_cmd = sqlite_conn.CreateCommand();

            // Let the SQLiteCommand object know our SQL-Query:
            sqlite_cmd.CommandText = "CREATE TABLE IF NOT EXISTS pass (id INT PRIMARY KEY, login varchar(100), privk varchar(100), pubk varchar(100)) ;";

            // Now lets execute the SQL ;-)
            sqlite_cmd.ExecuteNonQuery();

            /* // Utilisation de l'API asynchrone
             SQLiteAsyncConnection connAsync = new SQLiteAsyncConnection("myDb.db3");
         connection.CreateTableAsync<Personne>();*/


        }

        public object OutputTextBox { get; private set; }

        public SQLiteConnection GetConn()
        {
            if (conn == null)
            {
                new Database();
            }
            return conn;
        }

        public void insertUser(string login, string privk, string pubk)
        {

            SQLiteCommand sqlite_cmd = this.conn.CreateCommand();

            sqlite_cmd.CommandText = "INSERT INTO pass (login, privk, pubk) VALUES ('"+login+"', '"+privk+"','"+pubk+"');";

            sqlite_cmd.ExecuteNonQuery();
        }

        public void Test(SQLiteConnection conn)
        {


            SQLiteCommand sqlite_cmd = this.conn.CreateCommand();

            sqlite_cmd.CommandText = "INSERT INTO pass (login, privk, pubk) VALUES ('Michel', 'TTTEEESSSTTT' , 'Testpub');";

            sqlite_cmd.ExecuteNonQuery();

            sqlite_cmd.CommandText = "SELECT * FROM pass";

            SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();

            // The SQLiteDataReader allows us to run through each row per loop
            while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
            {
                // Print out the content of the text field:
                System.Console.WriteLine("----------------------------");
                //System.Console.WriteLine("DEBUG Output: '" + sqlite_datareader + "'");

                string loginReader = sqlite_datareader.GetString(1);
                string keyReader = sqlite_datareader.GetString(2);
                System.Console.WriteLine(loginReader);


                // OutputTextBox.Text += idReader + " '" + textReader + "' " + "\n";
            }

        }

        public string userKey(string username)
        {

            System.Console.WriteLine("----------------------------");
            System.Console.WriteLine(username);


            SQLiteCommand sqlite_cmd = this.conn.CreateCommand();

            sqlite_cmd.CommandText = "INSERT INTO pass (login, privk, pubk) VALUES ('Michel', 'TTTEEESSSTTT', 'aaaa');";

            sqlite_cmd.ExecuteNonQuery();

            sqlite_cmd.CommandText = "SELECT * FROM pass where login = '" + username + "'";

            SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();

            // The SQLiteDataReader allows us to run through each row per loop
            while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
            {
                // Print out the content of the text field:
                System.Console.WriteLine("----------------------------");
                //System.Console.WriteLine("DEBUG Output: '" + sqlite_datareader + "'");

                
                string keyReader = sqlite_datareader.GetString(2);
                System.Console.WriteLine(keyReader);
                return keyReader;

                // OutputTextBox.Text += idReader + " '" + textReader + "' " + "\n";
            }

            return "";
        }

        public string userPubKey(string username)
        {

            System.Console.WriteLine("----------------------------");
            System.Console.WriteLine(username);


            SQLiteCommand sqlite_cmd = this.conn.CreateCommand();

            sqlite_cmd.CommandText = "INSERT INTO pass (login, privk, pubk) VALUES ('Michel', 'TTTEEESSSTTT', 'aaaa');";

            sqlite_cmd.ExecuteNonQuery();

            sqlite_cmd.CommandText = "SELECT * FROM pass where login = '" + username + "'";

            SQLiteDataReader sqlite_datareader = sqlite_cmd.ExecuteReader();

            // The SQLiteDataReader allows us to run through each row per loop
            while (sqlite_datareader.Read()) // Read() returns true if there is still a result line to read
            {
                // Print out the content of the text field:
                System.Console.WriteLine("----------------------------");
                //System.Console.WriteLine("DEBUG Output: '" + sqlite_datareader + "'");


                string keyReader = sqlite_datareader.GetString(3);
                System.Console.WriteLine(keyReader);
                return keyReader;

                // OutputTextBox.Text += idReader + " '" + textReader + "' " + "\n";
            }

            return "";
        }
    }
}
