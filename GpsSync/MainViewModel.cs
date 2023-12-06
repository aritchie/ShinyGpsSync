using Shiny.Locations;
using Shiny.Notifications;

namespace GpsSync;

public class MainViewModel : ViewModel
{
    public MainViewModel(
        BaseServices services,
        AppSettings settings,
        IGpsManager gpsManager,
        INotificationManager notifications
    ) : base(services)
    {
        this.IsPunchedIn = settings.IsPunchedIn;
        this.IsNotificationsEnabled = settings.IsNotificationsEnabled;

        this.WhenAnyValue(x => x.IsNotificationsEnabled)
            .Skip(1)
            .Subscribe(x => settings.IsNotificationsEnabled = x);

        this.PunchIn = ReactiveCommand.CreateFromTask(
            async () =>
            {
                var access = await gpsManager.RequestAccess(GpsRequest.Realtime(true));
                if (access != AccessState.Available)
                {
                    await this.Dialogs.Alert("Insufficient GPS Permissions - " + access);
                    return;
                }
                
                access = await notifications.RequestAccess();
                if (access != AccessState.Available)
                {
                    await this.Dialogs.Alert("Insufficient Notification Permissions - " + access);
                    return;
                }

                await gpsManager.StopListener();
                await gpsManager.StartListener(GpsRequest.Realtime(true));
                settings.IsPunchedIn = true; // I could just use gps flag?
                this.IsPunchedIn = true;
            },
            this.WhenAny(
                x => x.IsPunchedIn,
                x => !x.GetValue()
            )
        );
        this.PunchOut = ReactiveCommand.CreateFromTask(
            async () =>
            {
                settings.IsPunchedIn = false;
                this.IsPunchedIn = false;
                await gpsManager.StopListener();                
            },
            this.WhenAny(
                x => x.IsPunchedIn,
                x => x.GetValue()
            )
        );
    }



    [Reactive] public bool IsPunchedIn { get; set; }
    [Reactive] public bool IsNotificationsEnabled { get; set; }
    public ICommand PunchIn { get; }
    public ICommand PunchOut { get; }
}