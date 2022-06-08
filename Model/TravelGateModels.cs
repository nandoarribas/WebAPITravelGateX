using System.Collections.Generic;
using System.Text.Json.Serialization;
using WebAPITravelGateX.Util;

namespace WebAPITravelGateX.Model
{
    public class Hotels
    {
        public IEnumerable<Hotel> hotels { get; set; }
    }

    public class Hotel
    {
        public string Code { get; set; }

        public string Name { get; set; }

        public string City { get; set; }

        public IEnumerable<HotelRoomInfo> Rooms { get; set; }
    }

    public class HotelRoomInfo
    {
        public string Name { get; set; }

        [JsonPropertyName("room_type")]
        public RoomType RoomType { get; set; }

        [JsonPropertyName("meal_plan")]
        public MealPlan MealPlan { get; set; }
        public decimal Price { get; set; }
    }
}
