using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetMauiApp.Services
{
    public class WonderOfTheWorldService
    {
        HttpClient _httpClient;

        List<WonderOfTheWorld> listWondersOfTheWorld = new();

        public WonderOfTheWorldService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<List<WonderOfTheWorld>> GetWondersOfTheWorld()
        {
            if(listWondersOfTheWorld?.Count > 0)
            {
                return listWondersOfTheWorld;
            }


            using var stream = await FileSystem.OpenAppPackageFileAsync("wonders.json");
            using var reader = new StreamReader(stream);
            var contents = await reader.ReadToEndAsync();
            var options = new JsonSerializerOptions { WriteIndented = true, PropertyNameCaseInsensitive = true };
            listWondersOfTheWorld = JsonSerializer.Deserialize<List<WonderOfTheWorld>>(contents, options);


            return listWondersOfTheWorld;
        }
    }
}
