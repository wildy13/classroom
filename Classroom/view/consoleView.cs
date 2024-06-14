using Classroom.controller;
using Classroom.model;
using System;
using System.Collections.Generic;

namespace Classroom.view
{
    internal class consoleView
    {
        public static void ShowLogin()
        {
            Console.WriteLine("=== Login ===");
            Console.Write("Username: ");
            string username = Console.ReadLine();
            Console.Write("Password: ");
            string password = usersController.ReadPassword();

            if (!string.IsNullOrEmpty(username) && !string.IsNullOrEmpty(password))
            {
                bool loginSuccess = usersModel.LoginUser(username, password);
                if (loginSuccess)
                {
                    Console.WriteLine("Login successful!");
                    ShowMainMenu(usersModel.GetUserRole(username) ?? string.Empty);
                }
                else
                {
                    Console.WriteLine("Login failed. Invalid username or password.");
                }
            }
            else
            {
                Console.WriteLine("Username and password cannot be null.");
            }
        }

        public static void ShowMainMenu(string currentUserRole)
        {
            string[] options;
            if (currentUserRole == "Teacher")
            {
                options = new string[] { "Create New User", "Create New Role", "Create New Room","Enter Room", "Exit" };
            }
            else if (currentUserRole == "Student")
            {
                options = new string[] { "Enter Room", "Exit" };
            }
            else
            {
                Console.WriteLine("Invalid user role.");
                return;
            }

            int selectedIndex = 0;
            ConsoleKey key;

            do
            {
                Console.Clear();
                Console.WriteLine("=== Main Menu ===");

                for (int i = 0; i < options.Length; i++)
                {
                    if (i == selectedIndex)
                    {
                        Console.BackgroundColor = ConsoleColor.White;
                        Console.ForegroundColor = ConsoleColor.Black;
                        Console.WriteLine(options[i]);
                        Console.ResetColor();
                    }
                    else
                    {
                        Console.WriteLine(options[i]);
                    }
                }

                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.UpArrow:
                        selectedIndex = (selectedIndex > 0) ? selectedIndex - 1 : options.Length - 1;
                        break;
                    case ConsoleKey.DownArrow:
                        selectedIndex = (selectedIndex < options.Length - 1) ? selectedIndex + 1 : 0;
                        break;
                    case ConsoleKey.Enter:
                        ExecuteOption(selectedIndex, currentUserRole);
                        break;
                }
            } while (key != ConsoleKey.Enter || (currentUserRole == "Teacher" && selectedIndex != 3) || (currentUserRole == "Student" && selectedIndex != 0));
        }

        private static void ExecuteOption(int selectedIndex, string currentUserRole)
        {
            if (currentUserRole == "Teacher")
            {
                switch (selectedIndex)
                {
                    case 0:
                        usersController.CreateUser(currentUserRole);
                        break;
                    case 1:
                        roleController.CreateRole(currentUserRole);
                        break;
                    case 2:
                        CreateRoom();
                        break;
                    case 3:
                        EnterRoom();
                        break;
                    case 4:
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            else if (currentUserRole == "Student")
            {
                switch (selectedIndex)
                {
                    case 0:
                        EnterRoom();
                        break;
                    case 1:
                        Console.WriteLine("Exiting...");
                        Environment.Exit(0);
                        break;
                    default:
                        Console.WriteLine("Invalid option.");
                        break;
                }
            }
            else
            {
                Console.WriteLine("Invalid user role.");
            }
        }

        private static void CreateRoom()
        {
            Console.WriteLine("Creating a new room...");
            string roomKey = GenerateUniqueKey();
            RoomKeyStorage.Rooms.Add(roomKey);
            Console.WriteLine($"Room created successfully. Share this key with your students: {roomKey}");

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private static void EnterRoom()
        {
            Console.Clear();
            Console.Write("Enter the room key provided by your teacher: ");
            string roomKey = Console.ReadLine().Trim();

            if (RoomKeyStorage.Rooms.Contains(roomKey))
            {
                Console.WriteLine("Entering the room...");
                Console.WriteLine($"You have entered room {roomKey}. Welcome!");
            }
            else
            {
                Console.WriteLine("Invalid room key. Please try again.");
            }

            Console.WriteLine("Press Enter to continue...");
            Console.ReadLine();
        }

        private static string GenerateUniqueKey()
        {
            return Guid.NewGuid().ToString("N");
        }

        public static class RoomKeyStorage
        {
            public static List<string> Rooms { get; } = new List<string>();
        }
    }
}
