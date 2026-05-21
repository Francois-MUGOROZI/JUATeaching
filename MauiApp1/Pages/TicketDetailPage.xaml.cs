using MauiApp1.Models;

namespace MauiApp1.Pages;

[QueryProperty(nameof(TicketId), "TicketId")]
public partial class TicketDetailPage : ContentPage
{
    private static readonly List<Ticket> Tickets =
    [
        new() { Id = 1, Number = "#1001", Title = "Login issue",                Status = "Open",        CustomerName = "John Doe",    Description = "User cannot log in after password change. Reports incorrect password error despite using the new credentials." },
        new() { Id = 2, Number = "#1002", Title = "Password reset not working", Status = "In Progress", CustomerName = "Jane Smith",  Description = "Password reset email is not being received. Checked spam folder, nothing there." },
        new() { Id = 3, Number = "#1003", Title = "Invoice not received",       Status = "Closed",      CustomerName = "Bob Johnson", Description = "Customer did not receive invoice for March. Resent manually and confirmed delivery." },
    ];

    public int TicketId
    {
        set => LoadTicket(value);
    }

    public TicketDetailPage()
    {
        InitializeComponent();
    }

    private void LoadTicket(int id)
    {
        var ticket = Tickets.FirstOrDefault(t => t.Id == id);
        if (ticket is null) return;

        Title = ticket.Title;
        NumberLabel.Text = ticket.Number;
        TitleLabel.Text = ticket.Title;
        StatusLabel.Text = ticket.Status;
        CustomerNameLabel.Text = ticket.CustomerName;
        DescriptionLabel.Text = ticket.Description;

        // Colour the status badge based on value
        (StatusFrame.BackgroundColor, StatusLabel.TextColor) = ticket.Status switch
        {
            "Open" => (Color.FromArgb("#FFF3E0"), Color.FromArgb("#E65100")),
            "In Progress" => (Color.FromArgb("#E3F2FD"), Color.FromArgb("#1565C0")),
            "Closed" => (Color.FromArgb("#E8F5E9"), Color.FromArgb("#2E7D32")),
            _ => (Color.FromArgb("#F5F5F5"), Color.FromArgb("#616161")),
        };
    }
}
