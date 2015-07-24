using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Model
{
    public class AlarmRequestResponse
    {
        public bool Success { get; set; }
        public string Error { get; set; }
        public bool Tripped { get; set; }
    }
}
