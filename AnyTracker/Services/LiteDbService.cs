#region

using AnyTracker.Constants;
using AnyTracker.Data.Entities;
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

    public async Task<List<TrackingSession>> GetHistoryAsync()
    {
        return await Task.Run(() =>
        {
            using var db = new LiteDatabase(_dbPath);
            var col = db.GetCollection<TrackingSession>("sessions");
            return col.Query()
                .OrderByDescending(x => x.StartTime)
                .ToList();
        });
    }
}