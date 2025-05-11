using MySql.Data.MySqlClient;

namespace Project
{
    public class DatabaseConnection
    {
        private string connStr = "server=localhost;user id=root;password=;database=bawangmerahdb";

        public MySqlConnection GetConnection()
        {
            return new MySqlConnection(connStr);
        }
    }
}
