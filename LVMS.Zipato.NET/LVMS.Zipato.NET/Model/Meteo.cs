using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class Meteo
    {
        public string Link { get; set; }        
        public string Location { get; set; }
        public string Query { get; set; }
        
        public Guid Uuid { get; set; }
    }
}
