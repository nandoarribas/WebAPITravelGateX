using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebAPITravelGateX.Model
{
    public class AtalayaHotels
    {
        [JsonPropertyName("hotels")]
        public List<AtalayaHotel> hotels { get; set; }
    }

    public class AtalayaHotel
    {
        [JsonPropertyName("city")]
        public string City { get; set; }
        [JsonPropertyName("code")]
        public string Code { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
