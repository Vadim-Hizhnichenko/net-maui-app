namespace NetMauiApp.View;


public partial class DetailsPage : ContentPage
{
    public DetailsPage(WonderOfTheWorldDetailsViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

    protected override void OnNavigatedTo(NavigatedToEventArgs args)
    {
        base.OnNavigatedTo(args);
    }



}