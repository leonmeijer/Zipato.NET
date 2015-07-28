using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LVMS.Zipato.Model;
using PortableRest;
using Attribute = LVMS.Zipato.Model.Attribute;

namespace LVMS.Zipato
{
    public partial class ZipatoClient
    {
        Endpoint[] _cachedEndpointsList;
        Dictionary<Guid, Endpoint> _cachedEndpoints;

        public async Task<Endpoint[]> GetEndpointsAsync(Enums.EndpointGetModes loadMode = Enums.EndpointGetModes.InlcudeEndpointInfoOnly, bool allowCache = true)
        {
            CheckInitialized();

            if (allowCache && _cachedEndpointsList != null)
                return _cachedEndpointsList;

            var request = new RestRequest("endpoints", HttpMethod.Get);
            PrepareRequest(request);
            var endpoints = await _httpClient.ExecuteAsync<Endpoint[]>(request);

            if (loadMode == Enums.EndpointGetModes.IncludeFullAttributes ||
                loadMode == Enums.EndpointGetModes.IncludeFullAttributesWithValues)
            {
                var attributes = await GetAttributesFullAsync(allowCache);
                var attributesWithEndpoint = attributes.Where(a=>a.Endpoint != null && a.Endpoint.Uuid != Guid.Empty);
                Attribute[] attributeValues = null;

                if (loadMode == Enums.EndpointGetModes.IncludeFullAttributesWithValues)
                {
                    attributeValues = await GetAttributeValuesAsync();
                }

                foreach (var attribute in attributesWithEndpoint)
                {
                    var endpoint = endpoints.FirstOrDefault(e => e.Uuid.Equals(attribute.Endpoint.Uuid));
                    if (endpoint != null)
                    {
                        if (endpoint.Attributes == null)
                            endpoint.Attributes = new List<Attribute>();

                        if (attributeValues != null)
                        {
                            var attributeWithValue =
                                attributeValues.FirstOrDefault(a => a.Uuid.Equals(attribute.Uuid) && a.Value != null);
                            if (attributeWithValue != null)
                                attribute.Value = attributeWithValue.Value;
                        }

                        endpoint.Attributes.Add(attribute);
                    }
                }

            }

            if (allowCache)
                _cachedEndpointsList = endpoints;
            return endpoints;
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

        public async Task<IEnumerable<Endpoint>> GetEndpointsWithOnOffAsync(bool includeValues = false, bool allowCache = true)
        {
            CheckInitialized();

            Endpoint[] endpoints = null;
            if (includeValues)
                endpoints = await GetEndpointsAsync(Enums.EndpointGetModes.IncludeFullAttributesWithValues, allowCache);
            else
                endpoints = await GetEndpointsAsync(Enums.EndpointGetModes.IncludeFullAttributes, allowCache);

            if (endpoints == null)
                return null;

            var onOffEndpoints =
                endpoints.Where(e => e.Attributes != null  && e.Attributes.Any(a => a.Config != null && !a.Config.Hidden &&  a.Definition != null &&
                                                                                   CultureInfo.CurrentCulture
                                                                                       .CompareInfo.IndexOf(
                                                                                           a.Definition.Cluster.Trim(),
                                                                                           ZipatoClient.OnOffCluster,
                                                                                           CompareOptions.IgnoreCase) >=
                                                                                   0));
            return onOffEndpoints;
        }
    }
}
