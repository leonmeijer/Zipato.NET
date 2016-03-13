using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LVMS.Zipato.Enums
{
    public enum DeviceGetModes
    {
        /// <summary>
        /// Return only the device information such as UUID and Icon
        /// </summary>
        DeviceOnly,
        /// <summary>
        /// Include the device state
        /// </summary>
        IncludeState,
        /// <summary>
        /// Include everything ("full"), including state, config, endpoints, info and network.
        /// </summary>
        IncludeAllInfo
    }
}
