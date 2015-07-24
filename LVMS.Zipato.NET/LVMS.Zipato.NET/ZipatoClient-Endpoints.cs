using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LVMS.Zipato.Model;
using PortableRest;

namespace LVMS.Zipato
{
    public partial class ZipatoClient
    {
        Endpoint[] _cachedEndpointsList;
        Dictionary<Guid, Endpoint> _cachedEndpoints;

        public async Task<Endpoint[]> GetEndpointsAsync(bool allowCache = true)
        {
            CheckInitialized();

            if (allowCache && _cachedEndpointsList != null)
                return _cachedEndpointsList;

            var request = new RestRequest("endpoints", HttpMethod.Get);
            PrepareRequest(request);
            var result = await _httpClient.ExecuteAsync<Endpoint[]>(request);

            if (allowCache)
                _cachedEndpointsList = result;
            return result;
        }

        public async Task<Endpoint> GetEndpointAsync(Guid uuid, bool allowCache = true)
        {
            CheckInitialized();

            if (allowCache && _cachedEndpoints != null && _cachedEndpoints.ContainsKey(uuid))
                return _cachedEndpoints[uuid];

            var request = new RestRequest("endpoints/" + uuid + "?attributes=true", HttpMethod.Get);
            
            PrepareRequest(request);
            var result = await _httpClient.ExecuteAsync<Endpoint>(request);

            if (_cachedEndpoints == null)
                _cachedEndpoints = new Dictionary<Guid, Endpoint>();

            if (_cachedEndpoints.ContainsKey(uuid))
                _cachedEndpoints.Remove(uuid);
            if (allowCache)
            {
                _cachedEndpoints.Add(uuid, result);
            }
            return result;
        }

        
    }
}
