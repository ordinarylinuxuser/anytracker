#region

using System.Timers;
using _Microsoft.Android.Resource.Designer;
using Android.App;
using Android.Content;
using Android.Content.PM;
using Android.OS;
using AndroidX.Core.App;
using OpenTracker.Utilities;
using Timer = System.Timers.Timer;

#endregion

namespace OpenTracker;

// The Attribute below automatically adds the <service> tag to the AndroidManifest.xml

[Service(Exported = false, ForegroundServiceType = ForegroundService.TypeDataSync)]
public class TickerService : Service
{
    private Timer _timer;
    private DateTime _startTime;
    private string _trackerTitle;
    private string _currentStage;
    private string _displayFormat;

    private const string ChannelId = "open_tracker_channel";
    private const int NotificationId = 1001;

    public override IBinder OnBind(Intent intent)
    {
        return null;
    }

    public override StartCommandResult OnStartCommand(Intent intent, StartCommandFlags flags, int startId)
    {
        if (intent?.Action == "STOP_SERVICE")
        {
            StopForeground(StopForegroundFlags.Remove);
            StopSelf();
            return StartCommandResult.NotSticky;
        }

        if (intent != null)
        {
            var ticks = intent.GetLongExtra("StartTime", DateTime.Now.Ticks);
            _startTime = new DateTime(ticks);
            _trackerTitle = intent.GetStringExtra("TrackerName") ?? "OpenTracker";
            _currentStage = intent.GetStringExtra("StageName") ?? "Tracking...";
            _displayFormat = intent.GetStringExtra("DisplayFormat") ?? "Time";

            if (intent.HasExtra("UpdateStageOnly"))
            {
                _currentStage = intent.GetStringExtra("StageName");
                UpdateNotification();
                return StartCommandResult.Sticky;
            }
        }

        CreateNotificationChannel();

        // Build the initial notification
        var notification = BuildNotification();

        // Start Foreground Service
        // For Android 14+ (API 34), we explicitly state the type if compiling against latest SDK
        if (Build.VERSION.SdkInt >= BuildVersionCodes.UpsideDownCake)
            StartForeground(NotificationId, notification,
                ForegroundService.TypeDataSync);
        else
            StartForeground(NotificationId, notification);

        if (_timer == null)
        {
            _timer = new Timer(1000);
            _timer.Elapsed += OnTimerTick;
            _timer.Start();
        }

        return StartCommandResult.Sticky;
    }

    private void OnTimerTick(object sender, ElapsedEventArgs e)
    {
        UpdateNotification();
    }

    private void UpdateNotification()
    {
        var notification = BuildNotification();
        var manager = GetSystemService(NotificationService) as NotificationManager;
        manager?.Notify(NotificationId, notification);
    }

    private Notification BuildNotification()
    {
        var elapsed = DateTime.Now - _startTime;
        var timeString = FormatHelper.FormatTime(elapsed, _displayFormat);

        // Intent to open app when tapped
        var intent = PackageManager?.GetLaunchIntentForPackage(PackageName);
        var pendingIntent = PendingIntent.GetActivity(this, 0, intent, PendingIntentFlags.Immutable);

        var builder = new NotificationCompat.Builder(this, ChannelId)
            .SetContentTitle(_trackerTitle)
            .SetContentText($"{_currentStage}  {timeString}")
            .SetSmallIcon(ResourceConstant.Drawable.ic_stat_tracker) // Ensure this icon exists
            .SetContentIntent(pendingIntent)
            .SetOnlyAlertOnce(true)
            .SetOngoing(true);

        return builder.Build();
    }

    public override void OnDestroy()
    {
        _timer?.Stop();
        _timer?.Dispose();
        base.OnDestroy();
    }

    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.O) return;

        var manager = GetSystemService(NotificationService) as NotificationManager;
        if (manager == null) return;

        var channel = new NotificationChannel(ChannelId, "Open Tracker Progress", NotificationImportance.Low)
        {
            Description = "Shows ongoing tracking progress"
        };
        manager.CreateNotificationChannel(channel);
    }
}