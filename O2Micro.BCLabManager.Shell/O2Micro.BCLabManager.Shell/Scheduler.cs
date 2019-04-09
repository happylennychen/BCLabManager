using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    public class TestItemScheduler
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
        public void OrderItems()
        { }
        //Summary:
        //      Auto assign assets and run
        public void AutoRun()
        {
            AutoAssignAssets();
            RunTopTasks();
        }
        public void AutoAssignAssets()
        { }
        public void RunTopTasks()
        { }
        public void CloseRunningTask()
        { 
            //ChangeRequestStatus();
            ReleaseAssets();
        }
        public void ReleaseAssets()
        { }
    }
    public class SchedulerItem
    {
        public Int32 RequestID { get; set; }
        public Int32 SubProgramID { get; set; }
        public Int32 RecipeID { get; set; }
        public TestStatus Status { get; set; }
        public String RedoReason { get; set; }
        public Int32 BatteryID { get; set; }
        public Int32 TesterID { get; set; }
        public Int32 ChannelID { get; set; }
        public Int32 ChamberID { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        //public Double StartCC { get; set; }
        //public Double EndCC { get; set; }

        public SchedulerItem(Int32 RequestID, Int32 SubProgramID, Int32 RecipeID, String RedoReason, Int32 BatteryID = 0)
        {
            this.RequestID = RequestID;
            this.SubProgramID = SubProgramID;
            this.RecipeID = RecipeID;
            this.Status = TestStatus.Invalid;       //New Item are all invalid untill been added to Queue
            this.RedoReason = RedoReason;
            this.BatteryID = BatteryID;
        }
    }
}
