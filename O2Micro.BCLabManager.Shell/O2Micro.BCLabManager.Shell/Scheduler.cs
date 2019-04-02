using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    class TestItemScheduler
    {
        private Dictionary<Int32, SchedulerItem> fixedItems;
        public Dictionary<Int32, SchedulerItem> FixedItems
        {
            get { return fixedItems; }
            set
            {
                fixedItems = value;
                schedulerItems = null;
            }
        }

        private Dictionary<Int32, SchedulerItem> orderableItems;
        public Dictionary<Int32, SchedulerItem> OrderableItems
        {
            get { return orderableItems; }
            set
            {
                orderableItems = value;
                schedulerItems = null;
            }
        }

        private Dictionary<Int32, SchedulerItem> schedulerItems;
        public Dictionary<Int32, SchedulerItem> SchedulerItems
        {
            get 
            {
                if (schedulerItems == null)
                {
                    //Todo: Combine FixedItems and OrderableItems, then copy into schedulerItems
                }
                return schedulerItems;
            }
        }
    }
    public enum TestStatus
    {
        Waiting,
        Executing,
        Abandoned,
        Failed,
        Completed
    }
    class SchedulerItem
    {
        //public Int32 ScheduledItemID { get; set; }
        public Int32 TestItemID { get; set; }
        public TestStatus Status { get; set; }
        public String RedoReason { get; set; }
        public Int32 BatteryID { get; set; }
        public Int32 TesterID { get; set; }
        public Int32 ChannelID { get; set; }
        public Int32 ChamberID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public Double StartCC { get; set; }
        public Double EndCC { get; set; }

        public SchedulerItem(Int32 TestItemID, TestStatus Status, String RedoReason, Int32 BatteryID = 0)
        {
            this.TestItemID = TestItemID;
            this.Status = Status;
            this.RedoReason = RedoReason;
            this.BatteryID = BatteryID;
        }
    }
}
