using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LVMS.Zipato.Model
{
    public class Scene
    {
        public Guid Uuid { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public string ClassName { get; set; }
        public string Description { get; set; }
        [JsonProperty("settings")]
        public List<SceneSettings> Settings { get; set; }
    }
}
