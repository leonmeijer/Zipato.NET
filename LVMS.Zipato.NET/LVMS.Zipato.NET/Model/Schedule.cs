using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LVMS.Zipato.Model
{
    public class Schedule
    {
        public Guid Uuid { get; set; }
        public string Name { get; set; }
        public string Link { get; set; }
        [JsonProperty("config")]
        public ScheduleConfig Config { get; set; }
    }
}
