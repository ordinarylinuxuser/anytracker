#region

using OpenTracker.Services;

#endregion

namespace OpenTracker.Pages;

public partial class LoadingPage : ContentPage
{
    private readonly TrackerService _trackerService;

    public LoadingPage(TrackerService trackerService)
    {
        InitializeComponent();
        _trackerService = trackerService;
    }

    protected override async void OnAppearing()
    {
        base.OnAppearing();

        // Load data
        await _trackerService.InitializeAsync();

        // Artificial delay if needed for UX, or just proceed
        await Task.Delay(500);

        // Switch to Main App Shell
        Application.Current.MainPage = new AppShell();
    }
}