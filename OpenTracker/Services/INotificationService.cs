namespace OpenTracker.Services;

public interface INotificationService
{
    void StartNotification(string trackerName, string stageName, DateTime startTime, string displayFormat);
    void UpdateStage(string stageName);
    void StopNotification();
}