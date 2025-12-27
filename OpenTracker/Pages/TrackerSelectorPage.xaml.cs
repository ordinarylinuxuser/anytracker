#region

using OpenTracker.Models;
using OpenTracker.Services;
using OpenTracker.ViewModels;

#endregion

namespace OpenTracker.Pages;

public partial class TrackerSelectorPage
{
    private readonly TrackerService _trackerService;
    // We need IDbService to delete, let's inject IServiceProvider to resolve dependencies simply here
    // or change constructor to accept IDbService if possible.
    // For modifying existing pages, we'll try to stick to existing patterns or resolve manually if needed.

    public TrackerSelectorPage()
    {
        InitializeComponent();
    }

    public TrackerSelectorPage(TrackerService trackerService)
    {
        InitializeComponent();
        _trackerService = trackerService;

        // Subscribe to changes so the list refreshes when we come back from the editor
        // Ideally this should be an ObservableCollection in a ViewModel, but we'll reload manually.
    }

    protected override void OnAppearing()
    {
        base.OnAppearing();
        LoadManifest();
    }

    public event Action<TrackerManifestItem> OnTrackerSelected;

    private async void LoadManifest()
    {
        try
        {
            if (_trackerService != null)
            {
                // Force reload manifest from DB in case we added/edited items
                // We need to access the underlying DB service via the TrackerService if exposed, 
                // or just re-initialize. 
                // For this implementation, let's assume TrackerService has a RefreshManifest method or we access DB directly.
                // Since TrackerService exposes Manifest property, let's refresh it.
                await _trackerService.InitializeAsync();
                TrackerList.ItemsSource = _trackerService.Manifest;
            }
        }
        catch (Exception ex)
        {
            await DisplayAlertAsync("Error", "Could not load tracker list.", "OK");
        }
    }

    private async void OnSelectionChanged(object sender, SelectionChangedEventArgs e)
    {
        if (e.CurrentSelection.FirstOrDefault() is TrackerManifestItem selected &&
            selected.Name != _trackerService.CurrentConfig.TrackerName) OnTrackerSelected?.Invoke(selected);

        await Navigation.PopModalAsync();
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }

    private async void OnAddTrackerClicked(object sender, EventArgs e)
    {
        // Resolve the Editor ViewModel and Page manually from the container for this action
        var vm = Application.Current.Handler.MauiContext.Services.GetService<TrackerEditorViewModel>();
        vm.InitializeNew();

        var page = new TrackerEditorPage(vm);
        await Navigation.PushModalAsync(page);
    }

    private async void OnEditClicked(object sender, EventArgs e)
    {
        if (sender is not Button btn || btn.CommandParameter is not TrackerManifestItem manifestItem) return;
        // Load config and open editor
        var dbService = Application.Current.Handler.MauiContext.Services.GetService<IDbService>();
        var config = await dbService.GetConfigAsync(manifestItem.FileName);

        if (config != null)
        {
            var vm = Application.Current.Handler.MauiContext.Services.GetService<TrackerEditorViewModel>();
            vm.LoadForEditing(manifestItem, config);
            await Navigation.PushModalAsync(new TrackerEditorPage(vm));
        }
    }

    private async void OnDeleteClicked(object sender, EventArgs e)
    {
        if (sender is not Button btn || btn.CommandParameter is not TrackerManifestItem manifestItem) return;
        // Prevent deleting the currently active tracker to avoid issues
        if (_trackerService.CurrentConfig?.FileName == manifestItem.FileName)
        {
            await DisplayAlertAsync("Cannot Delete",
                "You cannot delete the currently active tracker. Switch to another tracker first.", "OK");
            return;
        }

        var answer = await DisplayAlertAsync("Delete Tracker",
            $"Are you sure you want to delete {manifestItem.Name}?",
            "Yes", "No");
        if (!answer) return;

        var dbService = Application.Current.Handler.MauiContext.Services.GetService<IDbService>();
        await dbService.DeleteTrackerAsync(manifestItem.FileName);
        LoadManifest(); // Refresh list
    }
}