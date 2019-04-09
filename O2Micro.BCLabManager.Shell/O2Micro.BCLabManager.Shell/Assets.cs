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
    //public class BatteryModels
    //{
    //    public List<BatteryModel> BMs { get; set; }
    //    public void SaveToDB()
    //    { }
    //    public void LoadFromDB()
    //    { }
    //}


    // Summary:
    //     Represents a certain battery type
    public class BatteryModelClass
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
    }

    //public class Batteries
    //{
    //    public List<Battery> Bs { get; set; }
    //    public void SaveToDB()
    //    { }
    //    public void LoadFromDB()
    //    { }
    //}
    // Summary:
    //     Represents a specific battery pack
    public class BatteryClass : AssetClass
    {
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
    }


    public class TesterClass
    {
        public Int32 TesterID { get; set; }
        public String Manufactor { get; set; }
        public String Name { get; set; }
        public List<TesterChannel> TesterChannels { get; set; }

        public TesterClass(Int32 TesterID, String Manufactor, String Name, List<TesterChannel> TesterChannels)
        {
            this.TesterID = TesterID;
            this.Manufactor = Manufactor;
            this.Name = Name;
            this.TesterChannels = TesterChannels;
        }
    }
    public class TesterChannel : AssetClass
    {
        public Int32 TesterChannelID { get; set; }

        public TesterChannel(Int32 TesterChannelID)
        {
            this.TesterChannelID = TesterChannelID;
        }
    }

    public class ChamberClass : AssetClass
    {
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
    }
}
