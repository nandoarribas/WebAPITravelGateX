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
    public class BookingController : ControllerBase
    {
        private readonly ILogger<BookingController> _logger;
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

        /// <summary>
        /// Create an instance of BookingController 
        /// </summary>
        /// <param name="logger">To include logs if required</param>
        /// <param name="configuration">To retrieve the information from appsettings.json file</param>
        public BookingController(ILogger<BookingController> logger, IConfiguration configuration)
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


        /// <summary>
        /// Get all the available hotels with their information of rooms, meal plans...
        /// </summary>
        /// <returns>A list of hotels</returns>
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

        /// <summary>
        /// Get all available itineraries that fits with the itinerary info param given
        /// </summary>
        /// <param name="itineraryParams">Represents an object with the budget and a list of destination cities with their meal plans and nights</param>
        /// <returns>The Itineraries</returns>
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
            await _atalayaMethods.RetrieveHotelRoomInfo(atalayaHotels, atalayaAPIHotelRoomInfo);
            //For filling the room info for AtalayaHotel we use the roomInfo and mealInfo together, to fill all the fields.
            hotels.AddRange(await _atalayaMethods.RetrieveHotelMealInfo(atalayaHotels, roomsInfo, atalayaAPIMealInfo));

            hotels.AddRange(await _resortMethods.RetrieveHotelMealInfo(resortHotels, resortAPIMealInfo));
            return hotels;
        }
        #endregion

    }
}
