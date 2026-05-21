using MauiApp1.Models;

namespace MauiApp1.Pages;

public partial class UsersPage : ContentPage
{

    private List<User> users = new List<User>
    {
        new User { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@example.com" },
        new User { Id = 2, FirstName = "Jane", LastName = "Smith", Email = "jane.smith@example.com" }
    };

    public UsersPage()
    {
        InitializeComponent();

        LoadUsers();
    }

    // Load users
    private void LoadUsers()
    {
        UsersCollectionView.ItemsSource = users;
    }

    private async void OnUserSelected(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is User selectedUser)
        {
            // Navigate to the UserDetails page, passing the selected user's ID as a query parameter
            await Shell.Current.GoToAsync($"user-details?Id={selectedUser.Id}");
        }
    }

}