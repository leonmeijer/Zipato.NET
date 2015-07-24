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
        public string Room { get; set; }
        public string Order { get; set; }
        public string[] Tags { get; set; }
        public AttributeValue Value { get; set; }
    }
}
