using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class AttributeValue
    {
        public string Value { get; set; }
        public DateTime Timestamp { get; set; }
        public string PendingValue { get; set; }
        public DateTime PendingTimestamp { get; set; }
    }
}
