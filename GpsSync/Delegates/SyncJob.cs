using Shiny.Jobs;

namespace GpsSync.Delegates;


/// <summary>
/// This is a foreground job - with realtime GPS running on iOS and the foreground
/// service running on Android, this job will keep running every 30 seconds
/// If the app goes full to the background (GPS off), this job will run periodically
/// covering ALL areas of sync
/// </summary>
public class SyncJob : Job
{
    readonly MySqliteConnection conn;

    public SyncJob(ILogger<SyncJob> logger, MySqliteConnection conn) : base(logger)
    {
        this.MinimumTime = TimeSpan.FromMinutes(10);
        this.conn = conn;
    }


    protected override Task Run(CancellationToken cancelToken)
    {
        return this.conn.InsertAsync(new JobRun { Timestamp = DateTimeOffset.UtcNow });
    }
}