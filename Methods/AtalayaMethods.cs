using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPITravelGateX.Model;
using WebAPITravelGateX.Util;

namespace WebAPITravelGateX.Methods
{
    /// <summary>
    /// Class with all methods used in controller to retrieve info from atalaya API
    /// </summary>
    public class AtalayaMethods
    {
        /// <summary>
        /// Get all the atalaya hotels
        /// </summary>
        /// <param name="hotels">Previous existing hotels</param>
        /// <param name="endpoint">Atalaya endpoint to retrieve info</param>
        /// <returns>The updated hotel list</returns>
        public async Task<List<Hotel>> RetrieveHotels(List<Hotel> hotels, string endpoint)
        {
            var data = await Utils.GetDataFromUrl(endpoint);

            foreach (var hotel in JsonSerializer.Deserialize<AtalayaHotels>(data).hotels)
            {
                hotels.Add(new Hotel()
                {
                    City = hotel.City,
                    Code = hotel.Code,
                    Name = hotel.Name,
                    Rooms = new List<HotelRoomInfo>()
                });
            }
            return hotels;
        }

        /// <summary>
        /// Get the list of hotels with the room types available
        /// </summary>
        /// <param name="hotels">Previous existing hotels</param>
        /// <param name="endpoint">Atalaya endpoint to retrieve info</param>
        /// <returns>The updated hotels list</returns>
        public async Task<IEnumerable<AtalayaRoom>> RetrieveHotelRoomInfo(List<Hotel> hotels, string endpoint)
        {
            var data = await Utils.GetDataFromUrl(endpoint);
            return JsonSerializer.Deserialize<AtalayaRooms>(data).RoomsType;
        }

        /// <summary>
        /// Get all atalaya hotels with meal plan info filled in
        /// </summary>
        /// <param name="hotels">The previous hotels list</param>
        /// <param name="roomsInfo">The room info retrieved from the rooms endpoint</param>
        /// <param name="endpoint">Atalaya endpoint to retrieve info</param>
        /// <returns>The updated hotels list</returns>
        public async Task<List<Hotel>> RetrieveHotelMealInfo(List<Hotel> hotels, IEnumerable<AtalayaRoom> roomsInfo, string endpoint)
        {
            
            var data = await Utils.GetDataFromUrl(endpoint);
            var hotelsResult = new List<Hotel>();
            var hotelRooms = new Dictionary<string, List<HotelRoomInfo>>();

            foreach (var roomsMeal in JsonSerializer.Deserialize<AtalayaMealPlans>(data).Meals)
            {
                var mealPlan = roomsMeal.Code;
                var roomHotels = roomsMeal.Hotel.EnumerateObject();
                foreach(var hotel in roomHotels)
                {
                    var hotelCode = hotel.Name;
                    var roomFares = hotel.Value.EnumerateArray().ToList();
                    List<AtalayaMealPlanFare> atalayaRoomFareParsed = CreateMealPlanFares(roomFares);
                    var roomsInfoFilled = (from mpf in atalayaRoomFareParsed
                    select new HotelRoomInfo()
                    {
                        MealPlan = mealPlan,
                        Name = roomsInfo.Where(x=> x.Code == mpf.Room && x.Hotels.Contains(hotelCode)).Select(x=>x.Name).FirstOrDefault(),
                        Price = mpf.Price,
                        RoomType = mpf.Room
                    }).ToList();

                    if (!hotelRooms.ContainsKey(hotelCode))
                    {
                        hotelRooms.Add(hotelCode, new List<HotelRoomInfo>());
                    }
                    hotelRooms.Add(hotelCode, roomsInfoFilled);
                    
                }
            }
            hotelsResult = (from h in hotels
            select new Hotel()
            {
                City = h.City,
                Code = h.Code,
                Name = h.Name,
                Rooms = hotelRooms[h.Code]
            }).ToList();

            return hotelsResult;
        }

        private static List<AtalayaMealPlanFare> CreateMealPlanFares(List<JsonElement> roomFares)
        {
            var atalayaRoomPriceParsed = new List<AtalayaMealPlanFare>();
            foreach (var roomFare in roomFares)
            {
                atalayaRoomPriceParsed.Add(new AtalayaMealPlanFare()
                {
                    Price = roomFare.GetProperty("price").GetDecimal(),
                    Room = (RoomType)Enum.Parse(typeof(RoomType), roomFare.GetProperty("room").GetString())
                });
            }

            return atalayaRoomPriceParsed;
        }
    }
}
