using Classroom.model;
using Classroom.view;
using System;
using System.Data.SQLite;

namespace Classroom
{
    class Program
    {
        static void Main(string[] args)
        {
            string databaseFile = "classroom.db";

            if (!File.Exists(databaseFile))
            {
                SQLiteConnection.CreateFile(databaseFile);
                roleModel.CreateRoleTable();
                usersModel.CreateUsersTable();
            }
            consoleView.ShowLogin();
        }
    }
}