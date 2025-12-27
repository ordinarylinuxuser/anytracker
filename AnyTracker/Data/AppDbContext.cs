#region

using AnyTracker.Constants;
using AnyTracker.Data.Entities;
using Microsoft.EntityFrameworkCore;

#endregion

namespace AnyTracker.Data;

public sealed class AppDbContext : DbContext
{
    public AppDbContext()
    {
        // Required for migrations to work at design time
        Database.EnsureCreated();
    }

    public DbSet<TrackingSession> Sessions { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var dbPath = Path.Combine(FileSystem.AppDataDirectory, AppConstants.DatabaseFilename);
        optionsBuilder.UseSqlite($"Filename={dbPath}");
    }
}