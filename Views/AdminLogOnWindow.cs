using Terminal.Gui;

public class AdminLogOnWindow : Window
{

    public AdminLogOnWindow()
    {
        X = 3;
        Y = 3;
        Width = Dim.Fill(3);
        Height = Dim.Fill(3);
        Border.BorderStyle = BorderStyle.Rounded;
        ColorScheme = Colors.Dialog;
        Title = "Admin Login";

        // Create input components and labels
        var usernameLabel = new Label()
        {
            Text = "Username:",
            X = Pos.Center() - 20,
            Y = Pos.Center() - 2
        };

        var usernameText = new TextField("")
        {
            X = Pos.Right(usernameLabel) + 1,
            Y = usernameLabel.Y,
            Width = 32
        };

        var passwordLabel = new Label()
        {
            Text = "Password:",
            X = Pos.Left(usernameLabel),
            Y = Pos.Bottom(usernameLabel) + 1
        };

        var passwordText = new TextField("")
        {
            Secret = true,
            X = Pos.Left(usernameText),
            Y = Pos.Top(passwordLabel),
            Width = 32,
        };

        var confirm = new Button("Confirm")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(passwordText) + 1,
            IsDefault = true,
        };

        confirm.Clicked += () =>
        {
            if (usernameText.Text != AdminAccountManager.Account.Username ||
                passwordText.Text != AdminAccountManager.Account.Password)
            {
                MessageBox.ErrorQuery("Error", "Wrong username or password", "Okay");
                return;

            }
            // necessary or hitting enter on the next window will for some
            // reason toggle the default button from this window
            confirm.Enabled = false;

            Application.RequestStop();
            var choice = MessageBox.Query(
                    "Welcome",
                    "\nWould you like to add a new employee or view existing employees?",
                    "Add new",
                    "View existing"
                    );

            if (choice == 0)
            {
                MainWindow.Instance.Title = "Add New Employee (Ctrl + Q to return to main window)";
                Application.Refresh();
                Application.Run(new AddEmployeeView());
                MainWindow.Instance.Title = MainWindow.DefaultTitle;
            }
            else if (choice == 1)
            {
                if (EmployeeDatabase.Employees.Count == 0)
                {
                    MessageBox.ErrorQuery("Error", "There are no employees in the system", "Ok");
                    return;
                }
                MainWindow.Instance.Title = "Employee Info (Ctrl + Q to return to main window)";
                Application.Refresh();
                Application.Run(new EmployeesWindow());
                MainWindow.Instance.Title = MainWindow.DefaultTitle;
            }
        };

        // Add the views to the Window
        Add(usernameLabel, usernameText, passwordLabel, passwordText, confirm);

    }
}