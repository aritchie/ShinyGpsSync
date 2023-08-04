namespace GpsSync;


public class AppSettings : ReactiveObject
{
    [Reactive] public bool IsPunchedIn { get; set; }
}