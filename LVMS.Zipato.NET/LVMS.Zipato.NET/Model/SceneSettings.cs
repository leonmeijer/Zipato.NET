using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class SceneSettings
    {
        public Guid ClusterEndpoint { get; set; }
        public Guid Endpoint { get; set; }
        public string ClusterClass { get; set; }
        public string Attribute { get; set; }
        public Guid AttributeUuid { get; set; }
        public string Value { get; set; }
    }
}
