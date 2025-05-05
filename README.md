# 📔 Digital Diary

A simple console-based **Digital Diary** written in C#. This app allows users to register, log in, write and view their diary entries, and mark specific entries as their favorites. Each diary entry is saved with a timestamp and associated with a specific user for privacy and organization.

---

## ✨ Features

- 📝 User registration and login system
- 📖 Writing and viewing personal diary entries
- 🔎 Search by date
- 💗 Marking diary entries as favorites
- 📂 User data and entries saved in `.txt` files for persistence
- 🧑‍💼 Personalized view per user

---

## 🧠 Object-Oriented Programming Principles Used

Though written in a single class, the application incorporates several key **OOP principles**:

To demonstrate Object-Oriented Programming (OOP) principles in your **Digital Diary C# project**, you'll need to refactor your procedural code into classes and objects that clearly reflect the four OOP principles:

---

### 1. **Encapsulation**

> Wrapping related data and behavior into classes and restricting direct access to them.

**Example:** Use `User`, `DiaryEntry`, and `DiaryManager` classes.

```csharp
class User
{
    public string Username { get; private set; }
    public string Password { get; private set; }

    public User(string username, string password)
    {
        Username = username;
        Password = password;
    }
}
```

The password is kept private; it's only modifiable inside the class, not outside.

---

### 2. **Abstraction**

> Hiding complex logic behind simple methods or interfaces.

**Example:** Use `DiaryManager.WriteEntry()` instead of exposing how the diary file is written.

```csharp
class DiaryManager
{
    private string diaryFilePath;
    private string currentUser;

    public DiaryManager(string diaryFilePath, string currentUser)
    {
        this.diaryFilePath = diaryFilePath;
        this.currentUser = currentUser;
    }

    public void WriteEntry(string content)
    {
        string timestamp = DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss");
        string entry = $"[{currentUser}] {timestamp}\n{content}\n---\n";
        File.AppendAllText(diaryFilePath, entry);
    }
}
```

The main program doesn't need to know how the entry is written to the file.

---

### 3. **Inheritance**

> Allowing a class to inherit from another class.

**Example:** You can create a base class `Entry` and let `DiaryEntry` and `FavoriteEntry` inherit from it.

```csharp
class Entry
{
    public string Author { get; set; }
    public string Timestamp { get; set; }
    public string Content { get; set; }
}

class DiaryEntry : Entry { }

class FavoriteEntry : Entry { }
```

`DiaryEntry` and `FavoriteEntry` reuse fields and behaviors from `Entry`.

---

### 4. **Polymorphism**

> Using a base class reference to refer to derived class objects.

**Example:** You could store both diary and favorite entries in a single list of type `Entry`.

```csharp
List<Entry> entries = new List<Entry>();
entries.Add(new DiaryEntry { Author = "Kristal", Timestamp = "2025-05-05", Content = "My day was fun!" });
entries.Add(new FavoriteEntry { Author = "Kristal", Timestamp = "2025-05-05", Content = "Fav entry." });

foreach (Entry entry in entries)
{
    Console.WriteLine(entry.Content);  // Works on both types
}
```

The same interface (`Entry`) works for multiple object types.

---

## ▶️ How to Run the App

1. Make sure you have **.NET SDK** installed on your machine.
2. Clone this repository or copy the source code to your local folder.
3. Open a terminal in the project directory.
4. Run the app using the following command:

```bash
dotnet run
````

---

## 📁 File Structure

```
Digital Diary/
│
├── .idea/.idea.Digital Diary/.idea/         # JetBrains Rider or IntelliJ project metadata
│   ├── encodings.xml
│   ├── indexLayout.xml
│   ├── vcs.xml
│   └── ...
│
├── .vs/                                     # Visual Studio settings
│   ├── Digital Diary/
│   │   ├── DesignTimeBuild/
│   │   │   └── .dtbcache.v2
│   │   ├── v17/
│   │   │   ├── .wsuo
│   │   │   └── DocumentLayout.json
│   │   ├── .suo
│   │   ├── DocumentLayout.json
│   │   └── DocumentLayout.backup.json
│
├── Digital Diary/                           # Main project folder
│   │
│   ├── bin/Debug/net9.0/                    # Compiled output
│   │   ├── Digital Diary.exe                # Executable app
│   │   ├── Digital Diary.dll
│   │   ├── Digital Diary.pdb
│   │   ├── Digital Diary.deps.json
│   │   └── Digital Diary.runtimeconfig.json
│   │
│   ├── diary.txt                            # Stores all diary entries (shared file)
│   ├── users.txt                            # Stores username-password pairs
│   ├── favorites.txt                        # (Optional) Shared favorites (not currently used)
│   ├── kc_favorites.txt                     # Example of a user-specific favorites file
│
│   ├── obj/                                 # Intermediate build files
│   │   └── ...                              # Auto-generated files
│
│   ├── Digital Diary.csproj                 # Project configuration file for C#
│   ├── Program.cs                           # Main C# source code file
│
├── Digital Diary.sln                        # Visual Studio solution file
├── README.md                                # Project documentation

```

---

## Sample Output

```
╔════════════════════════════════╗
║   Welcome to the Diary App!    ║
╚════════════════════════════════╝

1. Login
2. Register

Enter your choice: 1

╔═══════════════╗
║     Login     ║
╚═══════════════╝

Username: kristal
Password: *****

Login successful!

╔═════════════════╗
║   Diary Menu    ║
╚═════════════════╝

1. Write Entry
2. View All Entries
3. Search by Date
4. Favorites
5. Logout
```

---

## 🧑‍💻 Team Members

* **De Castro, Ayelet Darcy**
  - Email: [23-01387@g.batstate-u.edu.ph](mailto:23-01387@g.batstate-u.edu.ph)

* **Dimayuga, Kristal Clarisse**
  - Email: [23-08318@g.batstate-u.edu.ph](mailto:23-08318@g.batstate-u.edu.ph)

* **Fajutnao, Brylle Julian**
  - Email: [23-02509@g.batstate-u.edu.ph](mailto:23-02509@g.batstate-u.edu.ph)

* **Villar, Vince Anjo**
  - Email: [23-01484@g.batstate-u.edu.ph](mailto:23-01484@g.batstate-u.edu.ph)

---

## 🙏 Acknowledgement

We would like to sincerely thank **Ms. Fatima Marie Agdon, MSCS** for her continuous guidance and support as our project adviser.

