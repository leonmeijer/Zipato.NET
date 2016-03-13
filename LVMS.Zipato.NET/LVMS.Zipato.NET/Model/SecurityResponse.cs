using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class SecurityResponse
    {
        public bool Success { get; set; }
        public SecurityUpdateItem Response { get; set; }
        public bool Retry { get; set; }
        public string Ref { get; set; }
        public string Error { get; set; }
        public string[] Errors { get; set; }
    }
}
