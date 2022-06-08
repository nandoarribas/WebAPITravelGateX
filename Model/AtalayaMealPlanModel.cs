using System.Collections.Generic;
using System.Text.Json;
using System.Text.Json.Serialization;
using WebAPITravelGateX.Util;

namespace WebAPITravelGateX.Model
{

    public class AtalayaMealPlans
    {

        [JsonPropertyName("meal_plans")]
        public IEnumerable<AtalayaMealPlann> Meals { get; set; }
    }

    public class AtalayaMealPlann
    {
        [JsonPropertyName("code")]
        [JsonConverter(typeof(JsonStringEnumConverter))]
        public MealPlan Code { get; set; }

        [JsonPropertyName("name")]
        public string Name { get; set; }

        [JsonPropertyName("hotel")]
        public JsonElement Hotel { get; set; }
    }
    public class AtalayaMealPlanFare
    {
        public RoomType Room { get; set; }
        public decimal Price { get; set; }
    }
}
