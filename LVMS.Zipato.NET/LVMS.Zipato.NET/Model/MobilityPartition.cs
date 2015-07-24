using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class MobilityPartition
    {
        public int Count { get; set; }
        public Mode Modes { get; set; }
        public int Tripped { get; set; }
    }
}
