using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classroom.model
{
    internal class usersModel
    {
        private static string connectionString = $"Data Source=classroom.db;Version=3;";

        public static void CreateUsersTable()
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string createTableQuery = @"
                    CREATE TABLE IF NOT EXISTS users (
                        id INTEGER PRIMARY KEY AUTOINCREMENT,
                        username TEXT NOT NULL,
                        email TEXT UNIQUE NOT NULL,
                        password TEXT NOT NULL,
                        role_id INTEGER,
                        FOREIGN KEY (role_id) REFERENCES role (id)
                    )";
                using (var command = new SQLiteCommand(createTableQuery, connection))
                {
                    command.ExecuteNonQuery();
                }

                // Buat pengguna default setelah tabel dibuat
                CreateDefaultUsers();
            }
        }

        private static void CreateDefaultUsers()
        {
            string defaultTeacherUsername = "teacher";
            string defaultTeacherEmail = "teacher@example.com";
            string defaultTeacherPassword = "teacher"; // Anda mungkin ingin menggunakan kata sandi yang lebih aman

            int teacherRoleId = roleModel.GetRoleId("Teacher");

            // Pastikan email tidak ada sebelum membuat pengguna baru
            if (!IsEmailExists(defaultTeacherEmail))
            {
                SaveUser(defaultTeacherUsername, defaultTeacherEmail, defaultTeacherPassword, teacherRoleId);
                Console.WriteLine("Default teacher created.");
            }
        }

        public static bool IsEmailExists(string email)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT COUNT(*) FROM users WHERE email = @Email";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Email", email);
                    long userCount = (long)command.ExecuteScalar();
                    return userCount > 0;
                }
            }
        }

        public static void SaveUser(string username, string email, string password, int roleId)
        {
            if (IsEmailExists(email))
            {
                Console.WriteLine("Error: Email already exists.");
                return;
            }

            try
            {
                string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);

                using (var connection = new SQLiteConnection(connectionString))
                {
                    connection.Open();
                    string insertQuery = "INSERT INTO users (username, email, password, role_id) VALUES (@Username, @Email, @Password, @RoleId)";
                    using (var command = new SQLiteCommand(insertQuery, connection))
                    {
                        command.Parameters.AddWithValue("@Username", username);
                        command.Parameters.AddWithValue("@Email", email);
                        command.Parameters.AddWithValue("@Password", hashedPassword);
                        command.Parameters.AddWithValue("@RoleId", roleId);
                        command.ExecuteNonQuery();
                    }
                }
            }
            catch (SQLiteException e)
            {
                Console.WriteLine($"Error saving user: {e.Message}");
            }
        }

        public static bool LoginUser(string username, string password)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = "SELECT password FROM users WHERE username = @Username";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    object result = command.ExecuteScalar();
                    if (result != null)
                    {
                        string hashedPassword = result.ToString();
                        return BCrypt.Net.BCrypt.Verify(password, hashedPassword);
                    }
                    return false;
                }
            }
        }

        public static string GetUserRole(string username)
        {
            using (var connection = new SQLiteConnection(connectionString))
            {
                connection.Open();
                string selectQuery = @"SELECT role.name FROM users
                                       INNER JOIN role ON users.role_id = role.id
                                       WHERE username = @Username";
                using (var command = new SQLiteCommand(selectQuery, connection))
                {
                    command.Parameters.AddWithValue("@Username", username);
                    object result = command.ExecuteScalar();
                    return result?.ToString();
                }
            }
        }
    }
}
