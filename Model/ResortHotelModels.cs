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
        public IEnumerable<ResortHotelRoom> Rooms { get; set; }

    }

    /// <summary>
    /// Notice that we have to swap the variable names because the name attribute refers to the code(Standard/Suite) and vice versa
    /// </summary>
    public class ResortHotelRoom
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("name")]
        public RoomType Code { get; set; }
        [JsonPropertyName("code")]
        public string Name { get; set; }
    }
}
