using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace LVMS.Zipato.Model
{
    public class AlarmConfig
    {
        public string ClassName { get; set; }
        public bool Silent { get; set; }
        public bool MobilityTripOnAway { get; set; }
        
        public bool QuickArm { get; set; }
        public int AwayEntryDelay { get; set; }
        
        
        public int SirenTime { get; set; }
        
        
        public List<string> Sirens { get; set; }
        public int SirenDelay { get; set; }
        
        public string order { get; set; }
        public bool CrossZoning { get; set; }
        public string Description { get; set; }
        public string Name { get; set; }
        public int HomeExitDelay { get; set; }
        public int HomeEntryDelay { get; set; }
        public bool Mobility { get; set; }
        
        public bool AlwaysArmed { get; set; }
        
        public int AwayExitDelay { get; set; }
        [JsonProperty("emailReports")]
        public List<EmailReport> EmailReports { get; set; }
        public string status { get; set; }
        public int number { get; set; }
        [JsonProperty("voiceReports")]
        public List<VoiceReport> VoiceReports { get; set; }
        
        public string Category { get; set; }
        
        public bool Hidden { get; set; }
        public Guid Uuid { get; set; }
        public bool MobilityResetOnStateChange { get; set; }
        
        public int MobilityTime { get; set; }
    }
}
