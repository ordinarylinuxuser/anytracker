namespace OpenTracker.Services;

public interface INotificationService
{
    void ShowStickyNotification(string title, string message);
    void CancelNotification();
}