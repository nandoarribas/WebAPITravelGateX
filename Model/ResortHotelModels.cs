using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebAPITravelGateX.Model
{
    public class ResortHotels
    {
        public List<ResortHotel> hotels { get; set; }
    }

    public class ResortHotel
    {
        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("code")]
        public string Code { get; set; }

        [JsonPropertyName("location")]
        public string Location { get; set; }

    }
}
