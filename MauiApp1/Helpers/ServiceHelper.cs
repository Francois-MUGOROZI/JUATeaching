namespace MauiApp1.Helpers;

/// <summary>
/// Convenience wrapper to resolve services from the MAUI DI container.
/// Useful for pages created by Shell (ContentTemplate) which bypass
/// the normal DI constructor injection path.
/// </summary>
public static class ServiceHelper
{
    public static TService GetService<TService>() where TService : notnull =>
        IPlatformApplication.Current!.Services.GetRequiredService<TService>();
}
