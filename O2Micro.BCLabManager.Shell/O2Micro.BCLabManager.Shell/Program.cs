using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    public class Program
    {
        public List<Program> Ps { get; set; }
        public void SaveToDB()
        { }
        public void LoadFromDB()
        { }
    }
    // Summary:
    //     Represents a test program which can be requested and executed over and over again
    class Program
    {
        public Int32 ProgramID { get; set; }
        public Int32 BatteryModelID { get; set; }
        public String Name { get; set; }
        public List<SubProgram> SubPrograms { get; set; }

        public Program(Int32 ProgramID, Int32 BatteryModelID, String Name, List<SubProgram> SubPrograms)
        {
            this.ProgramID = ProgramID;
            this.BatteryModelID = BatteryModelID;
            this.Name = Name;
            this.SubPrograms = SubPrograms;
        }
    }
}
