using Shiny.Notifications;

namespace GpsSync.Delegates;


public class NetworkMonitorService : IShinyStartupTask
{
    readonly MySqliteConnection data;
    readonly Shiny.Net.IConnectivity conn;
    readonly INotificationManager notifications;
    readonly ILogger logger;
    readonly AppSettings settings;


    public NetworkMonitorService(
        Shiny.Net.IConnectivity conn,
        INotificationManager notifications,
        MySqliteConnection data,
        ILogger<NetworkMonitorService> logger,
        AppSettings settings
    )
    {
        this.conn = conn;
        this.notifications = notifications;
        this.logger = logger;
        this.settings = settings;
        this.data = data;
    }


    // this will keep running as long as GPS is going OR the app is in the foreground
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
                        await this.data.InsertAsync(new NetworkEvent
                        {
                            HasInternet = this.conn.IsInternetAvailable(),
                            ConnectionTypes = this.conn.ConnectionTypes.ToString(),
                            Timestamp = DateTimeOffset.UtcNow
                        });
                        if (settings.IsNotificationsEnabled)
                        {
                            var title = connected ? "Online" : "Offline";
                            await this.notifications.Send(title, "GPS Sync Network has changed state");
                        }
                    }   
                },
                ex => this.logger.LogError(ex, "Error with online restore")
            );
    }
}
