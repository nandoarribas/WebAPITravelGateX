using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebAPITravelGateX.Util;

namespace WebAPITravelGateX.Model
{
    /// <summary>
    /// Represent the node of meal plans
    /// </summary>
    public class AtalayaMealPlans
    {
        /// <summary>
        /// The list of all meal plans available
        /// </summary>
        [JsonPropertyName("meal_plans")]
        public IEnumerable<AtalayaMealPlan> Meals { get; set; }
    }

    /// <summary>
    /// Class to represent the meal plan node
    /// </summary>
    public class AtalayaMealPlan
    {
        /// <summary>
        /// The type of meal plan
        /// </summary>
        [JsonPropertyName("code")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MealPlan Code { get; set; }

        /// <summary>
        /// The meal plan name
        /// </summary>
        [JsonPropertyName("name")]
        public string Name { get; set; }

        /// <summary>
        /// The node with hotels and their fares
        /// </summary>
        [JsonPropertyName("hotel")]
        public JsonElement Hotel { get; set; }
    }

    /// <summary>
    /// Class to represent the meal plan fares
    /// </summary>
    public class AtalayaMealPlanFare
    {
        /// <summary>
        /// The type of the room
        /// </summary>
        public RoomType Room { get; set; }
        /// <summary>
        /// The price per room per person
        /// </summary>
        public decimal Price { get; set; }
    }
}
