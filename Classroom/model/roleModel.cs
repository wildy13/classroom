using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classroom.model
{
    internal class roleModel
    {
        private static string connectionString = $"Data Source=classroom.db;Version=3;";

        public static void CreateRoleTable()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS role (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        name TEXT UNIQUE NOT NULL
                    )";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }
                CreateDefaultUsers();
            }
        }

        private static void CreateDefaultUsers()
        {
            SaveRole("Teacher");
            SaveRole("Student");
        }

        public static bool IsRoleExists(string name)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT COUNT(*) FROM role WHERE name = @Name";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", name);
                    long roleCount = (long)command.ExecuteScalar();
                    return roleCount > 0;
                }
            }
        }

        public static void SaveRole(string name)
        {
            if (IsRoleExists(name))
            {
                Console.WriteLine("Error: Role already exists.");
                return;
            }

            try
            {
                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO role (name) VALUES (@Name)";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Name", name);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException e)
            {
                Console.WriteLine($"Error saving role: {e.Message}");
            }
        }

        public static int GetRoleId(string roleName)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT id FROM role WHERE name = @Name";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Name", roleName);
                    object result = command.ExecuteScalar();
                    return result != null ? Convert.ToInt32(result) : -1;
                }
            }
        }

        public static List<string> GetRoles()
        {
            List<string> roles = new List<string>();
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT name FROM role";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    using (SQLiteDataReader reader = command.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            roles.Add(reader["name"].ToString());
                        }
                    }
                }
            }
            return roles;
        }
    }
}
