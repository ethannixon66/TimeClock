using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

public class Employee : IComparable<Employee>
{
    private static Random s_random = new Random();
    private decimal _hourlyWage;
    private int _allottedBreaks;
    private string _firstName;
    private string _lastName;

    public Employee(string firstName, string lastName, decimal hourlyWage, int allottedBreaks)
    {
        Id = GenerateUniqueId();
        FirstName = firstName;
        LastName = lastName;
        HourlyWage = hourlyWage;
        AllottedBreaks = allottedBreaks;
        CurrentShift = new Shift(hourlyWage, AllottedBreaks);
        CompletedShifts = new List<Shift>();
    }
    public Employee() { }

    public Shift CurrentShift { get; set; }
    public List<Shift> CompletedShifts { get; set; }
    [JsonIgnore]
    public List<Shift> AllShifts
    {
        get
        {
            return CurrentShift.StartTime == null ? CompletedShifts : CompletedShifts.Append(CurrentShift).ToList();
        }
    }
    
    public string Id { get; set; }
    public string FirstName
    {
        get => _firstName;
        set
        {
            if (value.Length == 0)
            {
                throw new ArgumentException("First name cannot be empty");
            }
            _firstName = value;
        }
    }
    public string LastName
    {
        get => _lastName;
        set
        {
            if (value.Length == 0)
            {
                throw new ArgumentException("Last name cannot be empty");
            }
            _lastName = value;
        }
    }
    public int AllottedBreaks
    {
        get => _allottedBreaks;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("NumBreaks must be > 0");
            }
            _allottedBreaks = value;
        }

    }

    public decimal HourlyWage
    {
        get => _hourlyWage;
        set
        {
            if (value < 0)
            {
                throw new ArgumentOutOfRangeException("Wage cannot be negative");
            }
            _hourlyWage = value;
        }

    }

    public void ClockOut()
    {
        CurrentShift.End();
        CompletedShifts.Add(CurrentShift);
        CurrentShift = new Shift(HourlyWage, AllottedBreaks);
    }

    public void ClockIn()
    {
        CurrentShift.Start();
    }

    public void StartBreak() => CurrentShift.StartBreak();
    public void EndBreak() => CurrentShift.EndBreak();

    private string GenerateUniqueId()
    {
        if (EmployeeDatabase.Employees == null)
        {
            throw new InvalidOperationException("Database must be loaded before employees can be created");
        }
        string id;
        var i = 0;
        do
        {
            i += 1;
            id = s_random.Next(1, 999999).ToString().PadLeft(6, '0');
        } while (EmployeeDatabase.Employees.Select(e => e.Id).Contains(id));
        return id;
    }
    public override string ToString()
    {
        var s = $"ID: {Id} | Name: {LastName}, {FirstName}";
        return s;
    }

    public int CompareTo(Employee other)
    {
        return LastName.CompareTo(other.LastName) == 0 ?
            FirstName.CompareTo(other.FirstName) :
            LastName.CompareTo(other.LastName);
    }
}

