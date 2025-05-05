using System;
using System.IO;
using System.Collections.Generic;
using System.Text;
using System.Linq;

class Program
{
    static string diaryFilePath = "diary.txt";
    static string userFilePath = "users.txt";
    static string favoritesFilePath = "favorites.txt"; 

    static void Main()
    {
        EnsureFileExists(diaryFilePath);
        EnsureFileExists(userFilePath);
        EnsureFileExists(favoritesFilePath);

        while (true)
        {
            PrintTitle("Welcome to the Diary App!📔");
            Console.WriteLine("1. Login");
            Console.WriteLine("2. Register");
            Console.Write("\nEnter your choice: ");
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
            Console.WriteLine();
            PrintTitle("Diary Menu");
            Console.WriteLine("1. Write Entry");
            Console.WriteLine("2. View all entries");
            Console.WriteLine("3. Search by Date");
            Console.WriteLine("4. Edit Entry");
            Console.WriteLine("5. Delete Entry");
            Console.WriteLine("6. Favorites");
            Console.WriteLine("7. Exit");
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
                    Favorites();
                    break;
                case "7":
                    Environment.Exit(0);
                    break;
                default:
                    Console.WriteLine("Invalid choice");
                    break;  
            }
        }
    }

    static void EnsureFileExists(string filePath)
    {
        if (!File.Exists(filePath))
        {
            File.Create(filePath).Close();
        }
    }
    
    static void PrintTitle(string title)
    {
        Console.OutputEncoding = Encoding.UTF8;
        int width = title.Length + 10;
        string top = "╔" + new string('═', width) + "╗";
        string middle = "║" + new string(' ', (width - title.Length) / 2) + title + new string(' ', (width - title.Length + 1) / 2) + "║";
        string bottom = "╚" + new string('═', width) + "╝";

        Console.WriteLine(top);
        Console.WriteLine(middle);
        Console.WriteLine(bottom);
    }
    
    static void Register()
    {
        Console.WriteLine();
        PrintTitle("Register ✏️");
        Console.Write("\nEnter a new username: ");
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
        Console.WriteLine();
        PrintTitle("Login ✏️");
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
        File.AppendAllText(diaryFilePath, $"[ENTRY]\nUser:{currentUser}\n{timestamp}\n{input}\n");
        Console.WriteLine("Entry added successfully.");
    }

    static void ViewEntry()
    {
        EnsureFileExists(diaryFilePath);
        string[] lines = File.ReadAllLines(diaryFilePath);

        Console.WriteLine();
        if (lines.Length == 0)
        {
            Console.WriteLine("Nothing to see here.");
        }

        PrintTitle($"{currentUser}'s Diary Entries 📔");
        Console.WriteLine();

        bool hasEntries = false;
        foreach (string line in lines) {
            if (line.Contains($"User:{currentUser}")) 
            {
                hasEntries = true;
                Console.WriteLine(line);
            }
        }

        if (!hasEntries) {
            Console.WriteLine("Nothing to see here.");
        }
    }

    static void SearchDate()
    {
        Console.Write("\nEnter date to search (yyyy-MM-dd or yyyy-MM): ");
        string? searchTerm = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(searchTerm))
        {
            Console.WriteLine("Invalid date format.");
            return;
        }

        bool found = false;
        string[] allEntries = File.ReadAllText(diaryFilePath).Split(new[] {"[ENTRY]"}, StringSplitOptions.RemoveEmptyEntries);

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
        string[] allEntries = File.ReadAllText(diaryFilePath).Split(new[] {"[ENTRY]"}, StringSplitOptions.RemoveEmptyEntries);
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
            Console.WriteLine("No content to update.");
            return;
        }

        string newTimestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string updatedEntry = $"User:{currentUser}\n{newTimestamp}\n{newContent}";

        allEntries[selectedNumber - 1] = updatedEntry;

        File.WriteAllText(diaryFilePath, string.Join("\n[ENTRY]", allEntries));
        Console.WriteLine("Entry updated successfully.");
    }

    static void DeleteEntry()
    {
        string[] allEntries = File.ReadAllText(diaryFilePath).Split(new[] {"[ENTRY]"}, StringSplitOptions.RemoveEmptyEntries);
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

        Console.Write("\nEnter the number of the entry you want to delete: ");
        if (!int.TryParse(Console.ReadLine(), out int selectedNumber) || !entryNumbers.Contains(selectedNumber))
        {
            Console.WriteLine("Invalid entry number.");
            return;
        }

        int actualIndex = Array.IndexOf(entryNumbers.ToArray(), selectedNumber);
        List<string> remainingEntries = allEntries.Where((e, index) => index != actualIndex).ToList();

        File.WriteAllText(diaryFilePath, string.Join("\n[ENTRY]", remainingEntries));
        Console.WriteLine("Entry deleted successfully.");
    }

    static void Favorites()
    {
        string favFile = $"{currentUser}_favorites.txt";
        EnsureFileExists(favFile);

        Console.WriteLine();
        PrintTitle($"{currentUser}'s Favorite Entries 💗");
        Console.WriteLine();

        string[] favorites = File.ReadAllLines(favFile);
        if (favorites.Length == 0)
        {
            Console.WriteLine("You have no favorite entries yet.\n");
        }
        else
        {
            string currentEntry = "";
            foreach (string line in favorites)
            {
                if (line == "---")
                {
                    Console.WriteLine(currentEntry);
                    Console.WriteLine("------------------------");
                    currentEntry = "";
                }
                else
                {
                    currentEntry += line + "\n";
                }
            }
            
            // In case the last entry doesn't end with "---"
            if (!string.IsNullOrWhiteSpace(currentEntry))
            {
                Console.WriteLine(currentEntry);
            }
        }

        Console.WriteLine();
        Console.WriteLine("1. Add a favorite entry");
        Console.WriteLine("2. Remove a favorite entry");
        Console.WriteLine("3. Return to main menu");
        Console.Write("\nEnter your choice: ");
        string? choice = Console.ReadLine();

        switch (choice)
        {
            case "1":
                AddToFavorites();
                break;
            case "2":
                RemoveFromFavorites(favFile);
                break;
            default:
                return;
        }
    }

    static void AddToFavorites()
    {
        string[] allEntries = File.ReadAllText(diaryFilePath).Split(new[] {"[ENTRY]"}, StringSplitOptions.RemoveEmptyEntries);
        var userEntries = new List<string>();
        var entryIndices = new List<int>();
        
        Console.WriteLine();
        PrintTitle("Add a favorite diary entry 💗");
        Console.WriteLine();

        for (int i = 0; i < allEntries.Length; i++)
        {
            string[] entryParts = allEntries[i].Trim().Split('\n');
            if (entryParts.Length >= 1 && entryParts[0] == $"User:{currentUser}")
            {
                userEntries.Add(allEntries[i]);
                entryIndices.Add(i);
            }
        }

        if (userEntries.Count == 0)
        {
            Console.WriteLine("\nYou have no diary entries to favorite.");
            return;
        }

        for (int i = 0; i < userEntries.Count; i++)
        {
            string[] entryParts = userEntries[i].Trim().Split('\n');
            Console.WriteLine($"\nDiary Entry #{i + 1}");
            Console.WriteLine($"Date: {entryParts[1]}");
            Console.WriteLine("Content:");
            Console.WriteLine(string.Join("\n", entryParts.Skip(2)));
            Console.WriteLine("------------------------");
        }

        Console.Write("\nEnter the number(s) of the diary entry you want to favorite (comma-separated): ");
        string? input = Console.ReadLine();
        Console.WriteLine();

        if (string.IsNullOrWhiteSpace(input)) return;

        string[] selections = input.Split(',');
        string favFile = $"{currentUser}_favorites.txt";
        
        foreach (string sel in selections)
        {
            if (int.TryParse(sel.Trim(), out int index) && index >= 1 && index <= userEntries.Count)
            {
                string[] entryParts = userEntries[index - 1].Trim().Split('\n');
                string timestamp = entryParts[1];
                string content = string.Join("\n", entryParts.Skip(2));
                
                // Format the favorite entry
                string favoriteEntry = $"Date: {timestamp}\nContent:\n{content}\n---\n";
                
                File.AppendAllText(favFile, favoriteEntry);
                Console.WriteLine($"Diary Entry #{index} added to favorites.");
            }
            else
            {
                Console.WriteLine($"Invalid entry number: {sel}");
            }
        }
    }
    
    static void RemoveFromFavorites(string favFile)
    {
        string[] favorites = File.ReadAllLines(favFile);
        if (favorites.Length == 0)
        {
            Console.WriteLine("You have no favorite entries to remove.");
            return;
        }

        List<List<string>> entries = new List<List<string>>();
        List<string> currentEntry = new List<string>();

        foreach (string line in favorites)
        {
            if (line == "---")
            {
                entries.Add(new List<string>(currentEntry));
                currentEntry.Clear();
            }
            else
            {
                currentEntry.Add(line);
            }
        }

        // Add the last entry if it exists
        if (currentEntry.Count > 0)
        {
            entries.Add(currentEntry);
        }

        Console.WriteLine();
        PrintTitle("Remove a favorite entry 💔");
        Console.WriteLine();

        for (int i = 0; i < entries.Count; i++)
        {
            Console.WriteLine($"Favorite #{i + 1}:");
            foreach (string line in entries[i])
            {
                Console.WriteLine(line);
            }
            Console.WriteLine("------------------------");
        }

        Console.Write("\nEnter the number(s) of the favorite entry to remove (comma-separated): ");
        string? input = Console.ReadLine();

        if (string.IsNullOrWhiteSpace(input)) return;

        string[] selections = input.Split(',');
        List<int> toRemove = new List<int>();

        foreach (string sel in selections)
        {
            if (int.TryParse(sel.Trim(), out int index) && index >= 1 && index <= entries.Count)
            {
                toRemove.Add(index - 1);
                Console.WriteLine($"Favorite #{index} will be removed.");
            }
            else
            {
                Console.WriteLine($"Invalid entry number: {sel}");
            }
        }

        toRemove.Sort((a, b) => b.CompareTo(a));

        foreach (int index in toRemove)
        {
            entries.RemoveAt(index);
        }

        File.WriteAllText(favFile, "");
        foreach (var entry in entries)
        {
            foreach (string line in entry)
            {
                File.AppendAllText(favFile, line + "\n");
            }
            File.AppendAllText(favFile, "---\n");
        }

        Console.WriteLine("Selected favorites removed successfully.");
    }

    static void Exit()
    {
        Console.WriteLine("\nThank you for using the Diary App!");
        Environment.Exit(0);
    }

    static string? currentUser = null;
}