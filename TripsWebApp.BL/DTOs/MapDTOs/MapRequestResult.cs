using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BlaBlaCar.BL.DTOs.MapDTOs
{
    [System.Serializable]
    public class MapRequestResult
    {
        [JsonProperty("features")] private List<Features> _featuresList;
        public List<Features> FeaturesList => _featuresList;
    }
}
