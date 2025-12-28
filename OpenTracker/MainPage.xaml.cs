#region

using OpenTracker.ViewModels;

#endregion

namespace OpenTracker;

public partial class MainPage : ContentPage
{
    private readonly MainViewModel _viewModel;
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = _viewModel = viewModel;
        _viewModel.PropertyChanged += (s, e) =>
        {
            if (e.PropertyName == nameof(MainViewModel.Progress) ||
                e.PropertyName == nameof(MainViewModel.CurrentStage))
            {
                StatusGraphicsView.Invalidate();
            }
        };
    }
}

public class TrackerDrawable : BindableObject, IDrawable
{
    // Define BindableProperty
    public static readonly BindableProperty ViewModelProperty =
        BindableProperty.Create(nameof(ViewModel), typeof(MainViewModel), typeof(TrackerDrawable), null);

    public MainViewModel ViewModel
    {
        get => (MainViewModel)GetValue(ViewModelProperty);
        set => SetValue(ViewModelProperty, value);
    }

    public void Draw(ICanvas canvas, RectF dirtyRect)
    {
        if (ViewModel == null) return;

        canvas.Antialias = true;

        float centerX = dirtyRect.Center.X;
        float centerY = dirtyRect.Center.Y;
        // Leave padding for the stroke and emojis
        float radius = Math.Min(dirtyRect.Width, dirtyRect.Height) / 2 - 20;

        // 1. Draw Background Track
        canvas.StrokeColor = Color.FromArgb("#333333");
        canvas.StrokeSize = 6;
        canvas.DrawCircle(centerX, centerY, radius);

        // 2. Draw Progress Arc
        // -90 is 12 o'clock
        float startAngle = 90;
        // Calculate end angle based on progress (0.0 - 1.0)
        float progressAngle = (float)(ViewModel.Progress * 360);

        // Use current stage color or default
        var progressColor = Color.FromArgb(ViewModel.CurrentStage?.ColorHex ?? "#2196F3");
        canvas.StrokeColor = progressColor;
        canvas.StrokeSize = 6;
        canvas.StrokeLineCap = LineCap.Round;

        // Draw the arc (Note: DrawArc takes top-left corner of bounding box, not center)
        canvas.DrawArc(centerX - radius, centerY - radius, radius * 2, radius * 2, startAngle, -progressAngle, false, false);

        // 3. Draw Stage Emojis
        if (ViewModel.Stages != null && ViewModel.MaxHours > 0)
        {
            canvas.FontSize = 14;

            foreach (var stage in ViewModel.Stages)
            {
                // Calculate angle for this stage start time
                float stageRatio = (float)(stage.StartHour / ViewModel.MaxHours);
                // Convert ratio to degrees, offset by -90 to start at top
                double angleRad = (stageRatio * 360 - 90) * (Math.PI / 180);

                // Position on the circle (pushing out slightly to radius + 15)
                float markerR = radius + 15;
                float x = centerX + markerR * (float)Math.Cos(angleRad);
                float y = centerY + markerR * (float)Math.Sin(angleRad);

                // Highlight active stage
                if (stage.IsActive)
                {
                    // Draw a small glow/circle behind active stage icon
                    canvas.FillColor = progressColor.WithAlpha(0.3f);
                    canvas.FillCircle(x, y, 12);
                }

                // Draw Emoji centered at X,Y
                // Note: Simple centering, might need slight adjustment based on font metrics
                canvas.FontColor = Colors.White;
                canvas.DrawString(stage.Icon, x - 10, y - 10, 20, 20, HorizontalAlignment.Center, VerticalAlignment.Center);
            }
        }
    }
}