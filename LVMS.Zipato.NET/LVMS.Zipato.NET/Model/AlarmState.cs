using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class AlarmState
    {
        public string ArmMode { get; set; }
        public DateTime BoxTimeStamp { get; set; }
        public DateTime TimeStamp { get; set; }
        public bool Tripped { get; set; }
    }
}
