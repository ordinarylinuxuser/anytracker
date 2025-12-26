namespace AnyTracker;

public partial class App : Application
{
    private readonly Page _rootPage;

    public App(MainPage mainPage)
    {
        InitializeComponent();
        // Wrap the MainPage in a NavigationPage so PushAsync works!
        // Create the root page in a field; CreateWindow will host it.
        _rootPage = new NavigationPage(mainPage)
        {
            BarBackgroundColor = Color.FromArgb("#121212"),
            BarTextColor = Colors.White
        };
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_rootPage);
    }
}