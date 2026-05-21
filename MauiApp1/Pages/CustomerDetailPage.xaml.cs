using MauiApp1.Helpers;
using MauiApp1.Models;
using MauiApp1.Services;

namespace MauiApp1.Pages;

// [QueryProperty] wires the Shell URL query param ?CustomerId=N to the CustomerId property.
[QueryProperty(nameof(CustomerId), "CustomerId")]
public partial class CustomerDetailPage : ContentPage
{
    private readonly ApiClient _apiClient;

    public CustomerDetailPage()
    {
        _apiClient = ServiceHelper.GetService<ApiClient>();
        InitializeComponent();
    }

    // Shell sets this before OnAppearing, so OnAppearing can safely read it.
    public int CustomerId { get; set; }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        if (CustomerId > 0)
            await LoadCustomerAsync(CustomerId);
    }

    private async Task LoadCustomerAsync(int id)
    {
        LoadingIndicator.IsVisible = true;
        LoadingIndicator.IsRunning = true;
        ContentArea.IsVisible = false;

        try
        {
            var customer = await _apiClient.GetAsync<Customer>($"/api/customers/{id}");

            if (customer is null)
            {
                await DisplayAlertAsync("Not Found", "This customer could not be found.", "OK");
                await Shell.Current.GoToAsync("..");
                return;
            }

            PopulateView(customer);
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", $"Could not load customer: {ex.Message}", "OK");
        }
        finally
        {
            LoadingIndicator.IsVisible = false;
            LoadingIndicator.IsRunning = false;
            ContentArea.IsVisible = true;
        }
    }

    private void PopulateView(Customer customer)
    {
        Title = customer.FullName;
        InitialsLabel.Text = customer.Initials;
        NameLabel.Text = customer.FullName;
        EmailLabel.Text = customer.Email;
        PhoneLabel.Text = customer.PhoneNumber;
        CreatedAtLabel.Text = customer.CreatedAt.ToString("MMMM d, yyyy");
        StatusLabel.Text = customer.StatusLabel;
        StatusBadge.BackgroundColor = customer.StatusColor;
    }
}
