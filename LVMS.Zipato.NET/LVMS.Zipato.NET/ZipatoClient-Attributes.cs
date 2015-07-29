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
        Dictionary<Guid, Model.Attribute> _cachedAttributes;
        bool _cachedListIncludesAttributes;

        public async Task<Model.Attribute[]> GetAttributesAsync(bool allowCache = true)
        {
            

            if (allowCache && _cachedAttributesList != null)
                return _cachedAttributesList;

            var request = new RestRequest("attributes", HttpMethod.Get);
            
            
            var result = await _httpClient.ExecuteWithPolicyAsync<Model.Attribute[]>(this, request);

            if (allowCache)
            {
                _cachedAttributesList = result;
                _cachedListIncludesAttributes = false;
            }
            return result;
        }

        public async Task<Model.Attribute[]> GetAttributesFullAsync(bool allowCache = true)
        {
            

            if (allowCache && _cachedAttributesList != null && _cachedListIncludesAttributes)
                return _cachedAttributesList;

            var request = new RestRequest("attributes/full?full=true", HttpMethod.Get);

            
            var result = await _httpClient.ExecuteWithPolicyAsync<Model.Attribute[]>(this, request);

            if (allowCache)
            {
                _cachedAttributesList = result;
                _cachedListIncludesAttributes = true;
            }
            return result;
        }

        public async Task<Model.Attribute> GetAttributeAsync(Guid uuid, bool allowCache = true)
        {
            

            if (allowCache && _cachedAttributes != null && _cachedAttributes.ContainsKey(uuid))
                return _cachedAttributes[uuid];

            var request = new RestRequest("attributes/" + uuid, HttpMethod.Get);

            
            var result = await _httpClient.ExecuteWithPolicyAsync<Model.Attribute>(this, request);

            if (_cachedAttributes == null)
                _cachedAttributes = new Dictionary<Guid, Model.Attribute>();

            if (_cachedAttributes.ContainsKey(uuid))
                _cachedAttributes.Remove(uuid);
            if (allowCache)
            {
                _cachedAttributes.Add(uuid, result);
            }
            return result;
        }

        /// <summary>
        /// Finds an endpoint by name. Will call GetEndpointsAsync if no endpoints are loaded yet.
        /// Will use cached data whenever possible.
        /// </summary>
        /// <param name="endpointName">Endpoint name</param>
        /// <returns>An Endpoint instance</returns>
        public async Task<Endpoint> GetEndpointAsync(string endpointName, Enums.EndpointGetModes loadMode = Enums.EndpointGetModes.IncludeEndpointInfoOnly)
        {
            var endpoints = await GetEndpointsAsync(loadMode);
            var endpoint = endpoints.First(e => e.Name == endpointName);
            return endpoint;
        }

        /// <summary>
        /// Send a command to change the state of an endpoint. This method will seach for an endpoint
        /// with given name. If the endpoint was found, an attribute named state is searched and finally
        /// a command will be send to Zipato to change the state for that attribute UUID.
        /// </summary>
        /// <param name="endpointName">Endpoint name</param>
        /// <param name="newState">The new state</param>
        /// <returns>True when the request was executed succesful, otherwise False</returns>
        public async Task<bool> SetOnOffStateAsync(string endpointName, bool newState)
        {
            Endpoint endpoint = await GetEndpointAsync(endpointName);
            return await SetOnOffStateAsync(endpoint, newState);
        }

        /// <summary>
        /// Send a command to change the state of an endpoint.
        /// </summary>
        /// <param name="attributeUuid">Attribute UUID</param>
        /// <param name="newState">The new state</param>
        /// <returns>True when the request was executed succesful, otherwise False</returns>
        public async Task<bool> SetOnOffStateAsync(Guid attributeUuid, bool newState)
        {
            

            // Send a PUT request with 'true' or 'false' as plain-text in the body.
            // Had to modify PortableRest package to support this.
            var request = new RestRequest("attributes/" + attributeUuid + "/value", HttpMethod.Put);
            request.ContentType = ContentTypes.PlainText;
            
            request.AddParameter(newState.ToString().ToLowerInvariant());
            var returnValue = await _httpClient.ExecuteWithPolicyAsync<object>(this, request);
            return true;
        }

        /// <summary>
        /// Send a command to change the state of an endpoint. This method will search for an attribute
        /// named state in the endpoint's attributes list and will use that attribute UUID.
        /// </summary>
        /// <param name="endpoint">Endpoint</param>
        /// <param name="newState">The new state</param>
        /// <returns>True when the request was executed succesful, otherwise False</returns>
        public async Task<bool> SetOnOffStateAsync(Endpoint endpoint, bool newState)
        {
                       

            Model.Attribute stateAttribute = await GetAttributeAsync(endpoint, Enums.CommonAttributeNames.STATE);
            return await SetOnOffStateAsync(stateAttribute.Uuid, newState);
        }


        /// <summary>
        /// Send a command to change the position of an endpoint. This method will seach for an endpoint
        /// with given name. If the endpoint was found, an attribute named state is searched and finally
        /// a command will be send to Zipato to change the state for that attribute UUID.
        /// </summary>
        /// <param name="endpointName">Endpoint name</param>
        /// <param name="position">The new position</param>
        /// <returns>True when the request was executed succesful, otherwise False</returns>
        public async Task<bool> SetPositionAsync(string endpointName, int position)
        {
            Endpoint endpoint = await GetEndpointAsync(endpointName);
            return await SetPositionAsync(endpoint, position);
        }

        /// <summary>
        /// Send a command to change the position of an endpoint.
        /// </summary>
        /// <param name="attributeUuid">Attribute UUID</param>
        /// <param name="position">The new position</param>
        /// <returns>True when the request was executed succesful, otherwise False</returns>
        public async Task<bool> SetPositionAsync(Guid attributeUuid, int position)
        {
            

            // Send a PUT request as plain-text in the body.
            // Had to modify PortableRest package to support this.
            var request = new RestRequest("attributes/" + attributeUuid + "/value", HttpMethod.Put);
            request.ContentType = ContentTypes.PlainText;
            
            request.AddParameter(position.ToString().ToLowerInvariant());
            var returnValue = await _httpClient.ExecuteWithPolicyAsync<object>(this, request);
            return true;
        }

        /// <summary>
        /// Send a command to change the position of an endpoint. This method will search for an attribute
        /// named POSITION in the endpoint's attributes list and will use that attribute UUID.
        /// </summary>
        /// <param name="endpoint">Endpoint</param>
        /// <param name="position">The new position</param>
        /// <returns>True when the request was executed succesful, otherwise False</returns>
        public async Task<bool> SetPositionAsync(Endpoint endpoint, int position)
        {
            

            Model.Attribute stateAttribute = await GetAttributeAsync(endpoint, Enums.CommonAttributeNames.POSITION);
            return await SetPositionAsync(stateAttribute.Uuid, position);
        }


        /// <summary>
        /// Finds an attribute with a given name on an endpoint
        /// </summary>
        /// <param name="endpoint">Endpoint instance</param>
        /// <param name="attributeName">Attribute name</param>
        /// <returns>Attribute instance</returns>
        private async Task<Model.Attribute> GetAttributeAsync(Endpoint endpoint, Enums.CommonAttributeNames attribute)
        {
            string attributeName = Enum.GetName(typeof(Enums.CommonAttributeNames), attribute);
            return await GetAttributeAsync(endpoint, attributeName);
        }

        /// <summary>
        /// Finds an attribute with a given name on an endpoint
        /// </summary>
        /// <param name="endpoint">Endpoint instance</param>
        /// <param name="attributeName">Attribute name</param>
        /// <returns>Attribute instance</returns>
        private async Task<Model.Attribute> GetAttributeAsync(Endpoint endpoint, string attributeName)
        {
            if (endpoint.Attributes == null)
                endpoint = await GetEndpointAsync(endpoint.Uuid);
            if (endpoint.Attributes == null)
                throw new Exceptions.CannotChangeStateException("Couldn't retrieve attribute list for endpoint: " + endpoint.Uuid);

            var stateAttribute = endpoint.Attributes.FirstOrDefault(a => (a.Name != null && a.Name.ToLowerInvariant() == attributeName.ToLowerInvariant()) ||
                            (a.AttributeName != null && a.AttributeName.ToLowerInvariant() == attributeName.ToLowerInvariant()));
            if (stateAttribute == null)
                throw new Exceptions.CannotChangeStateException("Couldn't find an attribute with name '" +attributeName + "' on endpoint: " + endpoint.Uuid);
            return stateAttribute;
        }

        

        /// <summary>
        /// Get the ON/OFF state of an endpoint
        /// </summary>
        /// <param name="endpointName">Name of the endpoint. The endpoint must have an attribute named state</param>
        /// <returns>True when the endpoint if on / enabled, otherwise False</returns>
        public async Task<bool> GetOnOffStateAsync(string endpointName)
        {
            Endpoint endpoint = await GetEndpointAsync(endpointName);
            return await GetOnOffStateAsync(endpoint);
        }


        /// <summary>
        /// Get the ON/OFF state of an endpoint
        /// </summary>
        /// <param name="endpoint">Endpoint which must have an attribute named state</param>
        /// <returns>True when the endpoint if on / enabled, otherwise False</returns>
        public async Task<bool> GetOnOffStateAsync(Endpoint endpoint)
        {
            

            Model.Attribute stateAttribute = await GetAttributeAsync(endpoint, Enums.CommonAttributeNames.STATE);
            return await GetOnOffStateAsync(stateAttribute.Uuid);
        }

        /// <summary>
        /// Get the ON/OFF state of an endpoint
        /// </summary>
        /// <param name="attributeUuid">Attribute UUID</param>
        /// <returns>True when the endpoint if on / enabled, otherwise False</returns>
        public async Task<bool> GetOnOffStateAsync(Guid attributeUuid)
        {
            

            var request = new RestRequest("attributes/" + attributeUuid + "/value", HttpMethod.Get);

            
            var result = await _httpClient.ExecuteWithPolicyAsync<Model.AttributeValue>(this, request);
            return bool.Parse(result.Value);
        }

        public async Task<Model.Attribute[]> GetAttributeValuesAsync()
        {
            

            var request = new RestRequest("attributes/values", HttpMethod.Get);

            
            return await _httpClient.ExecuteWithPolicyAsync<Model.Attribute[]>(this, request);
        }

        /// <summary>
        /// Get the value of an attribute
        /// </summary>
        /// <param name="attributeUuid">Attribute UUID</param>
        /// <returns>The value of the attribute, converted to T</returns>
        public async Task<T> GetAttributeValueAsync<T>(Model.Attribute attribute)
        {
            

            return await GetAttributeValueAsync<T>(attribute.Uuid);
        }

        /// <summary>
        /// Get the value of an attribute
        /// </summary>
        /// <param name="attributeUuid">Attribute UUID</param>
        /// <returns>The value of the attribute, converted to T</returns>
        public async Task<T> GetAttributeValueAsync<T>(Guid attributeUuid)
        {
            

            var request = new RestRequest("attributes/" + attributeUuid + "/value", HttpMethod.Get);

            
            var result = await _httpClient.ExecuteWithPolicyAsync<Model.AttributeValue>(this, request);
            return Utils.ChangeType<T>(result.Value);
        }

        /// <summary>
        /// Get the value of an attribute
        /// </summary>
        /// <param name="endpoint">The endpoint to retrieve the attribute from</param>
        /// <param name="attribute">The attribute to read</param>
        /// <returns>The value of the attribute, converted to T</returns>
        public async Task<T> GetAttributeValueAsync<T>(Endpoint endpoint, Enums.CommonAttributeNames attribute)
        {
            

            string attributeName = Enum.GetName(typeof(Enums.CommonAttributeNames), attribute);
            return await GetAttributeValueAsync<T>(endpoint, attributeName);
        }

        /// <summary>
        /// Get the value of an attribute
        /// </summary>
        /// <param name="endpoint">The endpoint to retrieve the attribute from</param>
        /// <param name="attribute">The attribute to read</param>
        /// <returns>The value of the attribute, converted to T</returns>
        public async Task<T> GetAttributeValueAsync<T>(Endpoint endpoint, string attributeName)
        {
            
            var attribute = await GetAttributeAsync(endpoint, attributeName);
            return await GetAttributeValueAsync<T>(attribute);
        }

        /// <summary>
        /// Get the value of an attribute
        /// </summary>
        /// <param name="endpoint">The endpoint to retrieve the attribute from</param>
        /// <param name="attribute">The attribute to read</param>
        /// <returns>The value of the attribute, converted to T</returns>
        public async Task<T> GetAttributeValueAsync<T>(string endpointName, Enums.CommonAttributeNames attribute)
        {
            

            Endpoint endpoint = await GetEndpointAsync(endpointName);
            return await GetAttributeValueAsync<T>(endpoint, attribute);
        }

        /// <summary>
        /// Get the value of an attribute
        /// </summary>
        /// <param name="endpoint">The endpoint to retrieve the attribute from</param>
        /// <param name="attribute">The attribute to read</param>
        /// <returns>The value of the attribute, converted to T</returns>
        public async Task<T> GetAttributeValueAsync<T>(string endpointName, string attributeName)
        {
            
            Endpoint endpoint = await GetEndpointAsync(endpointName);
            return await GetAttributeValueAsync<T>(endpoint, attributeName);
        }
    }
}
