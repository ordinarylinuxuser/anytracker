namespace AnyTracker.Services;

public interface INotificationService
{
    void ShowStickyNotification(string title, string message, int progressPercent);
    void CancelNotification();
}