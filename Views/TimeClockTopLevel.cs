using System;
using Terminal.Gui;

public class TimeClockTopLevel : Toplevel 
{
    public TimeClockTopLevel()
    {
        var menuItem = new MenuItem("New Employee", "", () => throw new Exception());
        MenuBar = new MenuBar(new MenuBarItem[] {
                    new MenuBarItem("_File", new MenuItem [] {
                        new MenuItem("Admin Login", "", () => {
                            Application.Run(new AdminLogOnWindow());
                        }),
                    })
                });
        Add(MenuBar);
        Add(MainWindow.Instance);
    } 
}
