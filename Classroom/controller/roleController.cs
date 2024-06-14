using Classroom.model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Classroom.controller
{
    internal class roleController
    {
        public static void CreateRole(string currentUserRole)
        {
            if (currentUserRole?.ToLower() != "teacher")
            {
                Console.WriteLine("You are not authorized to create a new role.");
                return;
            }

            Console.WriteLine("=== Create New Role ===");
            Console.Write("Name: ");
            string? name = Console.ReadLine();

            if (!string.IsNullOrWhiteSpace(name))
            {
                roleModel.SaveRole(name);
            }
            else
            {
                Console.WriteLine("Role name cannot be null or whitespace.");
            }
        }
    }
}
