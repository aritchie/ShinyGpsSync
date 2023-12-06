namespace GpsSync;


public class NetworkLogViewModel : AbstractLogViewModel<NetworkEvent>
{
	public NetworkLogViewModel(BaseServices services, MySqliteConnection data) : base(services, data)
	{
	}

    protected override async Task LoadData()
    {
        this.Logs = await this.Data
            .NetworkEvents
            .OrderByDescending(x => x.Timestamp)
            .ToListAsync();
    }
}
