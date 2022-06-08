using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebAPITravelGateX.Util;

namespace WebAPITravelGateX.Model
{
    public class AtalayaRooms
    {
        [JsonPropertyName("rooms_type")]
        public IEnumerable<AtalayaRoom> RoomsType { get; set; }
    }
    public class AtalayaRoom
    {
        [JsonPropertyName("hotels")]
        public IEnumerable<string> Hotels { get; set; }

        [JsonPropertyName("code")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RoomType Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
