using System;
using System.IO;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static string filePath = "diary.txt";
    static string userFilePath = "users.txt";
    static string? currentUser = null;

    static void Main()
    {
        EnsureFileExists();
        EnsureUserFileExists();
        
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
            Console.WriteLine("\nDiary Menu: ");
            Console.WriteLine("1. Write Entry");
            Console.WriteLine("2. View all entries");
            Console.WriteLine("3. Search by Date");
            Console.WriteLine("4. Edit Entry");
            Console.WriteLine("5. Delete Entry");
            Console.WriteLine("6. Exit");
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
                case "4":
                    EditEntry();
                    break;
                case "5":
                    DeleteEntry();
                    break;
                case "6":    
                    Environment.Exit(0);
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
    
    static void EnsureUserFileExists()
    {
        if (!File.Exists(userFilePath))
        {
            File.Create(userFilePath).Close();
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
        Console.WriteLine("\nWrite your entry (press Enter twice to finish):");
        string input = "";
        string line;
        
        while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
        {
            input += line + "\n";
        }

        if (string.IsNullOrWhiteSpace(input))
        {
            Console.WriteLine("No entry was added.");
            return;
        }

        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        File.AppendAllText(filePath, $"[ENTRY]\nUser:{currentUser}\n{timestamp}\n{input}\n");
        Console.WriteLine("Entry added successfully.");
    }

    static void ViewEntry()
    {
        Console.WriteLine();
    }

    static void SearchDate()
    {
        Console.Write("\nEnter date to search (yyyy-MM-dd or yyyy-MM): ");
        string searchTerm = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            Console.WriteLine("Invalid date format.");
            return;
        }

        bool found = false;
        string[] allEntries = File.ReadAllText(filePath).Split(new[] {"[ENTRY]"}, StringSplitOptions.RemoveEmptyEntries);

        Console.WriteLine($"\nSearch Results for '{searchTerm}':");
        Console.WriteLine("--------------------------------");

        foreach (string entry in allEntries)
        {
            string[] entryParts = entry.Trim().Split('\n');
            if (entryParts.Length >= 2 && entryParts[0] == $"User:{currentUser}" && entryParts[1].Contains(searchTerm))
            {
                found = true;
                Console.WriteLine($"\nDate: {entryParts[1]}");
                Console.WriteLine("Content:");
                Console.WriteLine(string.Join("\n", entryParts.Skip(2)));
            }
        }

        if (!found)
        {
            Console.WriteLine("No entries found for the specified date.");
        }
    }

    static void EditEntry()
    {
        string[] allEntries = File.ReadAllText(filePath).Split(new[] {"[ENTRY]"}, StringSplitOptions.RemoveEmptyEntries);
        var userEntries = new List<string>();
        var entryNumbers = new List<int>();

        
        for (int i = 0; i < allEntries.Length; i++)
        {
            string[] entryParts = allEntries[i].Trim().Split('\n');
            if (entryParts.Length >= 1 && entryParts[0] == $"User:{currentUser}")
            {
                userEntries.Add(allEntries[i]);
                entryNumbers.Add(i + 1); 
            }
        }

        if (userEntries.Count == 0)
        {
            Console.WriteLine("No entries found for the current user.");
            return;
        }
        
        for (int i = 0; i < userEntries.Count; i++)
        {
            string[] entryParts = userEntries[i].Trim().Split('\n');
            Console.WriteLine($"\nEntry #{entryNumbers[i]}");
            Console.WriteLine($"Date: {entryParts[1]}");
            Console.WriteLine("Content:");
            Console.WriteLine(string.Join("\n", entryParts.Skip(2)));
        }

        Console.Write("\nEnter the number of the entry you want to edit: ");
        if (!int.TryParse(Console.ReadLine(), out int selectedNumber) || !entryNumbers.Contains(selectedNumber))
        {
            Console.WriteLine("Invalid entry number.");
            return;
        }

        int actualIndex = Array.IndexOf(entryNumbers.ToArray(), selectedNumber);
        string[] selectedEntryParts = userEntries[actualIndex].Trim().Split('\n');

        Console.WriteLine("\nCurrent entry:");
        Console.WriteLine($"Date: {selectedEntryParts[1]}");
        Console.WriteLine("Current content:");
        Console.WriteLine(string.Join("\n", selectedEntryParts.Skip(2)));

        Console.WriteLine("\nEnter new content (press Enter twice to finish):");
        string newContent = "";
        string line;
        
        while (!string.IsNullOrWhiteSpace(line = Console.ReadLine()))
        {
            newContent += line + "\n";
        }

        if (string.IsNullOrWhiteSpace(newContent))
        {
            Console.WriteLine("No changes were made.");
            return;
        }
        
        int originalIndex = entryNumbers[actualIndex] - 1;
        allEntries[originalIndex] = $"\nUser:{currentUser}\n{selectedEntryParts[1]}\n{newContent}";
        
        File.WriteAllText(filePath, string.Join("[ENTRY]", allEntries));
        Console.WriteLine("Entry updated successfully.");
    }

    static void DeleteEntry()
    {
        string[] allEntries = File.ReadAllText(filePath).Split(new[] {"[ENTRY]"}, StringSplitOptions.RemoveEmptyEntries);
        var userEntries = new List<string>();
        var entryNumbers = new List<int>();
        
        for (int i = 0; i < allEntries.Length; i++)
        {
            string[] entryParts = allEntries[i].Trim().Split('\n');
            if (entryParts.Length >= 1 && entryParts[0] == $"User:{currentUser}")
            {
                userEntries.Add(allEntries[i]);
                entryNumbers.Add(i + 1); 
            }
        }

        if (userEntries.Count == 0)
        {
            Console.WriteLine("No entries found for the current user.");
            return;
        }
        
        for (int i = 0; i < userEntries.Count; i++)
        {
            string[] entryParts = userEntries[i].Trim().Split('\n');
            Console.WriteLine($"\nEntry #{entryNumbers[i]}");
            Console.WriteLine($"Date: {entryParts[1]}");
            Console.WriteLine("Content:");
            Console.WriteLine(string.Join("\n", entryParts.Skip(2)));
        }

        Console.Write("\nEnter the number of the entry you want to delete (or type 'all' to delete all entries): ");
        string input = Console.ReadLine()?.Trim().ToLower();

        if (input == "all")
        {
            Console.Write("Are you sure you want to delete ALL your entries? (y/n): ");
            string confirmAll = Console.ReadLine();
            if (confirmAll?.ToLower() == "y")
            {
                var remainingEntries = allEntries.Where(entry => 
                    !entry.Trim().StartsWith($"User:{currentUser}")).ToArray();
                File.WriteAllText(filePath, string.Join("[ENTRY]", remainingEntries));
                Console.WriteLine("All your entries deleted.");
            }
            else
            {
                Console.WriteLine("Action cancelled.");
            }
            return;
        }

        if (!int.TryParse(input, out int selectedNumber) || !entryNumbers.Contains(selectedNumber))
        {
            Console.WriteLine("Invalid entry number.");
            return;
        }

        Console.Write("Are you sure you want to delete this entry? (y/n): ");
        string confirm = Console.ReadLine();
        if (confirm?.ToLower() != "y")
        {
            Console.WriteLine("Deletion cancelled.");
            return;
        }
        
        int originalIndex = selectedNumber - 1;
        allEntries = allEntries.Where((entry, index) => index != originalIndex).ToArray();
        File.WriteAllText(filePath, string.Join("[ENTRY]", allEntries));
        Console.WriteLine("Entry deleted successfully.");
    }
}