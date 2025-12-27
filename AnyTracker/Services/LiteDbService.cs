#region

using AnyTracker.Constants;
using AnyTracker.Models;
using LiteDB;

#endregion

namespace AnyTracker.Services;

public class LiteDbService : IDbService
{
    private readonly string _dbPath;

    public LiteDbService()
    {
        _dbPath = Path.Combine(FileSystem.AppDataDirectory, AppConstants.DatabaseFilename);
    }


    public async Task AddSessionAsync(TrackingSession session)
    {
        // LiteDB is synchronous, but we wrap it in Task to satisfy the interface 
        // and keep UI responsive.
        await Task.Run(() =>
        {
            using var db = new LiteDatabase(_dbPath);
            var col = db.GetCollection<TrackingSession>("sessions");
            col.Insert(session);
        });
    }

    public async Task<List<TrackingSession>> GetHistoryAsync(string trackerName)
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(_dbPath);
            var col = db.GetCollection<TrackingSession>("sessions");
            return col.Query()
                .Where(s => s.TrackerName.Equals(trackerName, StringComparison.OrdinalIgnoreCase))
                .OrderByDescending(x => x.StartTime)
                .ToList();
        });
    }

    public async Task SeedDatabaseAsync(List<TrackerManifestItem> manifest, List<TrackerConfig> configs)
    {
        await Task.Run(() =>
        {
            using var db = new LiteDatabase(_dbPath);
            var manifestCol = db.GetCollection<TrackerManifestItem>("manifest");
            var configCol = db.GetCollection<TrackerConfig>("configs");

            // Only insert if empty
            if (manifestCol.Count() == 0)
            {
                manifestCol.InsertBulk(manifest);
                configCol.InsertBulk(configs);
            }
        });
    }

    public async Task<List<TrackerManifestItem>> GetManifestAsync()
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(_dbPath);
            var col = db.GetCollection<TrackerManifestItem>("manifest");
            return col.FindAll().ToList();
        });
    }

    public async Task<TrackerConfig?> GetConfigAsync(string fileName)
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(_dbPath);
            var col = db.GetCollection<TrackerConfig>("configs");
            return col.FindById(fileName);
        });
    }
}