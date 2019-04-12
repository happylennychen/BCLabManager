using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    public enum StatusEnum
    {
        Idle,
        Using,
    }
    public class AssetClass
    {
        public StatusEnum Stauts { get; set; }

        public AssetClass()
        {
            this.Stauts = StatusEnum.Idle;
        }
    }
    public class BatteryModelClass
    {
        private static Int32 nextID = 1;
        private Int32 NextID
        {
            get
            {
                nextID += 1;
                return nextID - 1;
            }
        }
        public Int32 BatteryModelID { get; set; }
        public String Manufactor { get; set; }
        public String Name { get; set; }
        public String Material { get; set; }
        public Int32 LimitedChargeVoltage { get; set; }
        public Int32 RatedCapacity { get; set; }
        public Int32 NominalVoltage { get; set; }
        public Int32 TypicalCapacity { get; set; }
        public Int32 CutoffDischargeVoltage { get; set; }
        //public List<Battery> Batteries { get; set; }
        //public List<Program> TestPlans { get; set; }

        public BatteryModelClass(Int32 BatteryModelID, String Manufactor, String Name, String Material, Int32 LimitedChargeVoltage, Int32 RatedCapacity, Int32 NominalVoltage, Int32 TypicalCapacity, Int32 CutoffDischargeVoltage)
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
        public BatteryModelClass(String Manufactor, String Name, String Material, Int32 LimitedChargeVoltage, Int32 RatedCapacity, Int32 NominalVoltage, Int32 TypicalCapacity, Int32 CutoffDischargeVoltage)
        {
            this.BatteryModelID = NextID;
            this.Manufactor = Manufactor;
            this.Name = Name;
            this.Material = Material;
            this.LimitedChargeVoltage = LimitedChargeVoltage;
            this.RatedCapacity = RatedCapacity;
            this.NominalVoltage = NominalVoltage;
            this.TypicalCapacity = TypicalCapacity;
            this.CutoffDischargeVoltage = CutoffDischargeVoltage;
        }
    }

    public class BatteryClass : AssetClass
    {
        private static Int32 nextID = 1;
        private Int32 NextID
        {
            get
            {
                nextID += 1;
                return nextID - 1;
            }
        }
        public Int32 BatteryID { get; set; }
        public String Name { get; set; }
        public BatteryModelClass BatteryModel { get; set; }
        public Double CycleCount { get; set; }
        //public AssestStatus Stauts { get; set; }

        public BatteryClass(Int32 BatteryID, String Name, BatteryModelClass BatteryModel, Double CycleCount = 0)
        {
            this.BatteryID = BatteryID;
            this.Name = Name;
            this.BatteryModel = BatteryModel;
            this.CycleCount = CycleCount;
        }
        public BatteryClass(String Name, BatteryModelClass BatteryModel, Double CycleCount = 0)
        {
            this.BatteryID = NextID;
            this.Name = Name;
            this.BatteryModel = BatteryModel;
            this.CycleCount = CycleCount;
        }
    }


    public class TesterClass
    {
        private static Int32 nextID = 1;
        private Int32 NextID
        {
            get
            {
                nextID += 1;
                return nextID - 1;
            }
        }
        public Int32 TesterID { get; set; }
        public String Manufactor { get; set; }
        public String Name { get; set; }
        public List<TesterChannelClass> TesterChannels { get; set; }

        public TesterClass(Int32 TesterID, String Manufactor, String Name, List<TesterChannelClass> TesterChannels)
        {
            this.TesterID = TesterID;
            this.Manufactor = Manufactor;
            this.Name = Name;
            this.TesterChannels = TesterChannels;
        }
        public TesterClass(String Manufactor, String Name, List<TesterChannelClass> TesterChannels)
        {
            this.TesterID = NextID;
            this.Manufactor = Manufactor;
            this.Name = Name;
            this.TesterChannels = TesterChannels;
        }
    }
    public class TesterChannelClass : AssetClass
    {
        private static Int32 nextID = 1;
        private Int32 NextID
        {
            get
            {
                nextID += 1;
                return nextID - 1;
            }
        }
        public Int32 TesterChannelID { get; set; }
        public TesterClass Tester { get; set; }

        public TesterChannelClass(Int32 TesterChannelID, TesterClass Tester)
        {
            this.TesterChannelID = TesterChannelID;
            this.Tester = Tester;
        }
        public TesterChannelClass(TesterClass Tester)
        {
            this.TesterChannelID = NextID;
            this.Tester = Tester;
        }
    }

    public class ChamberClass : AssetClass
    {
        private static Int32 nextID = 1;
        private Int32 NextID
        {
            get
            {
                nextID += 1;
                return nextID - 1;
            }
        }
        public Int32 ChamberID { get; set; }
        public String Manufactor { get; set; }
        public String Name { get; set; }
        public String TemperatureRange { get; set; }

        public ChamberClass(Int32 ChamberID, String Manufactor, String Name, String TemperatureRange)
        {
            this.ChamberID = ChamberID;
            this.Manufactor = Manufactor;
            this.Name = Name;
            this.TemperatureRange = TemperatureRange;
        }
        public ChamberClass(String Manufactor, String Name, String TemperatureRange)
        {
            this.ChamberID = NextID;
            this.Manufactor = Manufactor;
            this.Name = Name;
            this.TemperatureRange = TemperatureRange;
        }
    }

    public static class AssetsPool
    {
        public static List<AssetClass> AllAssets { get; set; }
        public static List<AssetClass> IdleAssets { get; set; }
        public static List<AssetClass> UsingAssets { get; set; }
    }
}
