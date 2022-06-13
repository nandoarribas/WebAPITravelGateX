using System.Collections.Generic;
using System.Text.Json.Serialization;
using WebAPITravelGateX.Util;

namespace WebAPITravelGateX.Model
{
    /// <summary>
    /// Represent the main node wrom the resort hotels api
    /// </summary>
    public class ResortHotels
    {
        /// <summary>
        /// The list of the hotels
        /// </summary>
        public List<ResortHotel> hotels { get; set; }
    }

    /// <summary>
    /// Represent the atalaya hotel
    /// </summary>
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
