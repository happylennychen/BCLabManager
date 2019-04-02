using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    enum TargetType
    {
        Model,
        Battery
    }
    class Program
    {
        public Int32 ProgramID { get; set; }
        public TargetType Target = TargetType.Model;
        public Int32 BatteryModelID { get; set; }
        public String Name { get; set; }
        public String Requester { get; set; }
        public DateTime RequestDate { get; set; }
        public List<Int32> CurrentPoints { get; set; }
        public List<Double> TempPoints { get; set; }
        //public Dictionary<String, Dictionary<String, Int32>> TestItems { get; set; }
        public List<SubProgram> TestItems { get; set; }
        public TestItemScheduler Scheduler { get; set; }

        public Program(Int32 ProgramID, TargetType Target, Int32 BatteryModelID, String Name, String Requester, DateTime RequestDate, List<Int32> CurrentPoints, List<Double> TempPoints)
        {
            this.ProgramID = ProgramID;
            this.Target = Target;
            this.BatteryModelID = BatteryModelID;
            this.Name = Name;
            this.Requester = Requester;
            this.RequestDate = RequestDate;
            this.CurrentPoints = CurrentPoints;
            this.TempPoints = TempPoints;

            if (Target == TargetType.Model)
                AddTestItemsForModel();
            else
                AddTestItemsForBattery();
        }

        public void AddTestItemsForModel()   //Create Test Items based on Test Plan settings
        {
            Int32 currentItemID = TestItems.Count;
            foreach (var t in TempPoints)
            {
                foreach (var c in CurrentPoints)
                {
                    SubProgram ti = new SubProgram(TestItems.Count, ProgramID, 0, t, c);
                    TestItems.Add(ti);
                }
            }
        }

        public void AddTestItemsForBattery()   //Create Test Items based on Test Plan settings
        {
            Int32 currentItemID = TestItems.Count;
            foreach (var t in TempPoints)
            {
                foreach (var c in CurrentPoints)
                {
                    SubProgram ti = new SubProgram(TestItems.Count, ProgramID, 0, t, c);
                    TestItems.Add(ti);
                }
            }
        }
    }
}
