using System;

namespace LVMS.Zipato.Model
{
    public class Zipabox
    {
        public string Owner { get; set; }
        public string YourRole { get; set; }
        public string Serial { get; set; }
        public string Name { get; set; }
        public string RemoteIp { get; set; }
        public string LocalIp { get; set; }
        public string Timezone { get; set; }
        public int GmtOffset { get; set; }
        public bool Online { get; set; }
        public string FirmwareVersion { get; set; }
        public bool FirmwareUpgradeAvailable { get; set; }
        public bool FirmwareUpgradeRequired { get; set; }
        public bool NeedSync { get; set; }
        public DateTime SaveDate { get; set; }
        public DateTime SyncDate { get; set; }

        public int Devices { get; set; }
        public int OfflineDevices { get; set; }
        public float TotalPower { get; set; }
        public BurglarPartition BurglarPartitions { get; set; }
        public MobilityPartition MobilityPartitions { get; set; }
        public Thermostat Thermostats { get; set; }

    }
}
