using System.Collections.Generic;
using System.Text.Json.Serialization;
using WebAPITravelGateX.Util;

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
        [JsonPropertyName("rooms")]
        public IEnumerable<ResortHotelRoom> rooms { get; set; }

    }

    public class ResortHotelRoom
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("code")]
        public RoomType code { get; set; }
        [JsonPropertyName("name")]
        public string name { get; set; }
    }
}
