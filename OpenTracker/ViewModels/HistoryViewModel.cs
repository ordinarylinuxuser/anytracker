#region

using System.Collections.ObjectModel;
using OpenTracker.Models;
using OpenTracker.Services;

#endregion

namespace OpenTracker.ViewModels;

public class ChartBar
{
    public string DayName { get; set; }
    public double Height { get; set; }
    public string ColorHex { get; set; }
    public string ValueLabel { get; set; }
}

public class HistoryViewModel : BindableObject
{
    private readonly IDbService _dbService;
    private readonly TrackerService _trackerService;
    private bool _isLoading;

    public HistoryViewModel(IDbService dbService, TrackerService trackerService)
    {
        _dbService = dbService;
        _trackerService = trackerService;
        RefreshCommand = new Command(async () => await LoadHistoryAsync());
    }

    public ObservableCollection<TrackingSession> HistoryList { get; set; } = [];
    public ObservableCollection<ChartBar> ChartData { get; set; } = [];

    public bool IsLoading
    {
        get => _isLoading;
        set
        {
            _isLoading = value;
            OnPropertyChanged();
        }
    }

    public Command RefreshCommand { get; }

    public async Task LoadHistoryAsync()
    {
        if (IsLoading) return;
        IsLoading = true;

        try
        {
            var currentConfig = _trackerService.CurrentConfig;
            if (currentConfig == null) return;
            var sessions = await _dbService.GetHistoryAsync(currentConfig.TrackerName);

            HistoryList.Clear();
            foreach (var s in sessions) HistoryList.Add(s);

            PrepareChartData(sessions);
        }
        finally
        {
            IsLoading = false;
        }
    }

    private void PrepareChartData(List<TrackingSession> sessions)
    {
        ChartData.Clear();

        // Group by Day (Last 7 days)
        var last7Days = Enumerable.Range(0, 7)
            .Select(i => DateTime.Today.AddDays(-6 + i))
            .ToList();

        var grouped = sessions
            .Where(s => s.StartTime.Date >= last7Days.First())
            .GroupBy(s => s.StartTime.Date)
            .ToDictionary(g => g.Key, g => g.Sum(x => x.DurationSeconds));

        // Find max for scaling
        var maxDuration = grouped.Values.DefaultIfEmpty(0).Max();
        if (maxDuration == 0) maxDuration = 1;

        foreach (var date in last7Days)
        {
            var totalSeconds = grouped.ContainsKey(date) ? grouped[date] : 0;
            var height = totalSeconds / maxDuration * 150; // 150 is max height of bar UI

            // Min height for visibility if there is data
            if (totalSeconds > 0 && height < 5) height = 5;

            ChartData.Add(new ChartBar
            {
                DayName = date.ToString("ddd"), // Mon, Tue
                Height = height,
                ColorHex = totalSeconds > 0 ? "#2196F3" : "#333333",
                ValueLabel = totalSeconds > 0 ? FormatDuration(totalSeconds) : ""
            });
        }
    }

    private string FormatDuration(double seconds)
    {
        var span = TimeSpan.FromSeconds(seconds);
        if (span.TotalHours >= 1) return $"{span.TotalHours:F1}h";
        return $"{span.TotalMinutes:F0}m";
    }
}