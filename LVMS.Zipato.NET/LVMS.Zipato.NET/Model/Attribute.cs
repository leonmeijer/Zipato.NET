using System;

namespace LVMS.Zipato.Model
{
    public class Attribute
    {
        public Guid Uuid { get; set; }
        public string Link { get; set; }
        public string Name { get; set; }
        public int AttributeId { get; set; }
        public string AttributeName { get; set; }
        
        public string Order { get; set; }
        public string[] Tags { get; set; }
        public AttributeValue Value { get; set; }

      
        public AttributeDefinition Definition { get; set; }
        public AttributeConfig Config { get; set; }
        public ClusterEndpoint ClusterEndpoint { get; set; }
        public Endpoint Endpoint { get; set; }
        public Device Device { get; set; }
        public Network Network { get; set; }       
        
        public bool? ShowIcon { get; set; }
        public Room Room { get; set; }
        public UiType UIType { get; set; }
    }
}
