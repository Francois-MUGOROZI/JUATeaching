namespace MauiApp1.Models;

/// <summary>
/// Maps the JSON response from GET /api/customers and GET /api/customers/{id}.
/// Field names match the CRMDemo domain entity so System.Text.Json deserialises
/// them without any custom converter.
/// </summary>
public class Customer
{
    public int Id { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string LastName { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PhoneNumber { get; set; } = string.Empty;

    // Serialised as an integer: 0 = Active, 1 = Inactive, 2 = Suspended
    public int Status { get; set; }

    public DateTime CreatedAt { get; set; }

    // ── Display helpers ──────────────────────────────────────────────────────

    public string FullName => $"{FirstName} {LastName}".Trim();

    public string Initials =>
        $"{(FirstName.Length > 0 ? FirstName[0] : ' ')}{(LastName.Length > 0 ? LastName[0] : ' ')}"
        .Trim().ToUpper();

    public string StatusLabel => Status switch
    {
        1 => "Inactive",
        2 => "Suspended",
        _ => "Active"
    };

    // Microsoft.Maui.Graphics.Color is available globally in MAUI projects
    public Color StatusColor => Status switch
    {
        1 => Color.FromArgb("#FF9500"),
        2 => Color.FromArgb("#FF3B30"),
        _ => Color.FromArgb("#34C759")
    };
}
