using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LVMS.Zipato.Model
{
    public class AlarmPartition
    {
        public Guid Uuid { get; set; }
        public int Room { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string Order { get; set; }
        public string Description { get; set; }
        public string[] Tags { get; set; }
        [JsonProperty("attributes")]
        public List<Model.Attribute> Attributes { get; set; }
        [JsonProperty("zones")]
        public List<AlarmZone> Zones { get; set; }
        public AlarmState State { get; set; }
        public AlarmControl Control { get; set; }
        public AlarmConfig Config { get; set; }

    }
}
