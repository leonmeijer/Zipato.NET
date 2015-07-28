using System;

namespace LVMS.Zipato.Model
{
    public class DeviceState
    {
        public DateTime? Timestamp { get; set; }
        public bool Trouble { get; set; }
        public string Modem { get; set; }
        /// <summary>
        /// Sometimes is a 0, sometimes a DateTime string such as 2015-07-28T18:47:14Z
        /// </summary>
        public object ReceiveTimestamp { get; set; }
        /// <summary>
        /// Sometimes is a 0, sometimes a DateTime string such as 2015-07-28T18:47:14Z
        /// </summary>
        public object BatteryTimestamp { get; set; }
        public int BatteryLevel { get; set; }
        /// <summary>
        /// Sometimes is a 0, sometimes a DateTime string such as 2015-07-28T18:47:14Z
        /// </summary>
        public object SentTimestamp { get; set; }
        public string OnlineState { get; set; }
        public bool Started { get; set; }
        public bool MainsPower { get; set; }
        public bool Online { get; set; }
    }
}
