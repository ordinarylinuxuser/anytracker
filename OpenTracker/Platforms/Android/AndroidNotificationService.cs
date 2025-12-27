#region

using Android.Content;
using Android.OS;
using OpenTracker.Services;
using Application = Android.App.Application;

#endregion

namespace OpenTracker;

public class AndroidNotificationService : INotificationService
{
    public void StartNotification(string trackerName, string stageName, DateTime startTime, string displayFormat)
    {
        var context = Application.Context;
        var intent = new Intent(context, typeof(TickerService));
        intent.PutExtra("TrackerName", trackerName);
        intent.PutExtra("StageName", stageName);
        intent.PutExtra("StartTime", startTime.Ticks);
        intent.PutExtra("DisplayFormat", displayFormat);

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            context.StartForegroundService(intent);
        else
            context.StartService(intent);
    }

    public void UpdateStage(string stageName)
    {
        // Update the running service with the new stage name
        var context = Application.Context;
        var intent = new Intent(context, typeof(TickerService));
        intent.PutExtra("UpdateStageOnly", true);
        intent.PutExtra("StageName", stageName);

        if (Build.VERSION.SdkInt >= BuildVersionCodes.O)
            context.StartForegroundService(intent);
        else
            context.StartService(intent);
    }

    public void StopNotification()
    {
        var context = Application.Context;
        var intent = new Intent(context, typeof(TickerService));
        intent.SetAction("STOP_SERVICE");
        context.StartService(intent);
    }
}