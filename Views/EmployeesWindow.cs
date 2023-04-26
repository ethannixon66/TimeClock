using Terminal.Gui;

public class EmployeesWindow : Window
{
    public EmployeesWindow()
    {
        X = 3;
        Y = 3;
        Width = Dim.Fill(3);
        Height = Dim.Fill(3);
        Border.BorderStyle = BorderStyle.None;
        ColorScheme = Colors.Dialog;
        
        var frame = new FrameView()
        {
            Title = "Employees",
            Height = Dim.Fill(),
            Width = Dim.Percent(50),
            X = 0,
            Y = 0,
            CanFocus = true,
        };

        var employeesListView = new ListView(EmployeeDatabase.Employees)
        {
            X = 1,
            Y = 1,
            Height = Dim.Fill(),
            Width = Dim.Fill(1),
            AllowsMarking = false,
            AllowsMultipleSelection = false,
            CanFocus = true,
        };
        var currentEmployee = EmployeeDatabase.Employees[employeesListView.SelectedItem];
        var frame2 = new EmployeeInfoView(employeesListView, currentEmployee)
        {
            X = Pos.Right(frame) + 1,
            Y = 0,
            Height = Dim.Percent(50),
            Width = Dim.Fill(),
            CanFocus = true,
        };

        var frame3 = new EmployeePayoutView(employeesListView, currentEmployee)
        {
            X = frame2.X,
            Y = Pos.Bottom(frame2),
            Height = Dim.Fill(),
            Width = Dim.Fill()
        };
        employeesListView.SelectedItemChanged += (arg) =>
        {
            var employee = EmployeeDatabase.Employees[arg.Item];
            
            frame2.Employee = employee;
            frame2.Init();
            frame3.Employee = employee;
            frame3.Init();
        };
        
        frame.Add(employeesListView);
        Add(frame, frame2, frame3);
        foreach (var view in Subviews)
        {
            view.Enter += (args) =>
            {
                MainWindow.Instance.Title = "Employee Info (Ctrl + Q to return to main window)";
                Application.Refresh();
            };
        }
        Activate += (args) =>
        {
            employeesListView.SetSource(EmployeeDatabase.Employees);
        };

    }

}


