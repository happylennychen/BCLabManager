using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    public class Battery
    {
        public Int32 BatteryID { get; set; }
        public String Name { get; set; }
        public Int32 BatteryModelID { get; set; }
        public Double CycleCount { get; set; }

        public Battery(Int32 BatteryID, String Name, Int32 BatteryModelID, Double CycleCount)
        {
            this.BatteryID = BatteryID;
            this.Name = Name;
            this.BatteryModelID = BatteryModelID;
            this.CycleCount = CycleCount;
        }
    }
}
