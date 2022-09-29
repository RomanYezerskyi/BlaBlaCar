using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using BlaBlaCar.BL.DTOs.MapDTOs;
using BlaBlaCar.BL.Interfaces;
using Newtonsoft.Json;


namespace BlaBlaCar.BL.Services.MapsServices
{
    public class MapsService:IMapService
    {

        public async Task<MapRequestResult?> GetPlaceInformation(double latitude, double longitude)
        {
            var locationLatitude = latitude.ToString(CultureInfo.InvariantCulture);
            var locationLongitude = longitude.ToString(CultureInfo.InvariantCulture);
            using var httpClient = new HttpClient();
            using var response = await httpClient.GetAsync(
                $"https://api.geoapify.com/v1/geocode/reverse?lat={locationLatitude}&lon={locationLongitude}&apiKey=32849d6b3d3b480c9a60be1ce5891252");
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
