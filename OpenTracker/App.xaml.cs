#region

using OpenTracker.Pages;

#endregion

namespace OpenTracker;

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