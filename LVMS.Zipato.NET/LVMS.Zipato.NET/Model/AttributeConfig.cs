using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class AttributeConfig
    {
        public string Name { get; set; }
        public bool Master { get; set; }
        public bool Hidden { get; set; }
        public bool Reported { get; set; }
        public int? Expire { get; set; }
        public string Compression { get; set; }
        public string Type { get; set; }
        public string Unit { get; set; }
        
        public double? Scale { get; set; }
        public int? Precision { get; set; }
        public int? Room { get; set; }
    }
}
