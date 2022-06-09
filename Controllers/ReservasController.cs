using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
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

        public ReservasController(ILogger<ReservasController> logger, IConfiguration configuration)
        {
            _logger = logger;
            _configuration = configuration;
            _atalayaMethods = new AtalayaMethods();
            _resortMethods = new ResortMethods();
        }

        [HttpGet("hotelInfo")]
        public async Task<Hotels> GetFullHotelInfo()
        {
            var hotels = new List<Hotel>();
            var atalayaHotels = new List<Hotel>();
            var resortHotels = new List<Hotel>();
            var endpointAtalaya = _configuration.GetSection("APIS").GetSection("atalayaAPIHotelInfo").Value;

            atalayaHotels = await _atalayaMethods.RetrieveHotels(atalayaHotels, endpointAtalaya);
            atalayaHotels = (List<Hotel>)await GetHotelRoomInfo(atalayaHotels);
            var endpointResort = _configuration.GetSection("APIS").GetSection("resortAPIHotelRoomInfo").Value;
            resortHotels = await _resortMethods.RetrieveHotels(resortHotels, endpointResort);
            //Until we dont have hotels set we cant get mealplan
            hotels = (List<Hotel>)await GetHotelMealPlanInfo(atalayaHotels, resortHotels);
            return new Hotels()
            {
                hotels = hotels
            };
        }

        private async Task<IEnumerable<Hotel>> GetHotelRoomInfo(List<Hotel> atalayaHotels)
        {
            var hotelRoomInfo = new List<Hotel>();
            var endpointAtalaya = _configuration.GetSection("APIS").GetSection("atalayaAPIRoomInfo").Value;

            hotelRoomInfo = await _atalayaMethods.RetrieveHotelRoomInfo(atalayaHotels, endpointAtalaya);
            return hotelRoomInfo;
        }

        private async Task<IEnumerable<Hotel>> GetHotelMealPlanInfo(List<Hotel> atalayaHotels, List<Hotel> resortHotels)
        {
            var hotels = new List<Hotel>();
            var endpointAtalaya = _configuration.GetSection("APIS").GetSection("atalayaAPIMealInfo").Value;

            hotels.AddRange(await _atalayaMethods.RetrieveHotelMealInfo(atalayaHotels, endpointAtalaya));
            var endpointResort = _configuration.GetSection("APIS").GetSection("resortAPIMealInfo").Value;
            hotels.AddRange(await _resortMethods.RetrieveHotelMealInfo(resortHotels, endpointResort));
            return hotels;
        }

    }
}
