using System.Collections;
using Terminal.Gui;

public class EditShiftView : EditTimeUnitView
{
    private Label wageLabel;
    private TextField wageText;
    
    public Shift Shift
    {
        get => (Shift)TimeUnit;
        set { TimeUnit = value; }
    }

    public EditShiftView(ListView shiftsList, Shift shift, Employee employee, IList listViewSource)
        : base(shiftsList, shift, employee, listViewSource)
    {
        
    }
    public override void Init()
    {
        MainWindow.Instance.Title = $"{Employee} (Ctrl + Q to return to previous window)";
        Application.Refresh();
        base.Init();
        wageLabel = new Label("Hourly Wage: $")
        {
            X = 1,
            Y = endTimeText.Y + 2,
        };
        wageText = new TextField(Shift.HourlyWage.ToString())
        {
            Enabled = false,
            X = Pos.Right(wageLabel),
            Y = wageLabel.Y,
            Width = Dim.Fill(1)
        };
        saveChanges.Y = wageLabel.Y + 2;
        // Removing and adding saveChanges ensures that tabbing order is correct
        Remove(saveChanges);
        Add(wageLabel, wageText);
        Add(saveChanges);
    }

    public override void SaveChanges_Clicked()
    {
        // toggles mode
        if (saveChanges.Text == "Edit")
        {
            wageText.Enabled = true;
        }
        else
        {
            var current = Employee.AllShifts[ListView.SelectedItem];
            decimal parsedWage;
            if (!decimal.TryParse(wageText.Text.ToString(), out parsedWage))
            {
                MessageBox.ErrorQuery("Error", "Wage could not be parsed", "Ok");
                return;
            }
            else
            {
                current.HourlyWage = parsedWage;
            }
            wageText.Enabled = false;
        }
        base.SaveChanges_Clicked();
    }
    public override void EditTimeUnitView_Leave(FocusEventArgs args)
    {
        /// disables text input if focus leaves the window
        if (saveChanges.Text == "Save Changes")
        {
            wageText.Enabled = false;
            wageText.Text = Shift.HourlyWage.ToString();
        }
        base.EditTimeUnitView_Leave(args);
    }
}

