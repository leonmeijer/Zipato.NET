using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class ScheduleConfig
    {
        public string ClassName { get; set; }
        public Guid AstroWeather { get; set; }
        public DateTime ActiveUntil { get; set; }
        public string Astro { get; set; }
        public int RepeatInterval { get; set; }
        public string Name { get; set; }
        public Guid Uuid { get; set; }
        public int AstroOffset { get; set; }
        public string CronExpression { get; set; }
        public string RepeatUnit { get; set; }
        public DateTime ActiveFrom { get; set; }
    }
}
