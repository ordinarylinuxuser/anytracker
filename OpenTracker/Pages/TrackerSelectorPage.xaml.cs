#region

using OpenTracker.Constants;
using OpenTracker.Models;
using OpenTracker.Services;
using OpenTracker.Utilities;

#endregion

namespace OpenTracker.Pages;

public partial class TrackerSelectorPage
{
    private readonly TrackerService _trackerService;

    // Default constructor for XAML previewer (optional) or fallback
    public TrackerSelectorPage()
    {
        InitializeComponent();
    }

    public TrackerSelectorPage(TrackerService trackerService)
    {
        InitializeComponent();
        _trackerService = trackerService;
        LoadManifest();
    }

    public event Action<TrackerManifestItem> OnTrackerSelected;

    private async void LoadManifest()
    {
        try
        {
            if (_trackerService != null)
            {
                // Load from service (which loads from DB)
                TrackerList.ItemsSource = _trackerService.Manifest;
            }
            else
            {
                // Fallback if service not injected (e.g. testing)
                var items =
                    await ResourceHelper.LoadJsonResourceFile<List<TrackerManifestItem>>(AppConstants
                        .TrackerManifestFile);
                TrackerList.ItemsSource = items;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", "Could not load tracker list.", "OK");
        }
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is not TrackerManifestItem selected) return;
        OnTrackerSelected?.Invoke(selected);
        await Navigation.PopModalAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}