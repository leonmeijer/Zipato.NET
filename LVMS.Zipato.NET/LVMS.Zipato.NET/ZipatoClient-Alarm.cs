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
        AlarmPartition[] _cachedAlarmPartitionsList;
        Dictionary<Guid, AlarmPartition> _cachedAlarmPartitions;
        AlarmZone[] _cachedAlarmZonesList;

        public async Task<AlarmPartition[]> GetAlarmPartitionsAsync(bool allowCache = true)
        {
            

            if (allowCache && _cachedAlarmPartitionsList != null)
                return _cachedAlarmPartitionsList;

            var request = new RestRequest("alarm/partitions", HttpMethod.Get);
            
            var result = await _httpClient.ExecuteWithPolicyAsync<AlarmPartition[]>(this, request);

            if (allowCache || _cachedAlarmPartitionsList != null)
                _cachedAlarmPartitionsList = result;
            return result;
        }

        public async Task<AlarmPartition> GetAlarmPartitionAsync(Guid uuid, bool allowCache = true)
        {
            

            if (allowCache && _cachedEndpoints != null && _cachedEndpoints.ContainsKey(uuid))
                return _cachedAlarmPartitions[uuid];

            var request = new RestRequest("alarm/partitions/" + uuid + "?full=true", HttpMethod.Get);
            
            
            var result = await _httpClient.ExecuteWithPolicyAsync<AlarmPartition>(this, request);


            if (_cachedAlarmPartitions == null)
                _cachedAlarmPartitions = new Dictionary<Guid, AlarmPartition>();

            if (_cachedAlarmPartitions.ContainsKey(uuid))
                _cachedAlarmPartitions.Remove(uuid);
            if (allowCache)
            {
                _cachedAlarmPartitions.Add(uuid, result);
            }
            return result;
        }

        /// <summary>
        /// Finds a AlarmPartition by name. Will call GetAlarmPartitionAsync if no AlarmPartitions are loaded yet.
        /// Will use cached data whenever possible.
        /// </summary>
        /// <param name="AlarmPartitionName">AlarmPartition name</param>
        /// <returns>A AlarmPartition instance</returns>
        public async Task<AlarmPartition> GetAlarmPartitionAsync(string partitionName)
        {
            var partitions = await GetAlarmPartitionsAsync(true);
            var partition = partitions.First(e => e.Name.ToLowerInvariant() == partitionName.ToLowerInvariant());
            return partition;
        }

        public async Task<AlarmZone[]> GetAlarmZonesAsync(AlarmPartition partition, bool allowCache = true)
        {
            return await GetAlarmZonesAsync(partition.Uuid, allowCache);
        }

        public async Task<AlarmZone[]> GetAlarmZonesAsync(Guid paritionUuid, bool allowCache = true)
        {
            

            if (allowCache && _cachedAlarmZonesList != null)
                return _cachedAlarmZonesList;

            var request = new RestRequest("alarm/partitions/" + paritionUuid + "/zones", HttpMethod.Get);
            
            var result = await _httpClient.ExecuteWithPolicyAsync<AlarmZone[]>(this, request);

            if (allowCache || _cachedAlarmZonesList != null)
                _cachedAlarmZonesList = result;
            return result;
        }

        public async Task<AlarmZone[]> GetAlarmZonesWithStatusesAsync(Guid paritionUuid, bool allowCache = true)
        {
            

            var alarmZones = await GetAlarmZonesAsync(paritionUuid, allowCache);

            var request = new RestRequest("alarm/partitions/" + paritionUuid + "/zones/statuses", HttpMethod.Get);
            
            var statuses = await _httpClient.ExecuteWithPolicyAsync<AlarmZoneStatus[]>(this, request);

            foreach (var alarmZone in alarmZones)
            {
                var status = statuses.FirstOrDefault(a => a.Uuid == alarmZone.Uuid);
                if (status != null)
                    alarmZone.Status = status;
            }

            return alarmZones;
        }

        /// <summary>
        /// Returns whether a partition is ready. A partition is ready when all of its zones are in Ready state.
        /// </summary>
        /// <param name="partition">Partition</param>
        /// <param name="allowCache">Whether or not partition and/or zone information may be cached</param>
        /// <returns>True when all zones are ready, otherwise False</returns>
        public async Task<bool> IsAlarmPartitionReady(AlarmPartition partition, bool allowCache = true)
        {
            return await IsAlarmPartitionReady(partition.Uuid, allowCache);
        }

        /// <summary>
        /// Returns whether a partition is ready. A partition is ready when all of its zones are in Ready state.
        /// </summary>
        /// <param name="partitionUuid">Partition UUID</param>
        /// <param name="allowCache">Whether or not partition and/or zone information may be cached</param>
        /// <returns>True when all zones are ready, otherwise False</returns>
        public async Task<bool> IsAlarmPartitionReady(Guid partitionUuid, bool allowCache = true)
        {
            var alarmZoneStatus = await GetAlarmZonesWithStatusesAsync(partitionUuid, allowCache);
            return alarmZoneStatus.All(a => a.Status.Ready);
        }
        public async Task<bool> SetAlarmModeAsync(AlarmPartition partition, string pinCode, Enums.AlarmArmMode newStatus, string secureSessionId = null)
        {
            return await SetAlarmModeAsync(partition.Uuid, pinCode, newStatus, secureSessionId);
        }

        public async Task<bool> SetAlarmModeAsync(Guid paritionUuid, string pinCode, Enums.AlarmArmMode newStatus, string secureSessionId = null,
            bool raiseExceptionWhenNotSuccesful = true)
        {
            

            if (secureSessionId == null)
            {
                var session = await InitializeSecuritySessionAsync();
                if (!session.Success)
                    throw new Exceptions.CannotEstablishSecuritySessionException(session.Error);

                var pinLoginResponse = await LoginAlarmWithPinAsync(session.Response.SecureSessionId,
                    session.Response.Salt,
                    session.Response.Nonce, pinCode);
                if (!pinLoginResponse.Success || !pinLoginResponse.Response.Success)
                    throw new Exceptions.CannotSignInAlarmSystemWithPINException(pinLoginResponse.Response.Error);
                secureSessionId = pinLoginResponse.Response.SecureSessionId;
            }

            var alarmRequest = new AlarmRequest()
            {
                ArmMode = Enum.GetName(typeof(Enums.AlarmArmMode), newStatus),
                SecureSessionId = secureSessionId
            };            

            var request = new RestRequest("alarm/partitions/" + paritionUuid + "/setMode", HttpMethod.Post);
            
            request.ContentType = ContentTypes.Json;
            request.AddParameter(alarmRequest);
            var retVal = await _httpClient.SendWithPolicyAsync<AlarmRequestResponse>(this, request);            

            if (!retVal.HttpResponseMessage.IsSuccessStatusCode)
            {
                if (raiseExceptionWhenNotSuccesful)
                    throw new Exceptions.CannotSetAlarmStateException("Server returned: " + retVal.HttpResponseMessage.StatusCode);
                return false;
            }

            if (retVal.Content.Success)
                return true;
            else
            {
                if (raiseExceptionWhenNotSuccesful)
                    throw new Exceptions.CannotSetAlarmStateException(retVal.Content.Error);
                return false;
            }
        }
    }
}
