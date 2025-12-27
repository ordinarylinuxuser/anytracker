#region

using System.Diagnostics;
using AnyTracker.Constants;
using AnyTracker.Models;
using AnyTracker.Utilities;

#endregion

namespace AnyTracker.Services;

public class TrackerService
{
    public TrackerConfig CurrentConfig { get; private set; }
    public List<TrackerManifestItem> Manifest { get; private set; } = [];

    public event Action OnTrackerChanged;

    public async Task InitializeAsync()
    {
        // 1. Load Manifest
        try
        {
            Manifest =
                await ResourceHelper.LoadJsonResourceFile<List<TrackerManifestItem>>(AppConstants.TrackerManifestFile);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading manifest: {ex.Message}");
        }

        // 2. Load Default Config (or last used)
        // For now, default to the one in AppConstants
        await LoadTrackerConfigAsync(AppConstants.DefaultTrackerFile);
    }

    public async Task LoadTrackerConfigAsync(string filename)
    {
        try
        {
            CurrentConfig = await ResourceHelper.LoadJsonResourceFile<TrackerConfig>(filename);
            OnTrackerChanged?.Invoke();
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error loading config {filename}: {ex.Message}");
        }
    }
}