using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace WebAPITravelGateX.Model
{
    /// <summary>
    /// Class to represent the atalaya hotels list
    /// </summary>
    public class AtalayaHotels
    {
        /// <summary>
        /// The list of atalaya hotels
        /// </summary>
        [JsonPropertyName("hotels")]
        public List<AtalayaHotel> hotels { get; set; }
    }

    /// <summary>
    /// Represent the atalaya hotel
    /// </summary>
    public class AtalayaHotel
    {
        /// <summary>
        /// The city where the hotel is located in
        /// </summary>
        [JsonPropertyName("city")]
        public string City { get; set; }
        /// <summary>
        /// The short name of the hotel
        /// </summary>
        [JsonPropertyName("code")]
        public string Code { get; set; }
        /// <summary>
        /// The name of the hotel
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
    }
}
