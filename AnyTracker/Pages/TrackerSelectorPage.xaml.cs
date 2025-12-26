using AnyTracker.Constants;
using AnyTracker.Models;
using AnyTracker.Utilities;

namespace AnyTracker.Pages;

public partial class TrackerSelectorPage
{
    public TrackerSelectorPage()
    {
        InitializeComponent();
        LoadManifest();
    }

    public event Action<TrackerManifestItem> OnTrackerSelected;

    private async void LoadManifest()
    {
        try
        {
            var items =
                await ResourceHelper.LoadJsonResourceFile<List<TrackerManifestItem>>(AppConstants.TrackerManifestFile);

            TrackerList.ItemsSource = items;
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