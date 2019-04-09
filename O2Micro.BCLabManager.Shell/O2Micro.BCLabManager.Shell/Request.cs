using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    //public class Requests
    //{
    //    public List<Request> Rs { get; set; }
    //    public void SaveToDB()
    //    { }
    //    public void LoadFromDB()
    //    { }
    //}
    public class RequestedProgram : ProgramClass
    { }

    public class RequestedSubProgram : SubProgram
    { }

    public class RequestedRecipe : Recipe
    {
        public RequestedRecipe()
        { }
    }
    // Summary:
    //     Represents a test request
    public class Request
    {
        //public Int32 ScheduledItemID { get; set; }
        public Int32 RequestID { get; set; }
        public ProgramClass Program { get; set; }
        public BatteryClass Battery { get; set; }        //0 means the program will run on a model, otherwise means run on a battery
        public String Requester { get; set; }
        public DateTime RequestDate { get; set; }
        public Int32 Priority { get; set; }
        public RequestedProgram rqstPro { get; set; }

        public Int32 CurrentNumber { get; set; }
        public Int32 TemperatureNumber { get; set; }
        public Int32 PlanNumber { get; set; }
        public Int32 RedoNumber { get; set; }
        public Double RedoRate { get; set; }
        public Int32 CompletedNumber { get; set; }
        public Int32 ExecutingNumber { get; set; }
        public Int32 WaitingNumber { get; set; }
        public Int32 AbandonedNumber { get; set; }
        public Int32 FailedNumber { get; set; }
        public DateTime CompleteDate { get; set; }

        public Request(Int32 RequestID, ProgramClass Program, String Requester, DateTime RequestDate, Int32 Priority, BatteryClass Battery = null)
        {
            this.RequestID = RequestID;
            this.Program = Program;
            this.Requester = Requester;
            this.RequestDate = RequestDate;
            this.Battery = Battery;
            //InitRequestedProgram();
            //Scheduler.Import(rqst);
        }
    }
}
