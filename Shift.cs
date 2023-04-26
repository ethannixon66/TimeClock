using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;

public class Shift : TimeUnit
{
    private decimal hourlyWage;

    public Shift(decimal hourlyWage, int numBreaks)
    {
        HourlyWage = hourlyWage;
        if (numBreaks < 0)
        {
            throw new ArgumentOutOfRangeException("Number of breaks must be >= 0");
        }
        Breaks = Enumerable.Range(0, numBreaks).Select(x => new TimeUnit()).ToList();
    }
    public Shift()
    {

    }
    public List<TimeUnit> Breaks { get; set; }
    [JsonIgnore]
    public double HoursWorked => TimeElapsed.TotalHours - LengthOfBreaks;
    [JsonIgnore]
    public double LengthOfBreaks => Breaks.Where(b => b.HasStarted).Select(b => b.TimeElapsed.TotalHours).Sum();
    [JsonIgnore]
    public decimal Payout => decimal.Round((decimal)HoursWorked * HourlyWage, 2);

    public decimal HourlyWage { get => hourlyWage; set => hourlyWage = decimal.Round(value, 2); }

    public override void End()
    {
        if (Breaks.Any(b => b.InProgress))
        {
            throw new InvalidOperationException("Cannot end shift if break is in progress");
        }
        base.End();
    }
    public void StartBreak()
    {
        if (Breaks.Any(b => b.InProgress))
        {
            throw new InvalidOperationException("Cannot start a new break until all others have ended");
        }
        if (Breaks.All(b => b.HasEnded))
        {
            throw new InvalidOperationException("No breaks left to start");
        }
        if (!InProgress)
        {
            throw new InvalidOperationException("Cannot start a new break until shift has begun");
        }
        Breaks.First(b => !b.HasStarted).Start();

    }
    public void EndBreak()
    {
        if (Breaks.All(b => !b.InProgress))
        {
            throw new InvalidOperationException("No Breaks in progress to end");
        }
        Breaks.First(b => b.InProgress).End();

    }

    public void AddBreak()
    {
        Breaks.Add(new TimeUnit());
    }
}




