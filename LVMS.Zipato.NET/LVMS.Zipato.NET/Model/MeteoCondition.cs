using System.Collections.Generic;
using Newtonsoft.Json;

namespace LVMS.Zipato.Model
{
    public class MeteoConditions
    {
        public string Link { get; set; }
        [JsonProperty("attributes")]
        public List<Model.Attribute> Attributes { get; set; }
        public string Category { get; set; }
        public string ClusterClass { get; set; }        
        public string Uuid { get; set; }        
    }
}
