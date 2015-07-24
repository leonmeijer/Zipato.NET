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
        Endpoint[] _cachedEndpoints;
        Dictionary<Guid, Endpoint> _cachedEndpoints2;

        public async Task<Endpoint[]> GetEndpointsAsync(bool allowCache = true)
        {
            CheckInitialized();

            if (allowCache && _cachedEndpoints != null)
                return _cachedEndpoints;

            var request = new RestRequest("endpoints", HttpMethod.Get);
            PrepareRequest(request);
            var result = await _httpClient.ExecuteAsync<Endpoint[]>(request);

            if (allowCache)
                _cachedEndpoints = result;
            return result;
        }

        public async Task<Endpoint> GetEndpointAsync(Guid uuid, bool allowCache = true)
        {
            CheckInitialized();

            if (allowCache && _cachedEndpoints2 != null && _cachedEndpoints2.ContainsKey(uuid))
                return _cachedEndpoints2[uuid];

            var request = new RestRequest("endpoints/" + uuid + "?attributes=true", HttpMethod.Get);
            
            PrepareRequest(request);
            var result = await _httpClient.ExecuteAsync<Endpoint>(request);

            if (allowCache)
            {
                if (_cachedEndpoints2 == null)
                    _cachedEndpoints2 = new Dictionary<Guid, Endpoint>();

                _cachedEndpoints2.Add(uuid, result);
            }
            return result;
        }

        
    }
}
