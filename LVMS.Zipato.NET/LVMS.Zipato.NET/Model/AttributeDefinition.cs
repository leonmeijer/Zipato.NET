using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class AttributeDefinition
    {
        public int Id { get; set; }
        public string Attribute { get; set; }
        public string AttributeType { get; set; }
        public string Cluster { get; set; }
        public bool Readable { get; set; }
        public bool Reportable { get; set; }
        public bool Writable { get; set; }
    }
}
