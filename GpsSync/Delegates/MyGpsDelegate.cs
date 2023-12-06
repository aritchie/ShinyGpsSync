using Shiny.Locations;

namespace GpsSync.Delegates;


public partial class MyGpsDelegate : GpsDelegate
{
    readonly ILogger logger;
    readonly MySqliteConnection conn;


    public MyGpsDelegate(ILogger<MyGpsDelegate> logger, MySqliteConnection conn) : base(logger)
    {
        this.logger = logger;
        this.conn = conn;

        this.MinimumDistance = Distance.FromMeters(50);
        this.MinimumTime = TimeSpan.FromMinutes(10);
    }


    protected override Task OnGpsReading(GpsReading reading)
    {
        this.logger.LogInformation($"GPS: {reading.Position.Latitude}/{reading.Position.Longitude}");
        return this.conn.InsertAsync(new GpsPing
        {
            Latitude = reading.Position.Latitude,
            Longitude = reading.Position.Longitude,
            Speed = reading.Speed,
            Timestamp = DateTimeOffset.UtcNow
        });
    }
}

#if ANDROID
public partial class MyGpsDelegate : IAndroidForegroundServiceDelegate
{
    public void Configure(AndroidX.Core.App.NotificationCompat.Builder builder)
    {
        
    }
}
#endif

