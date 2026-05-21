using MauiApp1.Services;
using Microsoft.Extensions.Logging;

namespace MauiApp1;

public static class MauiProgram
{
	// ── API settings ─────────────────────────────────────────────────────────
	// BaseUrl: matches ApiSettings:BaseUrl in CRMDemo/Web/appsettings.Development.json
	// ApiKey:  matches ApiSettings:ApiKey  in CRMDemo/API/appsettings.Development.json
	private const string ApiBaseUrl = "http://localhost:5246";
	private const string ApiKey = "";

	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		// Typed HttpClient — same pattern as CRMDemo/Web Program.cs.
		// Pages resolve ApiClient via ServiceHelper.GetService<ApiClient>() in their
		// constructors, so the pages themselves do NOT need to be registered here.
		// Shell creates pages via their parameterless constructor (ContentTemplate /
		// Routing.RegisterRoute + GoToAsync) and never touches the DI container for them.
		builder.Services.AddHttpClient<ApiClient>(client =>
		{
			client.BaseAddress = new Uri(ApiBaseUrl);
			client.DefaultRequestHeaders.Add("X-Api-Key", ApiKey);
		});

#if DEBUG
		builder.Logging.AddDebug();
#endif

		return builder.Build();
	}
}
