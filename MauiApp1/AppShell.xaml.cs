namespace MauiApp1;

public partial class AppShell : Shell
{
	public static readonly BindableProperty CurrentTitleProperty =
		BindableProperty.Create(nameof(CurrentTitle), typeof(string), typeof(AppShell), string.Empty);

	public string CurrentTitle
	{
		get => (string)GetValue(CurrentTitleProperty);
		set => SetValue(CurrentTitleProperty, value);
	}

	public AppShell()
	{
		BindingContext = this;
		InitializeComponent();

		// Keep the centre title in sync as the user navigates between tabs/pages.
		Navigated += (_, _) => CurrentTitle = CurrentPage?.Title ?? string.Empty;

		// Detail pages are NOT declared in AppShell.xaml, so they must be
		// registered here before Shell.GoToAsync() can resolve them.
		//
		// Route name must match the string used in GoToAsync() exactly.
		Routing.RegisterRoute("customer-detail", typeof(Pages.CustomerDetailPage));
		Routing.RegisterRoute("ticket-detail", typeof(Pages.TicketDetailPage));

		Routing.RegisterRoute("user-details", typeof(Pages.UserDetails));
	}

	private async void OnAvatarTapped(object sender, TappedEventArgs e)
	{
		var action = await DisplayActionSheetAsync(
			"Account",
			"Cancel",
			null,
			"View Profile",
			"Settings",
			"Log Out");

		switch (action)
		{
			case "View Profile":
				// TODO: navigate to profile page
				await DisplayAlertAsync("Profile", "Profile page coming soon.", "OK");
				break;
			case "Settings":
				// TODO: navigate to settings page
				await DisplayAlertAsync("Settings", "Settings page coming soon.", "OK");
				break;
			case "Log Out":
				bool confirm = await DisplayAlertAsync("Log Out", "Are you sure you want to log out?", "Yes", "Cancel");
				if (confirm)
				{
					// TODO: clear auth state and redirect to login
					await DisplayAlertAsync("Logged Out", "You have been logged out.", "OK");
				}
				break;
		}
	}
}
