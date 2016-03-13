using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class ClusterEndpoint
    {
        public string Link { get; set; }
        public string Name { get; set; }
        public string Uuid { get; set; }
        public int? Room { get; set; }        
    }
}
