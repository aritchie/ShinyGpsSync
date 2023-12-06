namespace GpsSync;


public abstract class AbstractLogViewModel<T> : ViewModel where T : class, new()
{
	protected AbstractLogViewModel(BaseServices services, MySqliteConnection data) : base(services)
	{
		this.Data = data;

		this.Load = ReactiveCommand.CreateFromTask(this.LoadData);
		this.BindBusyCommand(this.Load);

		this.Clear = ReactiveCommand.CreateFromTask(this.ClearData);
	}


    public override void OnAppearing()
    {
        base.OnAppearing();
        this.Load.Execute(null);
    }


    protected MySqliteConnection Data { get; }
	public ICommand Load { get; }
	public ICommand Clear { get; }
	[Reactive] public List<T>? Logs { get; protected set; }

	protected virtual async Task LoadData()
	{
		this.Logs = await this.Data.Table<T>().ToListAsync();
	}


	protected virtual async Task ClearData()
	{
		var confirm = await this.Dialogs.Confirm("Delete all data?", "Confirm");
		if (confirm)
		{
			await this.Data.DeleteAllAsync<T>();
			this.Load.Execute(null);
		}
	}
}

