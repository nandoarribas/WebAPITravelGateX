using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebAPITravelGateX.Util;

namespace WebAPITravelGateX.Model
{
    /// <summary>
    /// Represent the atalaya rooms main node in atalaya room api
    /// </summary>
    public class AtalayaRooms
    {
        /// <summary>
        /// The list with all room types  for atalaya
        /// </summary>
        [JsonPropertyName("rooms_type")]
        public IEnumerable<AtalayaRoom> RoomsType { get; set; }
    }

    /// <summary>
    /// Represent the node with the room
    /// </summary>
    public class AtalayaRoom
    {
        /// <summary>
        /// The list with all code hotels that include this room
        /// </summary>
        [JsonPropertyName("hotels")]
        public IEnumerable<string> Hotels { get; set; }

        /// <summary>
        /// The type of the room
        /// </summary>
        [JsonPropertyName("code")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public RoomType Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
