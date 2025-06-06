using CommunityToolkit.Maui;
using Microsoft.Extensions.Logging;

namespace HeraCrossController
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
                    fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
                });
#if ANDROID
            builder.Services.AddSingleton<Interfaces.IBluetoothSerial, Platforms.Android.BluetoothSerial>();
#elif WINDOWS
            builder.Services.AddSingleton<Interfaces.IBluetoothSerial, Platforms.Windows.BluetoothSerial>();
#endif
            builder.Services.AddSingleton<Interfaces.IDialogService,Services.DialogService>();

            builder.Services.AddTransient<ViewModels.MainPageViewModel>();
            builder.Services.AddTransient<Views.MainPageView>();

#if DEBUG
            builder.Logging.AddDebug();
#endif

            return builder.Build();
        }
    }
}
