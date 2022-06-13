using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPITravelGateX.Model;
using WebAPITravelGateX.Util;

namespace WebAPITravelGateX.Methods
{
    public class ResortMethods: IHotelProvider
    {
        /// <summary>
        /// Get the list of the hotels available for resort hotels
        /// </summary>
        /// <param name="hotels">The previous list with hotels retrieved</param>
        /// <param name="endpoint">The endpoint to get the data into the resort API</param>
        /// <returns>The updated list of hotels</returns>
        public async Task<List<Hotel>> RetrieveHotels(List<Hotel> hotels, string endpoint)
        {
            var data = await Utils.GetDataFromUrl(endpoint);

            foreach (var hotel in JsonSerializer.Deserialize<ResortHotels>(data).hotels)
            {
                hotels.Add(new Hotel()
                {
                    City = hotel.Location,
                    Code = hotel.Code,
                    Name = hotel.Name,
                    Rooms = ParseRoomInfo(hotel.Rooms)
                });
            }
            return hotels;
        }

        /// <summary>
        /// Get all the hotels with the meal plan filled in
        /// </summary>
        /// <param name="hotels">The previous hotel list without meal plan</param>
        /// <param name="endpoint">The endpoint to retrieve meal plans from resort API</param>
        /// <returns>The updated list of hotels with meal plans</returns>
        public async Task<List<Hotel>> RetrieveHotelMealInfo(List<Hotel> hotels, string endpoint)
        {
            var data = await Utils.GetDataFromUrl(endpoint);
            var hotelResult = new List<Hotel>();
            var mealPlansList = JsonSerializer.Deserialize<ResortMealPlans>(data).MealPlans;
            
            foreach(var hotel in hotels)
            {
                hotel.Rooms = FillResortRooms(hotel.Rooms, from meal in mealPlansList where meal.HotelCode == hotel.Code select meal);
            }
            
            return hotels;
        }

        private IEnumerable<HotelRoomInfo> FillResortRooms(IEnumerable<HotelRoomInfo> rooms, IEnumerable<ResortMealPlan> hotelMealPlan)
        {
            var roomInfo = new List<HotelRoomInfo>();
            foreach(var room in rooms)
            {
                var roomsMealPlan = (from meal in hotelMealPlan where room.RoomType == ConvertRoomType(meal.RoomType) 
                    select new HotelRoomInfo()
                    {
                        Name = room.Name,
                        RoomType = room.RoomType,
                        MealPlan= meal.Code,
                        Price = meal.Price
                    });
                roomInfo.AddRange(roomsMealPlan);
            }
            return roomInfo;
        }

        private RoomType ConvertRoomType(ResortRoomType resortRoomType)
        {
            var roomType = RoomType.standard;
            switch(resortRoomType)
            {
                case ResortRoomType.st:
                    roomType= RoomType.standard;
                    break;
                case ResortRoomType.su:
                    roomType= RoomType.suite;
                    break;
            }
            return roomType;
        }

        private IEnumerable<HotelRoomInfo> ParseRoomInfo(IEnumerable<ResortHotelRoom> resortRooms)
        {
            var rooms = from room in resortRooms
                        select new HotelRoomInfo()
                        {
                            Name = room.Name,
                            RoomType = room.Code

                        };
            return rooms;
        }
    }
}
