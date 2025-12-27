#region

using OpenTracker.Models;

#endregion

namespace OpenTracker.Pages;

public partial class StageEditorPage : ContentPage
{
    private TrackingStage _stage;
    private Action<TrackingStage> _onSave;
    private string _selectedColor = "#FFFFFF";

    public List<string> Emojis { get; } = new()
    {
        "🔥", "⚡", "💤", "🍽️", "🧠", "🏋️", "🧘", "💧", "☕", "🍺", "🚫", "✅", "👶", "💊", "📚", "🌡️", "🚀", "🚨"
    };

    public StageEditorPage(TrackingStage stage, Action<TrackingStage> onSave)
    {
        InitializeComponent();
        _stage = stage;
        _onSave = onSave;

        // FIX: Set BindingContext to 'this' so the XAML {Binding Emojis} works.
        // We removed the incorrect "EmojiGrid.BindableLayout.ItemsSource = ..." line.
        BindingContext = this;

        LoadColors();
        LoadStageData();
    }

    private void LoadColors()
    {
        var colors = new[]
        {
            "#F44336", "#E91E63", "#9C27B0", "#673AB7", "#3F51B5", "#2196F3", "#03A9F4", "#00BCD4", "#009688",
            "#4CAF50", "#8BC34A", "#CDDC39", "#FFEB3B", "#FFC107", "#FF9800", "#FF5722"
        };

        foreach (var c in colors)
        {
            var btn = new Button
            {
                BackgroundColor = Color.FromArgb(c),
                WidthRequest = 40,
                HeightRequest = 40,
                CornerRadius = 20,
                Margin = 2,
                BorderColor = Colors.White,
                BorderWidth = 0
            };
            btn.Clicked += (s, e) =>
            {
                _selectedColor = c;
                // Visual feedback
                TitleEntry.TextColor = Color.FromArgb(c);
            };
            ColorGrid.Children.Add(btn);
        }
    }

    private void LoadStageData()
    {
        TitleEntry.Text = _stage.Title;
        DescEntry.Text = _stage.Description;
        StartEntry.Text = _stage.StartHour.ToString();
        EndEntry.Text = _stage.EndHour.ToString();
        EmojiEntry.Text = _stage.Icon;
        _selectedColor = _stage.ColorHex;
    }

    private void OnEmojiClicked(object sender, EventArgs e)
    {
        if (sender is Button btn) EmojiEntry.Text = btn.Text;
    }

    private async void OnSaveClicked(object sender, EventArgs e)
    {
        _stage.Title = TitleEntry.Text;
        _stage.Description = DescEntry.Text;
        _stage.Icon = EmojiEntry.Text;
        _stage.ColorHex = _selectedColor;

        if (double.TryParse(StartEntry.Text, out double start)) _stage.StartHour = start;
        if (double.TryParse(EndEntry.Text, out double end)) _stage.EndHour = end;

        _onSave?.Invoke(_stage);
        await Navigation.PopModalAsync();
    }
}