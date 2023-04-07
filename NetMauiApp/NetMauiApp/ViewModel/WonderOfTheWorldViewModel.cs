using Microsoft.Maui.Devices.Sensors;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMauiApp.ViewModel
{
    public partial class WonderOfTheWorldViewModel : BaseViewModel
    {
         WonderOfTheWorldService _wonderService;
        public ObservableCollection<WonderOfTheWorld> WonderOfTheWorlds { get; } = new ObservableCollection<WonderOfTheWorld>();

        //public Command GetWondersComand { get; }

        private readonly IConnectivity _connectivity;
        private readonly IGeolocation _geolocation;
        public WonderOfTheWorldViewModel(WonderOfTheWorldService wonderOfTheWorldService,
            IConnectivity connectivity,
            IGeolocation geolocation)
        {
            Title = "Wonders Of TheWorld";
            _wonderService = wonderOfTheWorldService;
            _connectivity = connectivity;
            _geolocation = geolocation;
        }

        [RelayCommand]
        public async Task GoToWonderOfTeWordsDetails(WonderOfTheWorld wonderOfTheWorld)
        {
            if (wonderOfTheWorld == null)
            {
                return;
            }

            await Shell.Current.GoToAsync($"{nameof(DetailsPage)}", true, new Dictionary<string, object> { { "WonderOfTheWorld", wonderOfTheWorld } }); 
           
        }

        [RelayCommand]
        public async Task GetClosestWondersAsync()
        {
            if (IsBusy || WonderOfTheWorlds.Count == 0) { return; }

            try
            {
                var location = await _geolocation.GetLastKnownLocationAsync();
                if (location == null)
                {
                    location = await _geolocation.GetLocationAsync(new GeolocationRequest
                    {
                        DesiredAccuracy = GeolocationAccuracy.Medium,
                        Timeout = TimeSpan.FromSeconds(30)
                    });
                }

                var first = WonderOfTheWorlds.OrderBy(m => location.CalculateDistance(m.Latitude, m.Longitude, DistanceUnits.Kilometers)).FirstOrDefault();

                if (first == null) { return; }

                await Shell.Current.DisplayAlert("Closest Wonder", $"{first.Name}, {first.Location}", "OK!");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", $"Unnable to closest list of wonders {ex.Message}", "OK!");
            }
        }



        [RelayCommand]
        private async Task GetWondersAsync()
        {
            if (_connectivity.NetworkAccess != NetworkAccess.Internet)
            {
                await Shell.Current.DisplayAlert("Internet Error", "Check your Internet", "Close");
            }

            if(IsBusy) return;

            try
            {
                IsBusy = true;
                var wonders = await _wonderService.GetWondersOfTheWorld();

                if(WonderOfTheWorlds.Count != 0)
                {
                    WonderOfTheWorlds.Clear();
                }

                foreach (var wonder in wonders)
                {
                    WonderOfTheWorlds.Add(wonder);
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex);
                await Shell.Current.DisplayAlert("Error", $"Unnable to get wonders {ex.Message}", "OK!");
            }
            finally 
            {
                IsBusy = false; 
            }
        }
    }
}
