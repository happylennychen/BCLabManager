using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    public class BatteryModel
    {
        public Int32 BatteryModelID { get; set; }
        public String Manufactor { get; set; }
        public String Name { get; set; }
        public String Material { get; set; }
        public Int32 LimitedChargeVoltage { get; set; }
        public Int32 RatedCapacity { get; set; }
        public Int32 NominalVoltage { get; set; }
        public Int32 TypicalCapacity { get; set; }
        public Int32 CutoffDischargeVoltage { get; set; }
        public List<Battery> Batteries { get; set; }
        public List<Program> TestPlans { get; set; }

        public BatteryModel(Int32 BatteryModelID, String Manufactor, String Name, String Material, Int32 LimitedChargeVoltage, Int32 RatedCapacity, Int32 NominalVoltage, Int32 TypicalCapacity, Int32 CutoffDischargeVoltage)
        {
            this.BatteryModelID = BatteryModelID;
            this.Manufactor = Manufactor;
            this.Name = Name;
            this.Material = Material;
            this.LimitedChargeVoltage = LimitedChargeVoltage;
            this.RatedCapacity = RatedCapacity;
            this.NominalVoltage = NominalVoltage;
            this.TypicalCapacity = TypicalCapacity;
            this.CutoffDischargeVoltage = CutoffDischargeVoltage;
        }

        public void AddTestPlan(String TestPlanName, String Requester, DateTime RequestDate, List<Double> CurrentPoints, List<Double> TempPoints)
        {
            Program tp = new Program(1, TargetType.Model, this.BatteryModelID, TestPlanName, Requester, RequestDate, CurrentPoints, TempPoints);
            TestPlans.Add(tp);
        }
    }
}
