using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LVMS.Zipato.Model
{
    public class Endpoint
    {
        public Guid Uuid {get;set;}
        public string Link { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Room { get; set; }
        [JsonProperty("attributes")]
        public List<Attribute> Attributes { get; set; }
        public bool ShowIcon { get; set; }

    }

    
}
