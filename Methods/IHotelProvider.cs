using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebAPITravelGateX.Model;

namespace WebAPITravelGateX.Methods
{
    public interface IHotelProvider
    {
        Task<List<Hotel>> RetrieveHotels(List<Hotel> hotels, string endpoint);
    }
}
