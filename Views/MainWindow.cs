using System;
using Terminal.Gui;

public class MainWindow : Window
{
    private static MainWindow _instance = null;
    public static string DefaultTitle { get; } = "TimeClock App (Ctrl + Q to quit)";
    public MainWindow()
    {
        Title = DefaultTitle;
        if (String.IsNullOrEmpty(AdminAccountManager.Account.Username) ||
            String.IsNullOrEmpty(AdminAccountManager.Account.Password))
        {
            Application.Run(new CreateAdminAccountWizard());
        }
        Add(new EmployeeLogOnWindow()); 
    }
    public static MainWindow Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = new MainWindow();
            }
            return _instance;
        }
    }
}

