#region

using OpenTracker.ViewModels;

#endregion

namespace OpenTracker;

public partial class MainPage : ContentPage
{
    public MainPage(MainViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}