using System;
using Terminal.Gui;
enum TimePeriod
{
    AM,
    PM
}
// Custom input field allowing 12 hour time input
public class TwelveHourTimeField : TimeField
{
    public Button AMPMLabel { get; set; }
    private TimePeriod _timePeriod;
    public TimeSpan RealTime
    {
        get
        {
            if (_timePeriod == TimePeriod.AM)
            {
                if (Time.Hours == 12)
                {
                    return TimeSpan.FromMinutes(Time.Minutes);
                }
                return Time;
            }
            else
            {
                if (Time.Hours == 12)
                {
                    return Time;
                }
                return Time + new TimeSpan(12, 0, 0);
            }
        }
    }
    public TwelveHourTimeField(TimeSpan time)
    {
        _timePeriod = IsAM(time) ? TimePeriod.AM : TimePeriod.PM;
        Time = IsAM(time) ? time : time - new TimeSpan(12, 0, 0);
        //Takes care of edge case with noon and midnight
        if (Time.Hours == 0)
        {
            Time += new TimeSpan(12, 0, 0);
        }
        TimeChanged += (args) =>
        {
            if (args.NewValue > new TimeSpan(12, 59, 59))
            {
                if (CursorPosition == 0)
                {
                    Time = args.OldValue;
                }
                else
                {
                    Time = args.NewValue - TimeSpan.FromHours(10);
                }
                CursorPosition -= 1;
            }
            if (args.NewValue < new TimeSpan(1, 0, 0))
            {
                Time = args.OldValue;
                CursorPosition -= 1;
            }
        };
        
        Added += (args) =>
        {
            AMPMLabel = new Button(_timePeriod == TimePeriod.AM ? "AM" : "PM")
            {
                X = Pos.Right(this) + 1,
                Y = this.Y,
            };

            AMPMLabel.Clicked += () =>
            {
                AMPMLabel.Text = AMPMLabel.Text.ToString() == "AM" ? "PM" : "AM";
                _timePeriod = _timePeriod == TimePeriod.AM ? TimePeriod.PM : TimePeriod.AM;
            };
            EnabledChanged += () =>
            {
                AMPMLabel.Enabled = Enabled;
            };
            SuperView.Add(AMPMLabel);
            OnEnabledChanged();
        };
        
    }
    private bool IsAM(TimeSpan time)
    {
        return time < new TimeSpan(12, 0, 0);
    }
}

