namespace NetMauiApp.View;

public partial class MainPage : ContentPage
{

	public MainPage(WonderOfTheWorldViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
	}

   
}

