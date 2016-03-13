using System.Collections.Generic;
using Newtonsoft.Json;

namespace LVMS.Zipato.Model
{
    public class Device
    {
        public string Link { get; set; }
        public string Name { get; set; }
        public string Uuid { get; set; }
        public int? Room { get; set; }
        public string Description { get; set; }
        public DeviceState State { get; set; }
        [JsonProperty("endpoints")]
        public List<Endpoint>  Endpoints { get; set; }
    }
}
