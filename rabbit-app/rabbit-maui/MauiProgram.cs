using Microsoft.Extensions.Logging;
using rabbit_maui.Core.Services;
using System.Globalization;
using CommunityToolkit.Maui;


namespace rabbit_maui
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .UseMauiCommunityToolkit()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                });

            builder.Services.AddMauiBlazorWebView();

#if DEBUG
    		builder.Services.AddBlazorWebViewDeveloperTools();
    		builder.Logging.AddDebug();
#endif
            // Register app state
            builder.Services.AddSingleton<RallyState>();
            // Persistence service (JSON-based)
            builder.Services.AddSingleton<IRallyStore, JsonRallyStore>();

            // Ensure decimal parsing uses '.' (invariant) across the app
            CultureInfo.DefaultThreadCurrentCulture = CultureInfo.InvariantCulture;
            CultureInfo.DefaultThreadCurrentUICulture = CultureInfo.InvariantCulture;

            // XLSX exporter (OpenXML implementation)
            builder.Services.AddSingleton<IExcelExporter, OpenXmlExcelExporter>();



            return builder.Build();
        }
    }
}
