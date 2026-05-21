namespace MauiApp1.Models;

public class Ticket
{
    public int Id { get; init; }
    public string Number { get; init; } = string.Empty;
    public string Title { get; init; } = string.Empty;
    public string Status { get; init; } = string.Empty;
    public string Description { get; init; } = string.Empty;
    public string CustomerName { get; init; } = string.Empty;
}
