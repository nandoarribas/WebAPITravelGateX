using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace WebAPITravelGateX.Util
{
    public static class Utils
    {
        /// <summary>
        /// Retrieve the info from the API given
        /// </summary>
        /// <param name="endpointResort">API to be queried</param>
        /// <returns>The json data </returns>
        public static async Task<string> GetDataFromUrl(string endpointResort)
        {
            HttpClient client = new HttpClient();
            HttpResponseMessage responseResort = await client.GetAsync(endpointResort);
            responseResort.EnsureSuccessStatusCode();
            string responseResortBody = await responseResort.Content.ReadAsStringAsync();
            return responseResortBody;
        }

    }
}
