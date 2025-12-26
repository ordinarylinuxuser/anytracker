namespace AnyTracker.Pages;

public partial class SettingsPage
{
    private readonly string _currentTrackerName;
    private readonly Action<string> _onTrackerChanged;

    public SettingsPage(string currentTrackerName, Action<string> onTrackerChanged)
    {
        InitializeComponent();
        _currentTrackerName = currentTrackerName;
        _onTrackerChanged = onTrackerChanged;

        CurrentTrackerLabel.Text = _currentTrackerName;
    }

    private async void OnChangeTrackerTapped(object sender, EventArgs e)
    {
        // Open the existing Selector Page
        var selectorPage = new TrackerSelectorPage();

        selectorPage.OnTrackerSelected += async item =>
        {
            // 1. Notify MainViewModel to load the new file
            _onTrackerChanged?.Invoke(item.FileName);

            // 2. Update the label here immediately (optional, but looks nice)
            CurrentTrackerLabel.Text = item.Name;

            // 3. Close the Selector
            // (The Selector code we wrote earlier closes itself, 
            // but if it didn't, we would do it here).
        };

        await Navigation.PushModalAsync(selectorPage);
    }

    private async void OnAboutTapped(object sender, EventArgs e)
    {
        await Navigation.PushModalAsync(new AboutPage());
    }
}