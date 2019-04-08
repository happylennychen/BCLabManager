﻿using System;
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
    public class RequestedProgram : Program
    { }

    public class RequestedSubProgram : SubProgram
    { }

    public class RequestedRecipe : Recipe
    { }
    // Summary:
    //     Represents a test request
    public class Request
    {
        //public Int32 ScheduledItemID { get; set; }
        public Int32 RequestID { get; set; }
        public Int32 ProgramID { get; set; }
        public Int32 BatteryID { get; set; }        //0 means the program will run on a model, otherwise means run on a battery
        public String Requester { get; set; }
        public DateTime RequestDate { get; set; }
        public RequestedProgram rqstPro { get; set; }

        public Request(Int32 RequestID, Int32 ProgramID, String Requester, DateTime RequestDate, Int32 BatteryID = 0)
        {
            this.RequestID = RequestID;
            this.ProgramID = ProgramID;
            this.Requester = Requester;
            this.RequestDate = RequestDate;
            this.BatteryID = BatteryID;
            //InitRequestedProgram();
            //Scheduler.Import(rqst);
        }
    }
}
