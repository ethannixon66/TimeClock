using System;
using Terminal.Gui;

public class Program
{


    static void Main(string[] args)
    {
        try
        {
            EmployeeDatabase.LoadDatabase();
            AdminAccountManager.LoadAccount();
            Application.Init();

            var top = new TimeClockTopLevel();
            Application.Run(top);
            Application.Shutdown();
        }
        catch (IndexOutOfRangeException)
        {
          
        }
        catch (Exception e)
        {
            Application.Shutdown();
            Console.WriteLine(e);
        }

    }
}