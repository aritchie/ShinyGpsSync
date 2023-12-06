namespace GpsSync;


public class JobLogViewModel : AbstractLogViewModel<JobRun>
{
	public JobLogViewModel(BaseServices services, MySqliteConnection data) : base(services, data)
	{
	}


    protected override async Task LoadData()
    {
        this.Logs = await this.Data
            .JobRuns
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }
}