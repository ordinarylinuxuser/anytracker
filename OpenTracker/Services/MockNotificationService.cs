namespace OpenTracker.Services;

public class MockNotificationService : INotificationService
{
    public void StartNotification(string trackerName, string stageName, DateTime startTime, string displayFormat)
    {
        // No-op for non-Android platforms
    }

    public void UpdateStage(string stageName)
    {
        // No-op
    }

    public void StopNotification()
    {
        // No-op
    }
}