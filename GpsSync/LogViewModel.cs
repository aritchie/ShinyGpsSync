using GpsSync.Delegates;

namespace GpsSync;


public class LogViewModel : ViewModel
{
	public LogViewModel(BaseServices services, MySqliteConnection conn) : base(services)
	{
        this.Load = ReactiveCommand.Create(async () =>
        {
            this.Logs = await conn
                .JobRuns
                .OrderByDescending(x => x.Timestamp)
                .ToListAsync();
        });
        this.BindBusyCommand(this.Load);
	}


    public ICommand Load { get; }
    [Reactive] public List<JobRun> Logs { get; private set; }

    public override void OnAppearing()
    {
        base.OnAppearing();
        this.Load.Execute(null);
    }
}