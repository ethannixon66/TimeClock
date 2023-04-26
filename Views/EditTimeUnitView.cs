using System;
using System.Collections;
using NStack;
using Terminal.Gui;
public class EditTimeUnitView : FrameView
{
    protected Button saveChanges;
    protected Label startDateLabel;
    protected DateField startDateText;
    protected Label startTimeLabel;
    protected TwelveHourTimeField startTimeText;
    
    protected Label endDateLabel;
    protected DateField endDateText;
    protected Label endTimeLabel;
    protected TwelveHourTimeField endTimeText;
    private ustring originalStartTimePeriod;
    private ustring originalEndTimePeriod;

    public TimeUnit TimeUnit { get; set; }
    public ListView ListView { get; set; }
    public Employee Employee { get; set; }
    private IList  _listViewSource;
    public EditTimeUnitView(ListView listView, TimeUnit timeUnit, Employee employee, IList listViewSource)
    {
        if (!(listView.Source.ToList()[0] is TimeUnit))
        {
            throw new ArgumentException("ListView must be a list of TimeUnits");
        }
        TimeUnit = timeUnit;
        ListView = listView;
        Employee = employee;
        _listViewSource = listViewSource;
        ColorScheme = Colors.Dialog;
        Init();
    }
    public virtual void Init()
    {
        startDateLabel = new Label("Start Date:")
        {
            X = 1,
            Y = 0,
        };
        startDateText = new DateField()
        {
            X = Pos.Right(startDateLabel) + 1,
            Y = startDateLabel.Y,
            Date = TimeUnit.StartTime.GetValueOrDefault(),
            Enabled = false,
        };
        startTimeLabel = new Label("Start Time:")
        {
            X = 1,
            Y = startDateText.Y + 1,
        };
        startTimeText = new TwelveHourTimeField(TimeUnit.StartTime.GetValueOrDefault().TimeOfDay)
        {
            X = Pos.Right(startTimeLabel) + 1,
            Y = startTimeLabel.Y,
            IsShortFormat = true,
            Enabled = false,
        };

        
        endDateLabel = new Label("End Date:")
        {
            X = 1,
            Y = startTimeLabel.Y + 2,
        };
        endDateText = new DateField()
        {
            X = Pos.Right(endDateLabel) + 1,
            Y = endDateLabel.Y,
            Date = TimeUnit.EndTime.GetValueOrDefault(),
            Enabled = false,
            
        };
        endTimeLabel = new Label("End Time:")
        {
            X = 1,
            Y = endDateText.Y + 1,
        };
        endTimeText = new TwelveHourTimeField(TimeUnit.EndTime.GetValueOrDefault().TimeOfDay)
        {
            X = Pos.Right(endTimeLabel) + 1,
            Y = endTimeLabel.Y,
            IsShortFormat = true,
            Enabled = false,
        };
        
        saveChanges = new Button()
        {

            Text = "Edit",
            X = Pos.Center(),
            Y = Pos.Bottom(endTimeText) + 1,
            IsDefault = true
        };

        saveChanges.Clicked += SaveChanges_Clicked;
        if (startDateText.Date == default(DateTime))
        {
            startDateLabel.Text = "Start Date (Not Yet Started):";
            startTimeLabel.Text = "Start Time (Not Yet Started):";
        }
        if (endDateText.Date == default(DateTime))
        {
            endDateLabel.Text = "End Date (Not Yet Ended):";
            endTimeLabel.Text = "End Time (Not Yet Ended):";
        }
        Leave += EditTimeUnitView_Leave;
        RemoveAll();
        Add(startDateLabel, startDateText, startTimeLabel, startTimeText, endDateLabel, endDateText, endTimeLabel, endTimeText, saveChanges);
        originalStartTimePeriod = startTimeText.AMPMLabel.Text;
        originalEndTimePeriod = endTimeText.AMPMLabel.Text;
    }
    public virtual void SaveChanges_Clicked()
    {
        // handles toggling of edit mode
        if (saveChanges.Text == "Edit")
        {
            saveChanges.Text = "Save Changes";
            if (TimeUnit.HasStarted)
            startDateText.Enabled = true;
            startTimeText.Enabled = true;

            if (TimeUnit.HasEnded)
            {
                endDateText.Enabled = true;
                endTimeText.Enabled = true;
            }
            
        }
        // handles saving of channges
        else
        {
            var current = (TimeUnit)_listViewSource[ListView.SelectedItem];
            var oldStart = current.StartTime;
            var oldEnd = current.EndTime;
            try
            {
                var newStartTime = new DateTime(startDateText.Date.Date.Ticks + startTimeText.RealTime.Ticks);
                var newEndTime = new DateTime(endDateText.Date.Date.Ticks + endTimeText.RealTime.Ticks);
               
                current.StartTime = null;
                current.StartTime = newStartTime;
                
                if (current.EndTime != null)
                {
                    current.EndTime = null;
                    current.EndTime = newEndTime;
                }
            }
            catch (Exception e) when (e is InvalidOperationException || e is ArgumentOutOfRangeException)
            {
                current.StartTime = oldStart;
                current.EndTime = oldEnd;
                // gets the part of the error message after the initial description of the Exception
                var message = e.Message.Split(':')[1].Trim();
                MessageBox.ErrorQuery("Error", message, "Ok");
                return;
            }
            saveChanges.Text = "Edit";
            startDateText.Enabled = false;
            startTimeText.Enabled = false;
            endDateText.Enabled = false;
            endTimeText.Enabled = false;

            EmployeeDatabase.SaveSingleAndReload(Employee.Id);
            MessageBox.Query("Success", "Information succesfully changed", "Ok");

            ListView.SetSource(_listViewSource);
            // I think this fixes an issue with things not saving properly
            Application.Refresh();

        }
    }
    public virtual void EditTimeUnitView_Leave(FocusEventArgs args)
    {
        // disables inputs if focus leaves the window
        if (saveChanges.Text == "Save Changes")
        {
            saveChanges.Text = "Edit";
            if (startTimeText.AMPMLabel.Text != originalStartTimePeriod)
            {
                startTimeText.AMPMLabel.OnClicked();
            }
            if (endTimeText.AMPMLabel.Text != originalEndTimePeriod)
            {
                endTimeText.AMPMLabel.OnClicked();
            }
            startDateText.Enabled = false;
            startTimeText.Enabled = false;
            endDateText.Enabled = false;
            endTimeText.Enabled = false;

            startDateText.Date = TimeUnit.StartTime.GetValueOrDefault();
            startTimeText.Time = TimeUnit.StartTime.GetValueOrDefault().TimeOfDay;
            endDateText.Date = TimeUnit.EndTime.GetValueOrDefault();
            endTimeText.Time = TimeUnit.EndTime.GetValueOrDefault().TimeOfDay;
        }
    }
    
}

