using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Text.Json;
using System.Text.RegularExpressions;

public static class EmployeeDatabase
{
    private static List<Employee> _employees;

    public static List<Employee> Employees
    {
        get => _employees;
        private set
        {
            _employees = value;
        }
    }

    private static string s_filesLocation;
    public static void LoadDatabase()
    { 

        s_filesLocation = Path.Combine(Directory.GetCurrentDirectory(), "files");
        if (!Directory.Exists(s_filesLocation))
        {
            Directory.CreateDirectory(s_filesLocation);
        }
        var fileNames = Directory
            .GetFiles(s_filesLocation)
            .Where(file => Regex.IsMatch(file, @"\d+\.dat$"))
            .ToList();
        if (fileNames.Count > 0)
        {
            var jsonStrings = fileNames.Select(file => File.ReadAllText(file));
            Employees = jsonStrings.Select(json => JsonSerializer.Deserialize<Employee>(json)).ToList();
            Employees.Sort();
        }
        else
        {
            Employees = new List<Employee>();
        }
        
       
    }
    public static void SaveDatabase()
    {
        var employeeJsons = Employees.Select(employee => JsonSerializer.Serialize(employee)).ToList();

        var jsonAndId = Employees.Zip(employeeJsons, (e, j) => (e.Id, j));
        foreach (var (id, json) in jsonAndId)
        {
            var path = Path.Combine(s_filesLocation, $"{id}.dat");
            File.WriteAllText(path, json);

        }
    }

    public static void SaveSingle(string id)
    {
        var employeeJson = Employees
            .Where(e => e.Id == id)
            .Select(e => JsonSerializer.Serialize(e))
            .Single();
        var path = Path.Combine(s_filesLocation, $"{id}.dat");
        File.WriteAllText(path, employeeJson);
    }

    public static void SaveSingleAndReload(string id)
    {
        SaveSingle(id);
        LoadDatabase();
    }

    public static void SaveAndReload()
    {
        SaveDatabase();
        LoadDatabase();
    }

    public static void AddEmployee(Employee employee)
    {

        if (!Employees.Contains(employee))
        {
            Employees.Add(employee);
            SaveAndReload();
        }
    }

    public static void DeleteEmployee(Employee employee)
    {
        if (Employees.Contains(employee))
        {
            Employees.Remove(employee);
            var path = Path.Combine(s_filesLocation, $"{employee.Id}.dat");
            File.Delete(path);
        }
    }

    public static void DeleteAllEmployees()
    {
        Employees.ForEach(e => DeleteEmployee(e));
    }
}

