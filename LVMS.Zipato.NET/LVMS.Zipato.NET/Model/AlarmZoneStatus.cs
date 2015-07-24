using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class AlarmZoneStatus
    {
        public bool Ready { get; set; }
        public bool Armed { get; set; }
        public bool Tripped { get; set; }
        public bool Bypassed { get; set; }
        public bool AensorState { get; set; }
        public Guid Uuid { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
