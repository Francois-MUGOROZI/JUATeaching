using MauiApp1.Helpers;
using MauiApp1.Models;
using MauiApp1.Services;

namespace MauiApp1.Pages;

public partial class CustomersPage : ContentPage
{
    private readonly ApiClient _apiClient;

    public CustomersPage()
    {
        // ServiceHelper resolves from the MAUI DI container even when the page
        // is instantiated by Shell's ContentTemplate (which bypasses constructor DI).
        _apiClient = ServiceHelper.GetService<ApiClient>();
        InitializeComponent();
    }

    // Called every time the page becomes visible (tab switch, back navigation, etc.).
    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Only show the full-screen spinner on the very first load.
        if (CustomersCollection.ItemsSource is null)
            await LoadCustomersAsync(showSpinner: true);
    }

    // Pull-to-refresh handler wired in XAML (Refreshing="OnRefreshing").
    private async void OnRefreshing(object? sender, EventArgs e)
    {
        await LoadCustomersAsync(showSpinner: false);
    }

    private async Task LoadCustomersAsync(bool showSpinner)
    {
        if (showSpinner)
        {
            LoadingIndicator.IsVisible = true;
            LoadingIndicator.IsRunning = true;
        }

        try
        {
            var customers = await _apiClient.GetListAsync<Customer>("/api/customers");
            CustomersCollection.ItemsSource = customers;
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Could not load customers: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            Refresher.IsRefreshing = false;
        }
    }

    private async void OnCustomerSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Customer selected) return;

        // Reset selection so the same row can be tapped again after returning.
        if (sender is CollectionView list) list.SelectedItem = null;

        await Shell.Current.GoToAsync($"customer-detail?CustomerId={selected.Id}");
    }
}
