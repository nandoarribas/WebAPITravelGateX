using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System.Collections.Generic;
using System.Threading.Tasks;
using WebAPITravelGateX.Methods;
using WebAPITravelGateX.Model;
using WebAPITravelGateX.Util;

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
        private readonly TravelGateMethods _travelGateMethods;
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
            _travelGateMethods = new TravelGateMethods();
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
        
            resortHotels = await _resortMethods.RetrieveHotels(resortHotels, resortAPIHotelInfo);

            hotels = (List<Hotel>)await UpdateHotelMealPlanInfo(atalayaHotels, resortHotels);
            return new Hotels()
            {
                hotels = hotels
            };
        }

        //changed to post to include am input param for the itinerary
        //Destination will receive a list of paris destination-night-meal sepparated by _ If meal is empty we consider all meal plans
        [HttpPost("itineraryCancun")]
        public async Task<Itineraries> GetItinerary([FromBody] ItineraryInfo itineraryParams)
        {
            var hotels = await GetHotelList();

            var hotelsSelection = _travelGateMethods.RetrieveHotelsByCondition(hotels.hotels, itineraryParams.DestinationNigths);

            var itinerariesResult =  _travelGateMethods.CalculateOptions(hotelsSelection, itineraryParams.Budget);
            var result = _travelGateMethods.FillItinerary(hotels.hotels, itinerariesResult);
            return result;
        }

        #region auxiliary methods
        private async Task<IEnumerable<Hotel>> UpdateHotelMealPlanInfo(List<Hotel> atalayaHotels, List<Hotel> resortHotels)
        {
            var hotels = new List<Hotel>();
            var roomsInfo  = await _atalayaMethods.RetrieveHotelRoomInfo(atalayaHotels, atalayaAPIHotelRoomInfo);
            //For filling the room info for AtalayaHotel we use the roomInfo and mealInfo together, to fill all the fields.
            hotels.AddRange(await _atalayaMethods.RetrieveHotelMealInfo(atalayaHotels, roomsInfo, atalayaAPIMealInfo));

            hotels.AddRange(await _resortMethods.RetrieveHotelMealInfo(resortHotels, resortAPIMealInfo));
            return hotels;
        }
        #endregion

    }
}
