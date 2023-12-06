using GpsSync.Delegates;

namespace GpsSync;


public static class MauiProgram
{
    public static MauiApp CreateMauiApp() => MauiApp
        .CreateBuilder()
        .UseMauiApp<App>()
        .UseMauiCommunityToolkit()
        .UseShinyFramework(
            new DryIocContainerExtension(),
            prism => prism.OnAppStart("NavigationPage/MainPage"),
            new(
                #if DEBUG
                ErrorAlertType.FullError
                #else
                ErrorAlertType.NoLocalize
                #endif
            )
        )
        .ConfigureFonts(fonts =>
        {
            fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
            fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
        })
        .RegisterInfrastructure()
        .RegisterViews()
        .Build();


    static MauiAppBuilder RegisterInfrastructure(this MauiAppBuilder builder)
    {

        builder.Logging.AddSqlite(Path.Combine(FileSystem.AppDataDirectory, "logging.db"));
#if DEBUG
        builder.Logging.SetMinimumLevel(LogLevel.Trace);
        builder.Logging.AddDebug();
#endif
        var s = builder.Services;
        s.AddSingleton<MySqliteConnection>();
        s.AddShinyService<AppStartup>();
        s.AddShinyService<AppSettings>();

        s.AddJob(
            typeof(GpsSync.Delegates.SyncJob),
            requiredNetwork: Shiny.Jobs.InternetAccess.Any,
            runInForeground: true
        );
        s.AddGps<GpsSync.Delegates.MyGpsDelegate>();
        s.AddNotifications();
        return builder;
    }


    static MauiAppBuilder RegisterViews(this MauiAppBuilder builder)
    {
        var s = builder.Services;

        s.RegisterForNavigation<MainPage, MainViewModel>();
        s.RegisterForNavigation<LogPage, LogViewModel>();
        return builder;
    }
}
