using Terminal.Gui;

public class CreateAdminAccountWizard : Wizard
{
    public CreateAdminAccountWizard()
    {
		Modal = false;
		Title = "Admin Account Creation";
		Width = Height = Dim.Percent(50);
		var firstStep = new WizardStep("Welcome!")
		{
			Text = "Welcome to the TimeClock application! If you're seeing this message" +
			" either this is your first time running the program or your previous account was lost. " +
            "Press the next button to continue to account creation"
        };

		AddStep(firstStep);

		var secondStep = new WizardStep("Account Info");

		var usernameLabel = new Label()
		{
			Text = "Username:"
		};

		var usernameText = new TextField("")
		{
			X = Pos.Right(usernameLabel) + 1,

			Width = Dim.Fill(),
		};

		var passwordLabel = new Label()
		{
			Text = "Password:",
			X = Pos.Left(usernameLabel),
			Y = Pos.Bottom(usernameLabel) + 1
		};

		var passwordText = new TextField("")
		{
			Secret = true,
			X = Pos.Left(usernameText),
			Y = Pos.Top(passwordLabel),
			Width = Dim.Fill(),
		};

		var confirmPasswordLabel = new Label()
		{
			Text = "Confirm\nPassword:",
			X = Pos.Left(passwordLabel),
			Y = Pos.Bottom(passwordLabel) + 1
		};

		var confirmPasswordText = new TextField("")
		{
			Secret = true,
			X = Pos.Right(confirmPasswordLabel) + 1,
			Y = Pos.Top(confirmPasswordLabel) + 1,
			Width = Dim.Fill(),
		};

		secondStep.Add(usernameLabel, passwordLabel, usernameText, passwordText,
			confirmPasswordLabel, confirmPasswordText);
		
		AddStep(secondStep);
		
	
		var thirdStep = new WizardStep("Congratulations!")
		{
			Text = "Congratulations, you have finished making your account!",
		};
		AddStep(thirdStep);
		NextFinishButton.Clicked += () =>
		{
			if (CurrentStep == thirdStep)
			{
				if (passwordText.Text == "" || usernameText.Text == "")
				{
					CurrentStep = secondStep;
					MessageBox.ErrorQuery("Error", "All fields are required", "Ok");
					return;
				}
				if (passwordText.Text != confirmPasswordText.Text)
				{
					CurrentStep = secondStep;
					MessageBox.ErrorQuery("Error", "Passwords don't match!", "Ok");
					return;

				}
				if (passwordText.Text.Length < 8)
				{
					CurrentStep = secondStep;
					MessageBox.ErrorQuery("Error", "Password must be at least 8 characters", "Ok");
					return;
				}
			}


		};
		Finished += (args) => { 
			AdminAccountManager.Account = new AdminAccount()
			{
				Username = usernameText.Text.ToString(),
				Password = passwordText.Text.ToString(),
			};
			AdminAccountManager.SaveAccount();
			Application.RequestStop();
		};
	}
}

