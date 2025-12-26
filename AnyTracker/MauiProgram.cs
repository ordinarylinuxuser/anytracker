using AnyTracker.Services;
using AnyTracker.ViewModels;
using Microsoft.Extensions.Logging;

namespace AnyTracker;

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
            });

#if DEBUG
        builder.Logging.AddDebug();
#endif
// Register Platform Specific Service
#if ANDROID
        builder.Services
            .AddSingleton<INotificationService, AndroidNotificationService>();
#else
    // Dummy implementation for iOS/Windows to prevent crashes
        builder.Services.AddSingleton<INotificationService>(new MockNotificationService());
#endif
        builder.Services.AddSingleton<TrackingViewModel>();
        builder.Services.AddSingleton<MainPage>();
        return builder.Build();
    }
}