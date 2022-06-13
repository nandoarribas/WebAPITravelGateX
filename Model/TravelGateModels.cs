using System.Collections.Generic;
using System.Text.Json.Serialization;
using WebAPITravelGateX.Util;

namespace WebAPITravelGateX.Model
{
    /// <summary>
    /// Object to represent the main node with the list of hotels
    /// </summary>
    public class Hotels
    {
        /// <summary>
        /// The list of hotels
        /// </summary>
        public IEnumerable<Hotel> hotels { get; set; }
    }

    /// <summary>
    /// Object to represent the Hotel node for our model
    /// </summary>
    public class Hotel
    {
        /// <summary>
        /// Hotel code
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// Hotel name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// City where hotel is based in
        /// </summary>
        public string City { get; set; }

        /// <summary>
        /// The list of the rooms available for each hotel
        /// This option is used on the first exercise and ignored in the second one
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public IEnumerable<HotelRoomInfo> Rooms { get; set; }

        /// <summary>
        /// The room info used in the second exercise 
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public HotelRoomInfo Room { get; set; }
    }

    /// <summary>
    /// Object to represent all the info related to a room 
    /// </summary>
    public class HotelRoomInfo
    {
        /// <summary>
        /// Representative name for the room
        /// </summary>
        public string Name { get; set; }
        
        /// <summary>
        /// Represents the room code
        /// </summary>
        [JsonPropertyName("room_type")]
        public RoomType RoomType { get; set; }

        /// <summary>
        /// The meal available for the current room
        /// </summary>
        [JsonPropertyName("meal_plan")]
        public MealPlan MealPlan { get; set; }

        /// <summary>
        /// The value for person (exercise 1) or total amount for all nights(
        /// </summary>
        public decimal Price { get; set; }

        /// <summary>
        /// The number of nights to stay into the room
        /// </summary>
        [JsonIgnore(Condition = JsonIgnoreCondition.WhenWritingDefault)]
        public int Nights { get; set; }
        //Useful in itinerary to retrieve hotelcode
        [JsonIgnore]
        public string HotelCode { get; set; }
    }

    /// <summary>
    /// Class to retrieve the itineraries available 
    /// </summary>
    public class Itineraries
    {
        /// <summary>
        /// The list of all available itineraries
        /// </summary>
        public IEnumerable<Itinerary> ItinerariesList { get; set; }
    }

    /// <summary>
    /// Class to represent the itinerary found
    /// </summary>
    public class Itinerary
    {
        /// <summary>
        /// The list of all hotels that fits into the itinerary
        /// </summary>
        public IEnumerable<Hotel> Hotels { get; set; }
    }

    /// <summary>
    /// Class to indicate the params for the itinerary
    /// </summary>
    public class ItineraryInfo
    {
        /// <summary>
        /// The max amount of money for the budget
        /// </summary>
        public int Budget { get; set; }
        /// <summary>
        /// To include all the destinations for your itinerary with some options
        /// </summary>
        public List<Destinations> DestinationNigths { get; set; }
    }

    /// <summary>
    /// Class to indicate the detailed info for each destination
    /// </summary>
    public class Destinations
    {

        /// <summary>
        /// The city to stay in
        /// </summary>
        public string Destination { get; set; }
        /// <summary>
        /// The number of nights
        /// </summary>
        public int Nights { get; set; }
        /// <summary>
        /// The meal plan for the selected destination. If we dont include any meal plan, 
        /// the current destination will accept all meal plans
        /// </summary>
        public string MealPlan { get; set; }
    }
}
