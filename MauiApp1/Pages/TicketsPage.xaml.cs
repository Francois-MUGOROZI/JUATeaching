using MauiApp1.Models;

namespace MauiApp1.Pages;

public partial class TicketsPage : ContentPage
{
    private static readonly List<Ticket> Tickets =
    [
        new() { Id = 1, Number = "#1001", Title = "Login issue",                Status = "Open",        CustomerName = "John Doe"    },
        new() { Id = 2, Number = "#1002", Title = "Password reset not working", Status = "In Progress", CustomerName = "Jane Smith"  },
        new() { Id = 3, Number = "#1003", Title = "Invoice not received",       Status = "Closed",      CustomerName = "Bob Johnson" },
    ];

    public TicketsPage()
    {
        InitializeComponent();
        TicketsCollection.ItemsSource = Tickets;
    }

    private async void OnTicketSelected(object? sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not Ticket selected) return;

        if (sender is CollectionView list) list.SelectedItem = null;

        await Shell.Current.GoToAsync($"ticket-detail?TicketId={selected.Id}");
    }
}
