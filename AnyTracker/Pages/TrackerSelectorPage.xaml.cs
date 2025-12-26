using System.Text.Json;
using AnyTracker.Models;

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
            await using var stream = await FileSystem.OpenAppPackageFileAsync("tracker_manifest.json");
            using var reader = new StreamReader(stream);
            var json = await reader.ReadToEndAsync();
            var items = JsonSerializer.Deserialize<List<TrackerManifestItem>>(json);
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