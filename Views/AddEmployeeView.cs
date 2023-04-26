using Terminal.Gui;

public class AddEmployeeView : Window
{
    public AddEmployeeView()
    {
        X = 3;
        Y = 3;
        Width = Dim.Fill(3);
        Height = Dim.Fill(3);
        //Title = "Info For New Employee";
        ColorScheme = Colors.Dialog;
        Border.BorderStyle = BorderStyle.Rounded;
        var fNameLabel = new Label("First Name:")
        {
            X = 1,
            Y = 1,
        };
        var fNameText = new TextField()
        {
            X = Pos.Right(fNameLabel) + 1,
            Y = fNameLabel.Y,
            Width = Dim.Fill(1),
        };
        var lNameLabel = new Label("Last Name:")
        {
            X = 1,
            Y = fNameLabel.Y + 2,
        };
        var lNameText = new TextField()
        {
            X = Pos.Right(lNameLabel) + 1,
            Y = lNameLabel.Y,
            Width = Dim.Fill(1),
        };
        var wageLabel = new Label("Hourly Wage: $")
        {
            X = 1,
            Y = lNameLabel.Y + 2,
        };

        var wageText = new TextField()
        {
            X = Pos.Right(wageLabel),
            Y = wageLabel.Y,
            Width = Dim.Fill(1),

        };
        var breaksLabel = new Label("Allotted Breaks:")
        {
            X = 1,
            Y = wageLabel.Y + 2,

        };
        var breaksText = new TextField()
        {
            X = Pos.Right(breaksLabel) + 1,
            Y = breaksLabel.Y,
            Width = Dim.Fill(1)
        };
        var createEmployee = new Button()
        {

            Text = "Create Employee",
            X = Pos.Center(),
            Y = Pos.Bottom(breaksText) + 2,
            IsDefault = true
        };
        createEmployee.Clicked += () =>
        {

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
                MessageBox.ErrorQuery("Error", "Allotted breaks could not be parsed", "Ok");
                return;
            }
            var fName = fNameText.Text.ToString();
            var lName = lNameText.Text.ToString();
            var employee = new Employee(fName, lName, parsedWage, parsedBreaks);
            EmployeeDatabase.AddEmployee(employee);
            MessageBox.Query("Success", $"Employee #{employee.Id} successfully created", "Ok");
            Application.RequestStop();

        };
        Add(fNameLabel, fNameText, lNameLabel, lNameText, wageLabel,
            wageText, breaksText, createEmployee, breaksLabel);
    }
}

