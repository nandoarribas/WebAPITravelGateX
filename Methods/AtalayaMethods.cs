using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using WebAPITravelGateX.Model;
using WebAPITravelGateX.Util;

namespace WebAPITravelGateX.Methods
{
    public class AtalayaMethods
    {
        public async Task<List<Hotel>> RetrieveHotels(List<Hotel> hotels, string endpoint)
        {
            var client = new HttpClient();
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

        public async Task<List<Hotel>> RetrieveHotelRoomInfo(List<Hotel> hotels, string endpoint)
        {
            var client = new HttpClient();
            var data = await Utils.GetDataFromUrl(endpoint);
            foreach (var roomType in JsonSerializer.Deserialize<AtalayaRooms>(data).tipoHabitaciones)
            {
                var addHotels = from h in hotels where roomType.Hotels.Contains(h.Code)
                                select h;
                foreach(var hotel in addHotels)
                {
                    hotel.Rooms = hotel.Rooms.Append(new HotelRoomInfo()
                    {
                        Name= roomType.Name,
                        RoomType = roomType.Code
                    });
                }
            }
            return hotels;
        }

        public async Task<List<Hotel>> RetrieveHotelMealInfo(List<Hotel> hotels, string endpoint)
        {
            var client = new HttpClient();
            var data = await Utils.GetDataFromUrl(endpoint);
            var hotelesSalida = new List<Hotel>();
            foreach (var roomsMeal in JsonSerializer.Deserialize<AtalayaMealPlans>(data).Comidas)
            {
                var mealPlan = roomsMeal.Code;
                var roomHotels = roomsMeal.Hotel.EnumerateObject();
                foreach(var hotel in roomHotels)
                {
                    var hotelCode = hotel.Name;
                    var roomPrices = hotel.Value.EnumerateArray().ToList();
                    var atalayaRoomPriceParsed = new List<AtalayaMealPlanFare>();
                    foreach (var precioHabitacionPorPersona in roomPrices)
                    {
                        atalayaRoomPriceParsed.Add(new AtalayaMealPlanFare()
                        {
                            Price = precioHabitacionPorPersona.GetProperty("price").GetDecimal(),
                            Room = (Habitacion)Enum.Parse(typeof(Habitacion),precioHabitacionPorPersona.GetProperty("room").GetString())
                        });
                    }

                    var addHotels = from h in hotels
                                    where hotelCode == h.Code
                                    select new Hotel()
                                    {
                                        City = h.City,
                                        Code = h.Code,
                                        Name = h.Name,
                                        Rooms = FillTarifasHabitaciones(h.Rooms, atalayaRoomPriceParsed, mealPlan)
                                    };
                    hotelesSalida.AddRange(addHotels);
                }
            }
            return hotelesSalida;
        }

        private IEnumerable<HotelRoomInfo> FillTarifasHabitaciones(IEnumerable<HotelRoomInfo> rooms, IEnumerable<AtalayaMealPlanFare> tarifas, Regimenes mealplan )
        {
            var hotelRoomInfoResult = new List<HotelRoomInfo>();
            foreach (var roomInfo in rooms)
            {
                var tarifasTipoHabitacion =
                    from t in tarifas
                    where roomInfo.RoomType == t.Room
                    select new HotelRoomInfo()
                    {
                        Name = roomInfo.Name,
                        RoomType = roomInfo.RoomType,
                        MealPlan = mealplan,
                        Price = t.Price
                    };
                hotelRoomInfoResult.AddRange(tarifasTipoHabitacion);

            }
            return hotelRoomInfoResult;
        }
    }
}
