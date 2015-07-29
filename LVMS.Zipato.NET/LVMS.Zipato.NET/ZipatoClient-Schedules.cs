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
        Schedule[] _cachedSchedulesList;
        Dictionary<Guid, Schedule> _cachedSchedules;
        /// <summary>
        /// Returns a list of schedules
        /// </summary>
        /// <param name="allowCache"></param>
        /// <returns></returns>
        public async Task<Schedule[]> GetSchedulesAsync(bool allowCache = true)
        {
            

            if (allowCache && _cachedSchedulesList != null)
                return _cachedSchedulesList;

            var request = new RestRequest("schedules", HttpMethod.Get);
            
            var result = await _httpClient.ExecuteWithPolicyAsync<Schedule[]>(this, request);

            if (allowCache || _cachedSchedulesList != null)
                _cachedSchedulesList = result;
            return result;
        }

        /// <summary>
        /// Returns a schedule including the schedule configuration.
        /// </summary>
        /// <param name="uuid">UUID of the schedule</param>
        /// <param name="allowCache"></param>
        /// <returns></returns>
        public async Task<Schedule> GetScheduleAsync(Guid uuid, bool allowCache = true)
        {
            

            if (allowCache && _cachedSchedules != null && _cachedSchedules.ContainsKey(uuid))
                return _cachedSchedules[uuid];

            var request = new RestRequest("schedules/" + uuid + "?full=true", HttpMethod.Get);
            
            
            var result = await _httpClient.ExecuteWithPolicyAsync<Schedule>(this, request);


            if (_cachedSchedules == null)
                _cachedSchedules = new Dictionary<Guid, Schedule>();

            if (_cachedSchedules.ContainsKey(uuid))
                _cachedSchedules.Remove(uuid);
            if (allowCache)
            {
                _cachedSchedules.Add(uuid, result);
            }
            return result;
        }

        /// <summary>
        /// Finds a schedule by name. Will call GetScheduleAsync if no schedules are loaded yet.
        /// Will use cached data whenever possible.
        /// </summary>
        /// <param name="sceneName">Schedule name</param>
        /// <returns>A Schedule instance</returns>
        public async Task<Schedule> GetScheduleAsync(string scheduleName)
        {
            var schedules = await GetSchedulesAsync(true);
            var scedule = schedules.First(e => e.Name == scheduleName);
            return scedule;
        }        
    }
}
