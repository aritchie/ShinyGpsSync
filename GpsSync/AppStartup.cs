using Shiny.Notifications;

namespace GpsSync;


public class AppStartup : IShinyStartupTask
{
    readonly Shiny.Net.IConnectivity conn;
    readonly INotificationManager notifications;
    readonly ILogger logger;
    readonly AppSettings settings;


    public AppStartup(
        Shiny.Net.IConnectivity conn,
        INotificationManager notifications,
        ILogger<AppStartup> logger,
        AppSettings settings
    )
    {
        this.conn = conn;
        this.notifications = notifications;
        this.logger = logger;
        this.settings = settings;
    }


    public void Start()
    {
        // this is not needed WITH the job - but it is a good thought experiment
        this.conn
            .WhenInternetStatusChanged()
            .Skip(1) // skip initial ping
            .DistinctUntilChanged()
            .SubscribeAsync(
                async connected =>
                {
                    this.logger.LogInformation("Connected: " + connected);
                    if (settings.IsPunchedIn)
                    {
                        var title = connected ? "Online" : "Offline";
                        await this.notifications.Send(title, "GPS Sync has changed state");
                    }   
                },
                ex => this.logger.LogError(ex, "Error with online restore")
            );
    }
}
