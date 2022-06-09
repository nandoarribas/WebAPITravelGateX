using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPITravelGateX.Methods;
using WebAPITravelGateX.Model;

namespace WebAPITravelGateX.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReservasController : ControllerBase
    {
        private readonly ILogger<ReservasController> _logger;
        private readonly IConfiguration _configuration;
        private readonly AtalayaMethods _atalayaMethods;
        private readonly ResortMethods _resortMethods;
        #region endpoints
        private readonly string atalayaAPIHotelInfo;
        private readonly string atalayaAPIHotelRoomInfo;
        private readonly string atalayaAPIMealInfo;
        private readonly string resortAPIHotelInfo;
        private readonly string resortAPIMealInfo;
        #endregion

        public ReservasController(ILogger<ReservasController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _atalayaMethods = new AtalayaMethods();
            _resortMethods = new ResortMethods();
            atalayaAPIHotelInfo = _configuration.GetSection("APIS").GetSection("atalayaAPIHotelInfo").Value;
            resortAPIHotelInfo = _configuration.GetSection("APIS").GetSection("resortAPIHotelRoomInfo").Value;
            atalayaAPIHotelRoomInfo = _configuration.GetSection("APIS").GetSection("atalayaAPIRoomInfo").Value;
            atalayaAPIMealInfo= _configuration.GetSection("APIS").GetSection("atalayaAPIMealInfo").Value;
            resortAPIMealInfo= _configuration.GetSection("APIS").GetSection("resortAPIMealInfo").Value;
        }

        [HttpGet("hotelList")]
        public async Task<Hotels> GetHotelList()
        {
            var hotels = new List<Hotel>();
            var atalayaHotels = new List<Hotel>();
            var resortHotels = new List<Hotel>();
            
            atalayaHotels = await _atalayaMethods.RetrieveHotels(atalayaHotels, atalayaAPIHotelInfo);
            atalayaHotels = (List<Hotel>)await _atalayaMethods.RetrieveHotelRoomInfo(atalayaHotels, atalayaAPIHotelRoomInfo);

            resortHotels = await _resortMethods.RetrieveHotels(resortHotels, resortAPIHotelInfo);

            hotels = (List<Hotel>)await UpdateHotelMealPlanInfo(atalayaHotels, resortHotels);
            return new Hotels()
            {
                hotels = hotels
            };
        }

        [HttpGet("itineraryCancun")]
        public async Task<Itineraries> GetItinerary()
        {
            var hotels = await GetHotelList();
            var itin = new Itineraries();
            var itineraries = new List<Itinerary>();
            itineraries.Add(new Itinerary()
            {
                Hoteles = ((List<Hotel>)(hotels.hotels)).GetRange(0, 1)
            });
            itineraries.Add(new Itinerary()
            {
                Hoteles = ((List<Hotel>)(hotels.hotels)).GetRange(2, 1)
            });
            itin.ItinerariesList = itineraries;

            return itin;
        }

        #region auxiliary methods
        private async Task<IEnumerable<Hotel>> UpdateHotelMealPlanInfo(List<Hotel> atalayaHotels, List<Hotel> resortHotels)
        {
            var hotels = new List<Hotel>();
            hotels.AddRange(await _atalayaMethods.RetrieveHotelMealInfo(atalayaHotels, atalayaAPIMealInfo));

            hotels.AddRange(await _resortMethods.RetrieveHotelMealInfo(resortHotels, resortAPIMealInfo));
            return hotels;
        }
        #endregion

    }
}
