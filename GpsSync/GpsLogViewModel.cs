namespace GpsSync;


public class GpsLogViewModel : AbstractLogViewModel<GpsPing>
{
	public GpsLogViewModel(BaseServices services, MySqliteConnection conn) : base(services, conn)
	{
	}

    protected override async Task LoadData()
    {
        this.Logs = await this.Data
            .GpsPings
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }
}