using Shiny.Locations;

namespace GpsSync.Delegates;


public partial class MyGpsDelegate : GpsDelegate
{
    readonly ILogger logger;


    public MyGpsDelegate(ILogger<MyGpsDelegate> logger) : base(logger)
    {
        this.logger = logger;
    }


    protected override Task OnGpsReading(GpsReading reading)
    {
        this.logger.LogInformation($"GPS: {reading.Position.Latitude}/{reading.Position.Longitude}");
        return Task.CompletedTask;
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

