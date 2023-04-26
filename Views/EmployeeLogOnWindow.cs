using System;
using System.Text.RegularExpressions;
using System.Linq;
using Terminal.Gui;

public class EmployeeLogOnWindow : Window
{
	public EmployeeLogOnWindow()
	{
		X = 2;
		Y = 1;
		Width = Dim.Fill(2);
		Height = Dim.Fill(2);
		Border.BorderStyle = BorderStyle.Rounded;
		ColorScheme = Colors.Dialog;
		Title = "Employee Login";

		// Create input components and labels
		var usernameLabel = new Label()
		{
			Text = "Employee ID:",
			X = Pos.Center(),
			Y = Pos.Center() - 2
		};

		var usernameText = new TextField("")
		{
			X = Pos.Right(usernameLabel) + 1,
			Y = usernameLabel.Y,
			// perfectly fit an employee ID length of 6
			Width = 7,
		};
		// forces text input to only accept digits 0-9
		// automatically logs employee in upon entering of the full 6 digit employee ID
		// gives an error if the ID doesn't match an existing ID
		usernameText.TextChanging += (args) =>
		{
			var enteredId = args.NewText.ToString();
			if (!Regex.IsMatch(enteredId, @"^[0-9]*$"))
            {
				usernameText.CursorPosition -= 1;

				args.Cancel = true;
				return;
            }
			else if (args.NewText.Length == 6)
            {
				args.Cancel = true;
				usernameText.Text = "";
				try
                {
					var employee = EmployeeDatabase.Employees.Single(e => e.Id == enteredId);
					Application.Run(new EmployeeOverviewView(employee));
				}
				catch (InvalidOperationException)
                {
					MessageBox.ErrorQuery("Error", "Employee not found", "Ok");
                }
            }
		};
		Add(usernameLabel, usernameText);
	}
}