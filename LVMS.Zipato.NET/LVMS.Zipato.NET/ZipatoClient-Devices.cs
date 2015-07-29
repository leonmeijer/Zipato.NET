using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using LVMS.Zipato.Enums;
using LVMS.Zipato.Model;
using PortableRest;

namespace LVMS.Zipato
{
    public partial class ZipatoClient
    {
        Device[] _cachedDevicesList;
        Dictionary<Guid, Device> _cachedDevices;
        private Enums.EndpointGetModes _cachedLoadMode = EndpointGetModes.None;

        public async Task<Device[]> GetDevicesAsync(bool includeDeviceState = false, Enums.EndpointGetModes loadMode = EndpointGetModes.None, bool allowCache = true)
        {
            if (loadMode == EndpointGetModes.IncludeEndpointInfoOnly)
                throw new ArgumentException("IncludeEndpointInfoOnly is not supported because it doesn't contain information to correlate devices to endpoints. Please specify IncludeFullAttributes.");

            CheckInitialized();

            if (allowCache && _cachedDevicesList != null && _cachedLoadMode == loadMode)
                return _cachedDevicesList;

            var request = new RestRequest("devices", HttpMethod.Get);
            PrepareRequest(request);
            var devices = await _httpClient.ExecuteAsync<Device[]>(request);

            if (includeDeviceState)
            {
                // Do a seperate call to get all device statuses and correlate device state with devices
                // based on uuid
                var stateRequest = new RestRequest("devices/statuses", HttpMethod.Get);
                PrepareRequest(stateRequest);
                var deviceStatuses = await _httpClient.ExecuteAsync<Device[]>(stateRequest);

                foreach (var device in devices)
                {
                    var deviceState = deviceStatuses.FirstOrDefault(d => d.Uuid.Equals(device.Uuid));
                    if (deviceState != null)
                        device.State = deviceState.State;
                }
            }

            if (loadMode == EndpointGetModes.IncludeFullAttributes ||
                loadMode == EndpointGetModes.IncludeFullAttributesWithValues)
            {
                // Do seperate calls to get endpoint, endpoint attributes and maybe even attribute values
                // and correlate them to devices
                var endpoints = await GetEndpointsAsync(loadMode, allowCache);
                foreach (var device in devices)
                {
                    var deviceWithEndpoints =
                        endpoints.Where(
                            e =>
                                e.Attributes != null &&
                                e.Attributes.Any(a => a.Device != null && a.Device.Uuid.Equals(device.Uuid))).ToArray();
                    if (deviceWithEndpoints.Length > 0)
                    {
                        if (device.Endpoints == null)
                            device.Endpoints = new List<Endpoint>();
                        device.Endpoints.AddRange(deviceWithEndpoints);
                    }
                }
            }


            if (allowCache)
            {
                _cachedDevicesList = devices;
                _cachedLoadMode = loadMode;
            }
            return devices;
        }

        public async Task<Device[]> GetDevicesOfflineAsync(bool excludeDevicesWithoutEndpoints = true, bool allowCache = true)
        {
            CheckInitialized();

            var devices = await GetDevicesAsync(true, EndpointGetModes.IncludeFullAttributes, allowCache);
            if (devices == null)
                return null;
            devices = devices.Where(d => d.State != null && !d.State.Online).ToArray();
            if (excludeDevicesWithoutEndpoints)
                devices = devices.Where(d => d.Endpoints != null && d.Endpoints.Count > 0).ToArray();
            return devices;
        }

        public async Task<Device> GetDeviceAsync(Guid uuid, DeviceGetModes getMode = DeviceGetModes.DeviceOnly, bool allowCache = true)
        {
            CheckInitialized();

            if (allowCache && _cachedDevices != null && _cachedDevices.ContainsKey(uuid))
                return _cachedDevices[uuid];

            string resource = "devices/" + uuid;
            if (getMode == DeviceGetModes.IncludeState)
                resource += "?state=true";
            if (getMode == DeviceGetModes.IncludeAllInfo)
                resource += "?full=true";

            var request = new RestRequest(resource, HttpMethod.Get);

            PrepareRequest(request);
            var result = await _httpClient.ExecuteAsync<Device>(request);

            if (_cachedDevices == null)
                _cachedDevices = new Dictionary<Guid, Device>();

            if (_cachedDevices.ContainsKey(uuid))
                _cachedDevices.Remove(uuid);
            if (allowCache)
            {
                _cachedDevices.Add(uuid, result);
            }
            return result;
        }


    }
}
