using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.MapDTOs
{
    [System.Serializable]
    public class Properties
    {
        [JsonProperty("name")] private string _name;
        [JsonProperty("housenumber")] private string _housenumber;
        [JsonProperty("street")] private string _street;
        [JsonProperty("suburb")] private string _suburb;
        [JsonProperty("city")] private string _city;
        [JsonProperty("district")] private string _district;
        [JsonProperty("state")] private string _state;
        [JsonProperty("postcode")] private string _postcode;
        [JsonProperty("country")] private string _country;
        [JsonProperty("country_code")] private string _countryCode;
        [JsonProperty("lon")] private float _lon;
        [JsonProperty("lat")] private float _lat;
        [JsonProperty("distance")] private float _distance;
        [JsonProperty("result_type")] private string _resultType;
        [JsonProperty("formatted")] private string _formatted;
        [JsonProperty("address_line1")] private string _addressLine1;
        [JsonProperty("address_line2")] private string _addressLine2;
        [JsonProperty("category")] private string _category;
        [JsonProperty("place_id")] private string _placeId;
        public string Name => _name;
        public string HouseNumber => _housenumber;
        public string Street => _street;
        public string Suburb => _suburb;
        public string City => _city;
        public string District => _district;
        public string State => _state;
        public string Postcode => _postcode;
        public string Country => _country;
        public string CountryCode => _countryCode;
        public float Lon => _lon;
        public float Lat => _lat;
        public float Distance => _distance;
        public string ResultType => _resultType;
        public string Formatted => _formatted;
        public string AddressLine1 => _addressLine1;
        public string AddressLine2 => _addressLine2;
        public string Category => _category;
        public string PlaceId => _placeId;
    }
}
