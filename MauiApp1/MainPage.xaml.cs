namespace MauiApp1;

public partial class MainPage : ContentPage
{
	public MainPage()
	{
		InitializeComponent();
	}

	// Programmatic navigation: jump to the Customers tab by route name.
	// The route "//customers" uses // to navigate to a root shell route
	// (i.e. switch tabs), not push a new page onto a stack.
	private async void OnGoToCustomersClicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//customers");
	}

	// Same idea for Tickets.
	private async void OnGoToTicketsClicked(object? sender, EventArgs e)
	{
		await Shell.Current.GoToAsync("//tickets");
	}
}
