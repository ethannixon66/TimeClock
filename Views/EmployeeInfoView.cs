using Terminal.Gui;

public class EmployeeInfoView : FrameView
{
    public Employee Employee { get; set; }
    public ListView EmployeesListView { get; set; }
    public EmployeeInfoView(ListView listView, Employee employee)
    {
        Employee = employee;
        EmployeesListView = listView;
        ColorScheme = Colors.Dialog;
        CanFocus = true;
        Init();
    }
    public void Init()
    {
        Title = $"Employee #{Employee.Id}";
        var fNameLabel = new Label("First name:")
        {
            X = 1,
            Y = 1,
        };
        var fNameText = new TextField()
        {
            Text = Employee.FirstName,
            X = Pos.Right(fNameLabel) + 1,
            Y = fNameLabel.Y,
            Width = Dim.Fill(1),
            Enabled = false,
        };
        var lNameLabel = new Label("Last name:")
        {
            X = 1,
            Y = fNameLabel.Y + 2,
        };
        var lNameText = new TextField()
        {
            Text = Employee.LastName,
            X = Pos.Right(lNameLabel) + 1,
            Y = lNameLabel.Y,
            Width = Dim.Fill(1),
            Enabled = false,
        };
        var wageLabel = new Label("Hourly wage: $")
        {
            X = 1,
            Y = lNameLabel.Y + 2,
        };

        var wageText = new TextField()
        {
            Text = Employee.HourlyWage.ToString(),
            X = Pos.Right(wageLabel),
            Y = wageLabel.Y,
            Width = Dim.Fill(1),
            Enabled = false,

        };
        var breaksLabel = new Label("Allotted Breaks:")
        {
            X = 1,
            Y = wageLabel.Y + 2,

        };
        var breaksText = new TextField()
        {
            Text = Employee.AllottedBreaks.ToString(),
            X = Pos.Right(breaksLabel) + 1,
            Y = breaksLabel.Y,
            Width = Dim.Fill(1),
            Enabled = false,
        };
        var saveChanges = new Button()
        {
            Text = "Edit",
            X = Pos.Center() ,
            Y = Pos.Bottom(breaksText) + 1,
        };
        var delete = new Button("Delete")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(saveChanges) + 1,
        };
        Leave += (args) =>
        {
            if (saveChanges.Text == "Save Changes")
            {
                saveChanges.Text = "Edit";
                wageText.Enabled = false;
                fNameText.Enabled = false;
                lNameText.Enabled = false;
                breaksText.Enabled = false;

                wageText.Text = Employee.HourlyWage.ToString();
                fNameText.Text = Employee.FirstName;
                lNameText.Text = Employee.LastName;
                breaksText.Text = Employee.AllottedBreaks.ToString();

            }
        };
        delete.Clicked += () =>
        {
            
            var choice = MessageBox.Query("Delete Employee?",
                $"Are you sure you want to permanently delete {Employee.FirstName} {Employee.LastName}?",
                "No",
                "Yes");
            // cancel delete
            if (choice == 0)
            {
                return;
            }
            //perform delete
            else 
            {
                EmployeeDatabase.DeleteEmployee(Employee);
                MessageBox.Query("Success", "Employee succesfully deleted", "Ok");
                EmployeesListView.SetSource(EmployeeDatabase.Employees);
            }
            // leave the employees view if there are none left to view
            if (EmployeeDatabase.Employees.Count == 0)
            {
                Application.RequestStop();
            }
            
        };
        saveChanges.Clicked += () =>
        {
            // toggle edit mode
            if (saveChanges.Text == "Edit")
            {
                saveChanges.Text = "Save Changes";
                wageText.Enabled = true;
                fNameText.Enabled = true;
                lNameText.Enabled = true;
                breaksText.Enabled = true;

            }
            // save changes
            else
            {
                var current = EmployeesListView.SelectedItem;
                decimal parsedWage;
                int parsedBreaks;
                if (fNameText.Text == "" || lNameText.Text == "")
                {
                    MessageBox.ErrorQuery("Error", "All fields required", "Ok");
                    return;
                }
                if (!decimal.TryParse(wageText.Text.ToString(), out parsedWage))
                {
                    MessageBox.ErrorQuery("Error", "Wage could not be parsed", "Ok");
                    return;
                }
                if (!int.TryParse(breaksText.Text.ToString(), out parsedBreaks))
                {
                    MessageBox.ErrorQuery("Error", "Number of breaks could not be parsed", "Ok");
                    return;
                }

                Employee.FirstName = fNameText.Text.ToString();
                Employee.LastName = lNameText.Text.ToString();
                Employee.HourlyWage = parsedWage;
                Employee.AllottedBreaks = parsedBreaks;
                EmployeeDatabase.SaveAndReload();
                MessageBox.Query("Success", "Information succesfully changed", "Ok");
                EmployeesListView.SetSource(EmployeeDatabase.Employees);
                EmployeesListView.SelectedItem = current;

            }

        };
        RemoveAll();
        Add(fNameLabel, fNameText, lNameLabel, lNameText, wageLabel,
            wageText, breaksLabel, breaksText, saveChanges, delete);
    }
}

