using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace AnyTracker.Data.Entities;

public class TrackingSession
{
    [Key] public int Id { get; set; }

    public string TrackerName { get; set; }
    public DateTime StartTime { get; set; }
    public DateTime EndTime { get; set; }
    public double DurationSeconds { get; set; }

    // Not mapped to DB, just for UI
    [NotMapped]
    public string DurationDisplay
    {
        get
        {
            var span = TimeSpan.FromSeconds(DurationSeconds);
            if (span.TotalHours >= 1) return $"{span.TotalHours:F1} hrs";
            return $"{span.TotalMinutes:F0} mins";
        }
    }
}