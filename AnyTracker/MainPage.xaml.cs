using AnyTracker.ViewModels;

namespace AnyTracker;

public partial class MainPage : ContentPage
{
    public MainPage(TrackingViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
}