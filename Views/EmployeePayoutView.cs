using Terminal.Gui;
using System.Linq;

public class EmployeePayoutView : FrameView
{
    public Employee Employee { get; set; }
    public ListView EmployeesListView { get; set; }
    public EmployeePayoutView(ListView listView, Employee employee)
    {
        Employee = employee;
        EmployeesListView = listView;
        ColorScheme = Colors.Dialog;
        CanFocus = true;
        Init();
    }
    public void Init()
    {
        Title = $"Payout Info";
        var totalPayout = Employee.CompletedShifts.Select(s => s.Payout).Sum();
        var totalPayoutLabel = new Label()
        {

            Text = $"Total Payout: ${totalPayout}",
            X = 1,
            Y = 1,
        };

        var lastShift = Employee.CompletedShifts.LastOrDefault();
        var payout = lastShift == null ? 0m : lastShift.Payout;
        var lastShiftPayoutLabel = new Label()
        {
            Text = $"Last Shift Payout: ${payout}",
            X = 1,
            Y = totalPayoutLabel.Y + 1,
        };
        var viewAllShifts = new Button("View All Shifts")
        {
            X = Pos.Center(),
            Y = Pos.Bottom(lastShiftPayoutLabel),
        };
        viewAllShifts.Clicked += () =>
        {
            if (Employee.AllShifts.Count == 0)
            {
                MessageBox.ErrorQuery("Error", "Employee has no shifts", "Ok");
                return;
            }
            Application.Run(new AllShiftsView(Employee));
            Init();
        };

        RemoveAll();
        Add(totalPayoutLabel, lastShiftPayoutLabel, viewAllShifts);
    }
}

