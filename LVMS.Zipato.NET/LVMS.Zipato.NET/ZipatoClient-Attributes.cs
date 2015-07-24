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
        Model.Attribute[] _cachedAttributesList;
        Dictionary<string, Model.Attribute> _cachedAttributes;
        public async Task<Model.Attribute[]> GetAttributesAsync(bool allowCache = true)
        {
            CheckInitialized();

            if (allowCache && _cachedAttributesList != null)
                return _cachedAttributesList;

            var request = new RestRequest("attributes", HttpMethod.Get);
            
            PrepareRequest(request);
            var result = await _httpClient.ExecuteAsync<Model.Attribute[]>(request);

            if (allowCache)
            {
                _cachedAttributesList = result;
            }
            return result;
        }

        public async Task<Model.Attribute> GetAttributeAsync(string uuid, bool allowCache = true)
        {
            CheckInitialized();

            if (allowCache && _cachedAttributes != null && _cachedAttributes.ContainsKey(uuid))
                return _cachedAttributes[uuid];

            var request = new RestRequest("attributes/" + uuid, HttpMethod.Get);

            PrepareRequest(request);
            var result = await _httpClient.ExecuteAsync<Model.Attribute>(request);

            if (allowCache)
            {
                if (_cachedAttributes == null)
                    _cachedAttributes = new Dictionary<string, Model.Attribute>();

                _cachedAttributes.Add(uuid, result);
            }
            return result;
        }
        public async Task<bool> SendStateChangeCommandByName(string endpointName, bool newState)
        {
            var endpoints = await GetEndpointsAsync(true);
            var endpoint = endpoints.First(e => e.Name == endpointName);
            return await SendStateChangeCommand(endpoint, newState);
        }            

        public async Task<bool> SendStateChangeCommand(Endpoint endpoint, bool newState)
        {
            CheckInitialized();

            if (endpoint.Attributes == null)
                endpoint = await GetEndpointAsync(endpoint.Uuid);
            if (endpoint.Attributes == null)
                throw new Exceptions.CannotChangeStateException("Couldn't retrieve attribute list for endpoint: " + endpoint.Uuid);

            var stateAttribute = endpoint.Attributes.FirstOrDefault(a => (a.Name != null && a.Name.ToLowerInvariant() == "state") ||
                (a.AttributeName != null && a.AttributeName.ToLowerInvariant() == "state"));
            if (stateAttribute == null)
                throw new Exceptions.CannotChangeStateException("Couldn't find a STATE attribute on endpoint: " + endpoint.Uuid);

            return await SendStateChangeCommand(stateAttribute.Uuid, newState);
        }

        public async Task<bool> SendStateChangeCommand(string attributeUuid, bool newState)
        {
            var request = new RestRequest("attributes/" + attributeUuid + "/value", HttpMethod.Put);
            request.ContentType = ContentTypes.PlainText;
            // TODO: Turns out PortableRest cannot do a POST or PUT with a custom body. In this case true or false in the body.
            PrepareRequest(request);            
            request.AddParameter(newState.ToString().ToLowerInvariant());
            var returnValue = await _httpClient.ExecuteAsync<object>(request);
            return true;
        }
    }
}
