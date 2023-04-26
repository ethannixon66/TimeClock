using System;
using System.Text.Json.Serialization;

public class TimeUnit : IComparable<TimeUnit>
{
    private DateTime? _startTime;
    private DateTime? _endTime;

    public DateTime? StartTime
    {
        get => _startTime;
        set
        {
            if (value > EndTime)
            {
                throw new ArgumentOutOfRangeException("End time must be after start time");
            }

            _startTime = value;
        }

    }

    public DateTime? EndTime
    {
        get => _endTime;
        set
        {
            if (value < StartTime)
            {
                throw new ArgumentOutOfRangeException($"End time for time unit must be after start time");
            }
            _endTime = value;

        }
    }

    [JsonIgnore]
    public bool HasStarted => StartTime != null;
    [JsonIgnore]
    public bool HasEnded => EndTime != null;
    [JsonIgnore]
    public bool InProgress => HasStarted && !HasEnded;

    public void Start()
    {
        if (InProgress)
        {
            throw new InvalidOperationException("Time unit has already started");
        }
        if (HasEnded)
        {
            throw new InvalidOperationException("Time unit has already ended");
        }
        StartTime = DateTime.Now;
    }
    public virtual void End()
    {
        if (!InProgress)
        {
            throw new InvalidOperationException("Time unit cannot be ended unless it is in progress");
        }
        EndTime = DateTime.Now;
    }

    [JsonIgnore]
    public TimeSpan TimeElapsed
    {
        get
        {
            if (!StartTime.HasValue)
            {
                throw new InvalidOperationException("This time unit has not started yet");
            }
            if (!EndTime.HasValue)
            {
                throw new InvalidOperationException("This time unit has not ended yet");
            }

            return (EndTime - StartTime).Value;
        }

    }
    public override string ToString()
    {
        var startTime = StartTime.GetValueOrDefault();
        var date = HasStarted ? startTime.ToString("d") : "N/A";
        var time = HasStarted ? startTime.ToString("t") : "N/A";
        return $"Date: {date}  | Time: {time}";
    }

    public int CompareTo(TimeUnit timeUnit)
    {
        // if one is true and the other is false
        if (Convert.ToInt32(StartTime.HasValue) + Convert.ToInt32(timeUnit.StartTime.HasValue) == 1)
        {
            return StartTime.HasValue ? -1 : 1;
        }
        else if (StartTime != timeUnit.StartTime)
        {
            return StartTime.Value.CompareTo(timeUnit.StartTime.Value);
        }
        else if (Convert.ToInt32(EndTime.HasValue) + Convert.ToInt32(timeUnit.EndTime.HasValue) == 1)
        {
            return EndTime.HasValue ? -1 : 1;
        }
        else
        {
            return EndTime.Value.CompareTo(timeUnit.EndTime.Value);
        }
    }
}

