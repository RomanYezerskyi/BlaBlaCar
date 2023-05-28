using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.MapDTOs;
using BlaBlaCar.BL.Interfaces;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;


namespace BlaBlaCar.BL.Services.MapsServices
{
    public class MapsService:IMapService
    {
        private readonly HostSettings _hostSettings;

        public MapsService(IOptionsSnapshot<HostSettings> hostSettings)
        {
            _hostSettings = hostSettings.Value;
        }

        public async Task<MapRequestResult?> GetPlaceInformation(double latitude, double longitude)
        {
            var locationLatitude = latitude.ToString(CultureInfo.InvariantCulture);
            var locationLongitude = longitude.ToString(CultureInfo.InvariantCulture);
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(
                $"{_hostSettings.GeoapifyApiUrl}lat={locationLatitude}&lon={locationLongitude}&{_hostSettings.GeoapifyApiKey}");
            if (response.IsSuccessStatusCode)
            {
                var jsonData = await response.Content.ReadAsStringAsync();
                var result =
                    JsonConvert.DeserializeObject<MapRequestResult>(jsonData);
                return result;
            }

            throw new Exception("Location not found !");
        }
    }
}
