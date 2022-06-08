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
            var endpointAtalaya = _configuration.GetSection("APIS").GetSection("atalayaAPIHotelInfo").Value;

            hotels = await _atalayaMethods.RetrieveHotels(hotels, endpointAtalaya);
            hotels = (List<Hotel>)await GetHotelRoomInfo(hotels);
            hotels = (List<Hotel>)await GetHotelMealPlanInfo(hotels);
            var endpointResort = _configuration.GetSection("APIS").GetSection("resortAPIHotelRoomInfo").Value;
            hotels = await _resortMethods.RetrieveHotels(hotels, endpointResort);
            return new Hotels()
            {
                hotels = hotels
            };
        }

        public async Task<IEnumerable<Hotel>> GetHotelRoomInfo(List<Hotel> atalayaHotels)
        {
            var hotelRoomInfo = new List<Hotel>();
            var endpointAtalaya = _configuration.GetSection("APIS").GetSection("atalayaAPIRoomInfo").Value;

            hotelRoomInfo = await _atalayaMethods.RetrieveHotelRoomInfo(atalayaHotels, endpointAtalaya);
            
            return hotelRoomInfo;
        }

        public async Task<IEnumerable<Hotel>> GetHotelMealPlanInfo(List<Hotel> atalayaHotels)
        {
            var hotelRoomInfo = new List<Hotel>();
            var endpointAtalaya = _configuration.GetSection("APIS").GetSection("atalayaAPIMealInfo").Value;

            hotelRoomInfo = await _atalayaMethods.RetrieveHotelMealInfo(atalayaHotels, endpointAtalaya);
            return hotelRoomInfo;
        }

    }
}
