#region

using OpenTracker.Models;

#endregion

namespace OpenTracker.Services;

public interface IDbService
{
    Task AddSessionAsync(TrackingSession session);

    Task<List<TrackingSession>> GetHistoryAsync(string tackerName);

    // Configuration & Manifest
    Task SeedDatabaseAsync(List<TrackerManifestItem> manifest, List<TrackerConfig> configs);
    Task<List<TrackerManifestItem>> GetManifestAsync();
    Task<TrackerConfig?> GetConfigAsync(string fileName);

    // New Methods for Editing
    Task SaveTrackerAsync(TrackerManifestItem manifestItem, TrackerConfig config);
    Task DeleteTrackerAsync(string fileName);
}