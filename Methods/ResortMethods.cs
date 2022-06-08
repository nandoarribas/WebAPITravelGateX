using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPITravelGateX.Model;
using WebAPITravelGateX.Util;

namespace WebAPITravelGateX.Methods
{
    public class ResortMethods
    {
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

        public async Task<List<Hotel>> RetrieveHotelMealInfo(List<Hotel> hotels, string endpoint)
        {
            return null;
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
