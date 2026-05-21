namespace MauiApp1.Pages;

[QueryProperty(nameof(Id), "Id")]
public partial class UserDetails : ContentPage
{
    public UserDetails()
    {
        InitializeComponent();
    }

    public int Id
    {
        set => LoadUser(value);
    }

    private void LoadUser(int id)
    {
        TitleLabel.Text = $"User Details for User #{id}";
    }

}