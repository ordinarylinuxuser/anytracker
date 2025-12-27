#region

using AnyTracker.Pages;

#endregion

namespace AnyTracker;

public partial class App : Application
{
    private readonly Page _rootPage;

    public App(LoadingPage loadingPage)
    {
        InitializeComponent();
        _rootPage = loadingPage;
    }

    protected override Window CreateWindow(IActivationState? activationState)
    {
        return new Window(_rootPage);
    }
}