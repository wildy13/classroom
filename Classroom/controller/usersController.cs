using Classroom.model;
using Classroom.view;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classroom.controller
{
    internal class usersController
    {
        public static void CreateUser(string currentUserRole)
        {
            if (currentUserRole?.ToLower() != "teacher")
            {
                Console.WriteLine("You are not authorized to create a new user.");
                return;
            }

            Console.WriteLine("=== Create New User ===");
            Console.Write("Username: ");
            string? username = Console.ReadLine();
            Console.Write("Email: ");
            string? email = Console.ReadLine();
            Console.Write("Password: ");
            string? password = ReadPassword();

            if (username != null && email != null && password != null)
            {
                var roles = roleModel.GetRoles();
                Console.WriteLine("Available Roles:");
                for (int i = 0; i < roles.Count; i++)
                {
                    Console.WriteLine($"{i + 1}. {roles[i]}");
                }

                Console.Write("Choose Role: ");
                if (int.TryParse(Console.ReadLine(), out int roleIndex) && roleIndex >= 1 && roleIndex <= roles.Count)
                {
                    string selectedRole = roles[roleIndex - 1];
                    int roleId = roleModel.GetRoleId(selectedRole);

                    usersModel.SaveUser(username, email, password, roleId);
                }
                else
                {
                    Console.WriteLine("Invalid role selected.");
                }
            }
            else
            {
                Console.WriteLine("Username, email, and password cannot be null.");
            }
        }

        public static string ReadPassword()
        {
            string password = string.Empty;
            ConsoleKey key;

            do
            {
                var keyInfo = Console.ReadKey(intercept: true);
                key = keyInfo.Key;

                if (key == ConsoleKey.Backspace && password.Length > 0)
                {
                    Console.Write("\b \b");
                    password = password[0..^1];
                }
                else if (!char.IsControl(keyInfo.KeyChar))
                {
                    Console.Write("*");
                    password += keyInfo.KeyChar;
                }
            } while (key != ConsoleKey.Enter);

            Console.WriteLine();
            return password;
        }
    }
}
