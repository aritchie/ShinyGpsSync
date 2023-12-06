using SQLite;

namespace GpsSync;


public class MySqliteConnection : SQLite.SQLiteAsyncConnection
{
	public MySqliteConnection() : base(Path.Combine(FileSystem.AppDataDirectory, "test.db"))
	{
		var conn = this.GetConnection();
		conn.CreateTable<JobRun>();
        conn.CreateTable<NetworkEvent>();
        conn.CreateTable<GpsPing>();
    }


	public AsyncTableQuery<JobRun> JobRuns => this.Table<JobRun>();
    public AsyncTableQuery<NetworkEvent> NetworkEvents => this.Table<NetworkEvent>();
    public AsyncTableQuery<GpsPing> GpsPings => this.Table<GpsPing>();
}


public class JobRun
{
	[PrimaryKey]
	[AutoIncrement]
	public int Id { get; set; }
    public bool IsPunchedIn { get; set; } // if this is false, it ran on the platform job
	public DateTimeOffset Timestamp { get; set; }
}

public class NetworkEvent
{
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

	public bool HasInternet { get; set; }
	public string? ConnectionTypes { get; set; }
    public DateTimeOffset Timestamp { get; set; }
}


public class GpsPing
{
    [PrimaryKey]
    [AutoIncrement]
    public int Id { get; set; }

    public double Altitude { get; set; }
    public double Speed { get; set; }
    public double Latitude { get; set; }
    public double Longitude { get; set; }

    public DateTimeOffset Timestamp { get; set; }
}