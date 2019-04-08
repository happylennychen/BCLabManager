using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    class Chamber
    {
        public Int32 ChamberID { get; set; }
        public String Manufactor { get; set; }
        public String Name { get; set; }
        public String TemperatureRange { get; set; }
        public AssestStatus Stauts { get; set; }
    }
}
