#region

using OpenTracker.ViewModels;

#endregion

namespace OpenTracker.Pages;

public partial class TrackerEditorPage : ContentPage
{
    public TrackerEditorPage(TrackerEditorViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }

    private async void OnCancelClicked(object sender, EventArgs e)
    {
        await Navigation.PopModalAsync();
    }
}