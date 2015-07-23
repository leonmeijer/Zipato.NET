using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using LVMS.Zipato.Model;
using RestSharp.Portable;

namespace LVMS.Zipato
{
    public partial class ZipatoClient
    {
        public async Task<Endpoint[]> GetEndpointsAsync()
        {
            CheckInitialized();

            var request = new RestRequest("endpoints", Method.GET);
            PrepareRequest(request);
            var result = await _httpClient.Execute<Endpoint[]>(request);
            if (result.StatusCode != System.Net.HttpStatusCode.OK)
                throw new Exceptions.RequestFailedException(result.StatusCode);
            return result.Data;
        }
    }
}
