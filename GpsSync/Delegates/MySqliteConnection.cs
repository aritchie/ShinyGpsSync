using SQLite;

namespace GpsSync.Delegates;


public class MySqliteConnection : SQLite.SQLiteAsyncConnection
{
	public MySqliteConnection() : base(Path.Combine(FileSystem.AppDataDirectory, "test.db"))
	{
		this.GetConnection().CreateTable<JobRun>();
	}


	public AsyncTableQuery<JobRun> JobRuns => this.Table<JobRun>();
}


public class JobRun
{
	[PrimaryKey]
	[AutoIncrement]
	public int Id { get; set; }

	public DateTimeOffset Timestamp { get; set; }
}