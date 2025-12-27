#region

using System.Diagnostics.CodeAnalysis;
using Android.App;
using Android.Content;
using Android.OS;
using AndroidX.Core.App;
using OpenTracker.Services;
using Application = Android.App.Application;

#endregion

namespace OpenTracker;

public class AndroidNotificationService : INotificationService
{
    private const string ChannelId = "open_tracker_channel";
    private const int NotificationId = 1001;
    private readonly NotificationManager _notificationManager;

    public AndroidNotificationService()
    {
        var context = Application.Context;
        _notificationManager = context.GetSystemService(Context.NotificationService) as NotificationManager ??
                               throw new NullReferenceException("NotificationManager");
        CreateNotificationChannel();
    }

    public void ShowStickyNotification(string title, string message)
    {
        var context = Application.Context;

        // Intent to open app when clicked
        var intent = context.PackageManager?.GetLaunchIntentForPackage(context.PackageName) ??
                     throw new NullReferenceException("Launch intent not found");
        var pendingIntent = PendingIntent.GetActivity(context, 0, intent, PendingIntentFlags.Immutable);

        var builder = new NotificationCompat.Builder(context, ChannelId)
            .SetContentTitle(title)
            .SetContentText(message)
            .SetSmallIcon(Android.Resource.Drawable.IcMenuRecentHistory) // Default android icon
            .SetOngoing(true) // This makes it "Sticky"
            .SetContentIntent(pendingIntent)
            .SetOnlyAlertOnce(true);

        _notificationManager.Notify(NotificationId, builder.Build());
    }

    public void CancelNotification()
    {
        _notificationManager.Cancel(NotificationId);
    }

    [SuppressMessage("Interoperability", "CA1416:Validate platform compatibility")]
    private void CreateNotificationChannel()
    {
        if (Build.VERSION.SdkInt < BuildVersionCodes.O) return;

        var channel = new NotificationChannel(ChannelId, "Open Tracker Progress", NotificationImportance.Low)
        {
            Description = "Shows ongoing tracking progress"
        };
        // Importance Low prevents the sound/vibration on every update
        _notificationManager.CreateNotificationChannel(channel);
    }
}