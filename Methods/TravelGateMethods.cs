using System;
using System.Collections.Generic;
using System.Linq;
using WebAPITravelGateX.Model;

namespace WebAPITravelGateX.Methods
{
    public class TravelGateMethods
    {
        public Dictionary<string, List<HotelRoomInfo>> RetrieveHotelsByCondition(IEnumerable<Hotel> hotels, List<Destinations> destinations)
        {
            var filteredCityHotel = new Dictionary<string, List<HotelRoomInfo>>();
            foreach (var city in destinations)
            {
                var hotelsResult = (from h in hotels where city.Destination == h.City select h).ToList();

                foreach (var hotelResult in hotelsResult)
                {
                    city.Nights = city.Nights <= 0 ? 1 : city.Nights;
                    var filteredRooms = (from h in hotelResult.Rooms
                                  where city.MealPlan != "" ? city.MealPlan == h.MealPlan.ToString() : 1 == 1
                                  select new HotelRoomInfo()
                                  {
                                      MealPlan = h.MealPlan,
                                      Name = h.Name,
                                      Nights = city.Nights,
                                      Price = city.Nights * h.Price * 2,
                                      RoomType = h.RoomType,
                                      HotelCode = hotelResult.Code //Useful to build the json output 
                                  }).ToList();

                    if (filteredCityHotel.ContainsKey(city.Destination))
                    {
                        filteredCityHotel[city.Destination].AddRange(filteredRooms);
                    }
                    else
                    {
                        filteredCityHotel.Add(city.Destination, (List<HotelRoomInfo>)filteredRooms);
                    }
                }
            }
            return filteredCityHotel;
        }


        public IEnumerable<IEnumerable<HotelRoomInfo>> CalculateOptions(Dictionary<string, List<HotelRoomInfo>> hotelsSelection, decimal budget)
        {
            //The calculation criteria option depends on the following variables : First Price, Snd RoomType (suite,standard)
            //We can also order by meal plan 
            var destination = hotelsSelection.First();

            var city = destination.Key;

            var hotelRoomsResult = new List<List<HotelRoomInfo>>();
            var orderedHotelsSelection = new Dictionary<string, List<HotelRoomInfo>>();
            foreach (var availableRooms in hotelsSelection)
            {
                orderedHotelsSelection.Add(availableRooms.Key, availableRooms.Value.OrderByDescending(x => x.MealPlan).ThenByDescending(x => x.RoomType).ToList());
            }
            foreach (var cartesianValue in CartesianProduct(orderedHotelsSelection.Values, budget))
            {
                if (FitsInBudget(cartesianValue, budget))
                {
                    hotelRoomsResult.Add(cartesianValue.ToList());
                }
            }
            //If we have more than one result, return only the first 2 options
            if(hotelRoomsResult.Count()>1)
            {
                hotelRoomsResult.RemoveRange(2, hotelRoomsResult.Count() - 2);
            }
            return hotelRoomsResult;
        }

        public Itineraries FillItinerary(IEnumerable<Hotel> hotels, IEnumerable<IEnumerable<HotelRoomInfo>> hotelItineraries)
        {
            var result = new Itineraries();
            var itineraries = new List<Itinerary>();
            foreach (var itineraryResult in hotelItineraries)
            {
                var hotelList = new List<Hotel>();
                foreach (var hotelRoom in itineraryResult)
                {
                    var hotel = hotels.Where(x => x.Code == hotelRoom.HotelCode).FirstOrDefault();
                    hotelList.Add(Create(hotel,hotelRoom));
                }
                itineraries.Add(new Itinerary()
                {
                    Hotels = hotelList
                });
            }
            result.ItinerariesList = itineraries;
            return result;
        }

        #region auxiliary methods
        private bool FitsInBudget(IEnumerable<HotelRoomInfo> roomCombination, decimal budget)
        {
            return roomCombination.Sum(x => x.Price) < budget;
        }

        private IEnumerable<IEnumerable<T>> CartesianProduct<T>(IEnumerable<IEnumerable<T>> sequences, decimal budget)
        {
            IEnumerable<IEnumerable<T>> emptyProduct =
              new[] { Enumerable.Empty<T>() };
            var roomCombinations = sequences.Aggregate(
              emptyProduct,
              (accumulator, sequence) =>
                from accseq in accumulator
                from item in sequence
                select accseq.Concat(new[] { item }));

            return roomCombinations;
        }

        /// <summary>
        /// Creates an copy of an hotel with the same base properties and the roomInfo
        /// </summary>
        /// <param name="hotelBase"></param>
        /// <returns></returns>
        private Hotel Create(Hotel hotelBase, HotelRoomInfo room)
        {
            return new Hotel()
            {
                City = hotelBase.City,
                Code = hotelBase.Code,
                Name = hotelBase.Name,
                Room = room
            };
        }
        #endregion
    }
}
