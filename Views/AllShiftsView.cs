using Terminal.Gui;

public class AllShiftsView : Dialog
{
    public AllShiftsView(Employee employee)
    {
        X = 3;
        Y = 3;
        Width = Dim.Fill(3);
        Height = Dim.Fill(3);
        Border.BorderStyle = BorderStyle.None;
        var frame = new FrameView()
        {
            Title = "Shifts",
            Height = Dim.Fill(),
            Width = Dim.Percent(33.33333f),
            X = 0,
            Y = 0,
            CanFocus = true,
        };
        employee.AllShifts.Sort();
        var shiftsList = new ListView(employee.AllShifts)
        {
            X = 1,
            Y = 1,
            Height = Dim.Fill(),
            Width = Dim.Fill(1),
            AllowsMarking = false,
            AllowsMultipleSelection = false,
            CanFocus = true,
            
        };
        frame.Add(shiftsList);

        var currentShift = employee.AllShifts[shiftsList.SelectedItem];
        var frame2 = new EditShiftView(shiftsList, currentShift, employee, employee.AllShifts)
        {
            Title = "Edit Shift",
            Height = Dim.Percent(50f),
            Width = Dim.Percent(33.33333f),
            X = Pos.Right(frame),
            Y = 0,
            CanFocus = true,
        };
        


        var breaksList = new ListView(currentShift.Breaks)
        {
            X = 1,
            Y = 1,
            Height = Dim.Fill(),
            Width = Dim.Fill(1),
            AllowsMarking = false,
            AllowsMultipleSelection = false,
            CanFocus = true,
        };

        var frame3 = new FrameView()
        {
            Title = "Breaks",
            Height = Dim.Fill(),
            Width = frame2.Width,
            X = frame2.X,
            Y = Pos.Bottom(frame2),
            CanFocus = true,
        };
        frame3.Add(breaksList);
        var currentBreak = currentShift.Breaks[breaksList.SelectedItem];
        var frame4 = new EditTimeUnitView(breaksList, currentBreak, employee, currentShift.Breaks)
        {
            Title = "Edit Break",
            Height = Dim.Fill(),
            Width = Dim.Fill(1),
            X = Pos.Right(frame3),
            Y = 0,
            CanFocus = true,
        };
        shiftsList.SelectedItemChanged += (args) =>
        {
            var shift = employee.AllShifts[args.Item];
            frame2.Employee = employee;
            frame2.Shift = shift;
            frame2.Init();
            breaksList.SetSource(shift.Breaks);
            breaksList.OnSelectedChanged();
        };
        breaksList.SelectedItemChanged += (args) =>
        {
            currentShift = employee.AllShifts[shiftsList.SelectedItem];
            currentBreak = currentShift.Breaks[breaksList.SelectedItem];
            frame4.TimeUnit = currentBreak;
            frame4.Init();
        };
        Add(frame, frame2, frame3, frame4);

    }
}

