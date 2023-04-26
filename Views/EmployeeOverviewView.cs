using System;
using Terminal.Gui;
using System.Linq;

public class EmployeeOverviewView : Window
{
    public Employee Employee { get; set; }
    public EmployeeOverviewView(Employee employee)
    {
        Employee = employee;
        Title = employee.ToString();
        X = 3;
        Y = 3;
        Width = Dim.Fill(3);
        Height = Dim.Fill(3);
        Border.BorderStyle = BorderStyle.Rounded;
        ColorScheme = Colors.Dialog;

        var clockIn = new Button()
        {
            X = Pos.Center(),
            Y = Pos.Center() - 2,
            Text = "Clock In"
        };
        var clockOut = new Button()
        {
            X = Pos.Center(),
            Y = Pos.Bottom(clockIn),
            Text = "Clock Out"
        };
        var startBreak = new Button()
        {
            X = Pos.Center(),
            Y = Pos.Bottom(clockOut),
            Text = "Start Break"
        };
        var endBreak = new Button()
        {
            X = Pos.Center(),
            Y = Pos.Bottom(startBreak),
            Text = "End Break"
        };

        // Clicked methods
        clockIn.Clicked += () =>
        {
            Employee.ClockIn();
            EmployeeDatabase.SaveSingle(Employee.Id);
            MessageBox.Query("Success!", $"Succesfully clocked in at {DateTime.Now.ToString("hh:mm:ss tt")}!", "Okay");
            Application.RequestStop();
        };
        clockOut.Clicked += () =>
        {
            Employee.ClockOut();
            EmployeeDatabase.SaveSingle(Employee.Id);
            MessageBox.Query("Success!", $"Succesfully clocked out at {DateTime.Now.ToString("hh:mm:ss tt")}!", "Okay");
            Application.RequestStop();
        };
        startBreak.Clicked += () =>
        {
            Employee.StartBreak();
            EmployeeDatabase.SaveSingle(Employee.Id);
            MessageBox.Query("Success!", $"Succesfully started break at {DateTime.Now.ToString("hh:mm:ss tt")}!", "Okay");
            Application.RequestStop();
        };
        endBreak.Clicked += () =>
        {
            Employee.EndBreak();
            EmployeeDatabase.SaveSingle(Employee.Id);
            MessageBox.Query("Success!", $"Succesfully ended break at {DateTime.Now.ToString("hh:mm:ss tt")}!", "Okay");
            Application.RequestStop();
        };

        // conditions where buttons should be disabled
        if (Employee.CurrentShift.InProgress)
        {
            clockIn.Enabled = false;
        }
        else
        {
            clockOut.Enabled = false;
            startBreak.Enabled = false;
            endBreak.Enabled = false;
        }

        if (Employee.CurrentShift.Breaks.Count(b => b.HasEnded) >= Employee.AllottedBreaks)
        {
            startBreak.Enabled = false;
            endBreak.Enabled = false;
        }
        else if (Employee.CurrentShift.Breaks.Any(b => b.InProgress))
        {
            startBreak.Enabled = false;
            clockOut.Enabled = false;
        }
        else
        {
            endBreak.Enabled = false;
        }

        Add(clockIn, clockOut, startBreak, endBreak);
    }
}

