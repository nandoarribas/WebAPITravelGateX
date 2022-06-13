using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;
using WebAPITravelGateX.Util;

namespace WebAPITravelGateX.Model
{
    /// <summary>
    /// Class to represent the node of mealplans
    /// </summary>
    public class ResortMealPlans
    {
        /// <summary>
        /// The list of all available meal plans
        /// </summary>
        [JsonPropertyName("regimenes")]
        public IEnumerable<ResortMealPlan> MealPlans { get; set; }
    }

    /// <summary>
    /// Represent the meal plan node
    /// </summary>
    public class ResortMealPlan
    {
        /// <summary>
        /// The type of meal plan
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("code")]
        public MealPlan Code { get; set; }
        /// <summary>
        /// The meal plan name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }
        /// <summary>
        /// The hotel that have this meal plan
        /// </summary>
        [JsonPropertyName("hotel")]
        public string HotelCode { get; set; }
        /// <summary>
        /// The room type that include this meal plan
        /// </summary>
        [JsonConverter(typeof(JsonStringEnumConverter))]
        [JsonPropertyName("room_type")]
        public ResortRoomType RoomType { get; set; }
        /// <summary>
        /// The price per person per night
        /// </summary>
        [JsonPropertyName("price")]
        public decimal Price { get; set; }


    }
}
