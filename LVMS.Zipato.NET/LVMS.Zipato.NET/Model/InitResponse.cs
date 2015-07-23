using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class InitResponse
    {
        public bool Success { get; set; }
        public string JSessionId { get; set; }
        public string Nonce { get; set; }
    }
}
