#region

using OpenTracker.Services;

#endregion

namespace OpenTracker.Pages;

public partial class SettingsPage
{
    private readonly TrackerService _trackerService;

    public SettingsPage(TrackerService trackerService)
    {
        InitializeComponent();
        _trackerService = trackerService;

        // Set initial label
        UpdateLabel();

        // Listen for changes (in case changed elsewhere)
        _trackerService.OnTrackerChanged += UpdateLabel;
    }

    private void UpdateLabel()
    {
        if (_trackerService.CurrentConfig != null) CurrentTrackerLabel.Text = _trackerService.CurrentConfig.TrackerName;
    }

    private async void OnChangeTrackerTapped(object sender, EventArgs e)
    {
        // Pass the service to the selector
        var selectorPage = new TrackerSelectorPage(_trackerService);

        selectorPage.OnTrackerSelected += async item =>
        {
            await _trackerService.LoadTrackerConfigAsync(item.FileName);
        };

        await Navigation.PushModalAsync(selectorPage);
    }

    private async void OnAboutTapped(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AboutPage());
    }
}