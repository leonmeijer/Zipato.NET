using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class EndpointConfig
    {
        public string ClassName { get; set; }
        
        
        public string GenericDevClass { get; set; }
        
        public string Description { get; set; }
        public string Name { get; set; }
        public bool OptionalFunc { get; set; }
        
        public bool Mute { get; set; }
        
        public string Status { get; set; }
        
        public int MultiInstanceId { get; set; }
        
        public int EpId { get; set; }
        public string SpecificDevClass { get; set; }
        public bool MainEndpoint { get; set; }
        public int GroupId { get; set; }
        
        public bool HiddenRules { get; set; }
        public string Category { get; set; }
        public bool Hidden { get; set; }
        public bool IgnoreAssoc { get; set; }
        public string Uuid { get; set; }
        public object Room { get; set; }
    }

}
