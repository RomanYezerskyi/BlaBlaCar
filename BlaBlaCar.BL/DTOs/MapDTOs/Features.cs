using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.MapDTOs
{
    [System.Serializable]
    public class Features
    {
        [JsonProperty("properties")] private Properties _properties;
        public Properties Properties => _properties;
    }
}
