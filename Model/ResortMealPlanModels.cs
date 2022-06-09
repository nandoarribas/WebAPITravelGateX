using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebAPITravelGateX.Util;

namespace WebAPITravelGateX.Model
{
    public class ResortMealPlans
    {
        [JsonPropertyName("regimenes")]
        public IEnumerable<ResortMealPlan> MealPlans { get; set; }
    }

    public class ResortMealPlan
    {
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("code")]
        public MealPlan Code { get; set; }
        [JsonPropertyName("name")]
        public string Name { get; set; }
        [JsonPropertyName("hotel")]
        public string HotelCode { get; set; }
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("room_type")]
        public ResortRoomType RoomType { get; set; }
        [JsonPropertyName("price")]
        public decimal Price { get; set; }


    }
}
