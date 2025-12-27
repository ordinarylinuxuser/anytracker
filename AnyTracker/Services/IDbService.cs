using AnyTracker.Data.Entities;

namespace AnyTracker.Services;

public interface IDbService
{
    Task AddSessionAsync(TrackingSession session);

    Task<List<TrackingSession>> GetHistoryAsync();
}