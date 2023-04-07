

namespace NetMauiApp.ViewModel
{
    [QueryProperty(nameof(WonderOfTheWorld), "WonderOfTheWorld")]
    public  partial class WonderOfTheWorldDetailsViewModel : BaseViewModel
    {
        private readonly IMap _map;
        public WonderOfTheWorldDetailsViewModel(IMap map)
        {
            _map = map;
        }

        [ObservableProperty]
        WonderOfTheWorld wonderOfTheWorld;

        [RelayCommand]
        public async Task OpenMapAsync()
        {
            try
            {
                await _map.OpenAsync(wonderOfTheWorld.Longitude, wonderOfTheWorld.Latitude, new MapLaunchOptions
                {
                    Name = wonderOfTheWorld.Name,
                    NavigationMode = NavigationMode.None
                });
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", $"Unnable to open map {ex.Message}", "OK!");
            }
        }
    }
}
