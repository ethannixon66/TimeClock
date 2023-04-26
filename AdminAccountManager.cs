using System;
using System.IO;
using System.Linq;
using System.Text.Json;

public static class AdminAccountManager
{
    public static AdminAccount Account { get; set; }

    private static string s_filesLocation = Path.Combine(Directory.GetCurrentDirectory(), "files");

    public static void LoadAccount()
    {
        if (!Directory.Exists(s_filesLocation))
        {
            Directory.CreateDirectory(s_filesLocation);
        }
        var filePath = Directory
            .GetFiles(s_filesLocation)
            .SingleOrDefault(path => path.EndsWith("admin.dat"));

        if (filePath == null)
        {
            Account = new AdminAccount();
        }
        else
        {
            var jsonString = File.ReadAllText(filePath);
            Account = JsonSerializer.Deserialize<AdminAccount>(jsonString);
        }
    }
    public static void SaveAccount()
    {
        if (!Directory.Exists(s_filesLocation))
        {
            Directory.CreateDirectory(s_filesLocation);
        }
        var jsonString = JsonSerializer.Serialize(Account);
        var path = Path.Combine(s_filesLocation, "admin.dat");
        File.WriteAllText(path, jsonString);
    }

}

