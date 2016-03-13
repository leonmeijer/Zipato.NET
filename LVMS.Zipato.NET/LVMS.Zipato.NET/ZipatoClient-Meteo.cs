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
        MeteoConditions _cachedMeteoConditions;

        /// <summary>
        /// Returns a list of meteo information
        /// </summary>
        /// <returns></returns>
        public async Task<Meteo[]> GetMeteoAsync()
        {
            var request = new RestRequest("meteo", HttpMethod.Get);
            
            var result = await _httpClient.ExecuteWithPolicyAsync<Meteo[]>(this, request);
            return result;
        }

        public async Task<MeteoConditions> GetMeteoConditionsAsync(Meteo meteo, bool allowCache = true)
        {
            return await GetMeteoConditionsAsync(meteo.Uuid, allowCache);
        }

        /// <summary>
        /// Returns a list of meteo conditions
        /// </summary>
        /// <param name="uuid">UUID of the meteo condition</param>
        /// <param name="allowCache"></param>
        /// <returns></returns>
        public async Task<MeteoConditions> GetMeteoConditionsAsync(Guid uuid, bool allowCache = true)
        {
            if (allowCache && _cachedMeteoConditions != null)
                return _cachedMeteoConditions;

            var request = new RestRequest("meteo/" + uuid + "/conditions", HttpMethod.Get);
            
            var result = await _httpClient.ExecuteWithPolicyAsync<MeteoConditions>(this, request);

            if (allowCache || _cachedMeteoConditions != null)
                _cachedMeteoConditions = result;
            return result;
        }

        public async Task<MeteoConditions> GetMeteoConditionsWithValuesAsync(bool allowCache = true)
        {
            var meteo = await GetMeteoAsync();
            if (meteo == null || meteo.Length == 0)
                return null;
            return await GetMeteoConditionsWithValuesAsync(meteo[0].Uuid, allowCache);
        }

        public async Task<MeteoConditions> GetMeteoConditionsWithValuesAsync(Meteo meteo, bool allowCache = true)
        {
            return await GetMeteoConditionsWithValuesAsync(meteo.Uuid, allowCache);
        }

        public async Task<MeteoConditions> GetMeteoConditionsWithValuesAsync(Guid uuid, bool allowCache = true)
        {
            
            var meteoConditions = await GetMeteoConditionsAsync(uuid, allowCache);
            var request = new RestRequest("meteo/attributes/values?update=false", HttpMethod.Get);

            
            var attributeValues = await _httpClient.ExecuteWithPolicyAsync<Model.Attribute[]>(this, request);

            foreach (Model.Attribute attribute in meteoConditions.Attributes)
            {
                var attributeValue = attributeValues.FirstOrDefault(a => a.Uuid == attribute.Uuid);
                if (attributeValue != null)
                {
                    // Note that not all conditions received are in the attribute value list
                    attribute.Value = attributeValue.Value;
                }
            }


            return meteoConditions;
        }
             
    }
}
