using System.IO;
using System;
using System.Collections.Generic;

class Program
{
    static string filePath = "diary.txt";

    static void Main()
    {
        while (true) 
        {
            
            EnsureFileExists();
            
            Console.WriteLine("Diary Menu: ");
            Console.WriteLine("1. Write Entry");
            Console.WriteLine("2. View all entry");
            Console.WriteLine("3. Search by Date");
            Console.Write("Enter your choice: ");
            string? choice = Console.ReadLine();

            switch (choice)
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
                default:
                    Console.WriteLine("Invalid choice");
                    break;  
            }
        }
    }

    static void EnsureFileExists()
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
    }
    static void WriteEntry()
    {
        Console.WriteLine("Write your entry: ");
        string input = Console.ReadLine();
        if (input.ToLower() == "")
            return;
        
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        File.AppendAllText(filePath, timestamp + "\n" + input);
        Console.WriteLine("Entry added");


    }

    static void ViewEntry()
    {
        Console.WriteLine();
    }

    static void SearchDate()
    {
        Console.WriteLine();
    }
}