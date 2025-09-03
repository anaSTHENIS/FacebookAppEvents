using FacebookAppEvents.src.Plugin.Maui.FacebookAppEvents.Extensions;
using Microsoft.Extensions.Logging;

namespace AppEventApp
{
    public static class MauiProgram
    {
        public static MauiApp CreateMauiApp()
        {
            var builder = MauiApp.CreateBuilder();
            builder
                .UseMauiApp<App>()
                .ConfigureFonts(fonts =>
                {
                    fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                })
                .UseFacebookEvents("YOUR_FACEBOOK_APP_ID", "YOUR_FACEBOOK_CLIENT_TOKEN");

#if DEBUG
    		builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
