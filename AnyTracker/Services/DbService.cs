using AnyTracker.Data;
using AnyTracker.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace AnyTracker.Services;

public class DbService : IDbService
{
    public async Task AddSessionAsync(TrackingSession session)
    {
        await using var context = new AppDbContext();
        context.Sessions.Add(session);
        await context.SaveChangesAsync();
    }

    public async Task<List<TrackingSession>> GetHistoryAsync()
    {
        await using var context = new AppDbContext();

        // EF Core LINQ query
        return await context.Sessions
            .OrderByDescending(s => s.StartTime)
            .AsNoTracking() // Performance optimization for read-only lists
            .ToListAsync();
    }
}