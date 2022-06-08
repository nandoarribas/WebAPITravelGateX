using System.Collections.Generic;
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

            foreach (var hotel in JsonSerializer.Deserialize<ResortHotel>(data).hotels)
            {
                hotels.Add(new Hotel()
                {
                    City = hotel.Location,
                    Code = hotel.Code,
                    Name = hotel.Name
                });
            }
            return hotels;
        }
    }
}
