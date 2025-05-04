using System;
using System.IO;
using System.Collections.Generic;

class Program
{
    static string diaryFilePath = "diary.txt";
    static string userFilePath = "users.txt";
    static string? currentUser = null;

    static void Main()
    {
        EnsureFileExists(diaryFilePath);
        EnsureFileExists(userFilePath);

        while (true)
        {
            Console.WriteLine("Welcome to the Diary App!");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.Write("Enter your choice: ");
            string? choice = Console.ReadLine();

            if (choice == "1")
            {
                if (Login())
                    break;
            }
            else if (choice == "2")
            {
                Register();
            }
            else
            {
                Console.WriteLine("Invalid choice.\n");
            }
        }

        while (true)
        {
            Console.WriteLine("\nDiary Menu:");
            Console.WriteLine("1. Write Entry");
            Console.WriteLine("2. View All Entries");
            Console.WriteLine("3. Search by Date");
            Console.WriteLine("4. Logout");
            Console.Write("Enter your choice: ");
            string? menuChoice = Console.ReadLine();

            switch (menuChoice)
            {
                case "1":
                    WriteEntry();
                    break;
                case "2":
                    ViewEntry();
                    break;
                case "3":
                    SearchDate();
                    break;
                case "4":
                    Console.WriteLine("Logged out.");
                    return;
                default:
                    Console.WriteLine("Invalid choice");
                    break;
            }
        }
    }

    static void EnsureFileExists(string path)
    {
        if (!File.Exists(path))
        {
            File.Create(path).Close();
        }
    }

    static void Register()
    {
        Console.Write("Enter a new username: ");
        string? username = Console.ReadLine();
        Console.Write("Enter a new password: ");
        string? password = Console.ReadLine();

        if (username == "" || password == "")
        {
            Console.WriteLine("Username and password cannot be empty.");
            return;
        }

        if (File.ReadAllText(userFilePath).Contains(username + ":"))
        {
            Console.WriteLine("Username already exists.");
            return;
        }

        File.AppendAllText(userFilePath, $"{username}:{password}\n");
        Console.WriteLine("Registration successful.\n");
    }

    static bool Login()
    {
        Console.Write("Username: ");
        string? username = Console.ReadLine();
        Console.Write("Password: ");
        string? password = Console.ReadLine();

        string[] lines = File.ReadAllLines(userFilePath);
        foreach (string line in lines)
        {
            string[] parts = line.Split(':');
            if (parts.Length == 2 && parts[0] == username && parts[1] == password)
            {
                currentUser = username;
                Console.WriteLine("Login successful!\n");
                return true;
            }
        }

        Console.WriteLine("Invalid username or password.\n");
        return false;
    }

    static void WriteEntry()
    {
        Console.WriteLine("Write your entry:");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input))
            return;
        
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string entry = $"[{currentUser}] {timestamp}\n{input}\n---\n";
        File.AppendAllText(diaryFilePath, entry);
        Console.WriteLine("Entry added.");
    }

    static void ViewEntry(){}

    static void SearchDate(){}
}
