using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    public enum TestStatus
    {
        Invalid,
        Waiting,
        Executing,
        Abandoned,
        Failed,
        Completed
    }
    public class ResultClass
    {
        public TestStatus Status { get; set; }
        public String RedoReason { get; set; }
        public BatteryClass Battery { get; set; }
        //public TesterClass Tester { get; set; }
        public TesterChannelClass TesterChannel { get; set; }
        public ChamberClass Chamber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public ResultClass()
        {
            this.Status = TestStatus.Waiting;
            this.RedoReason = String.Empty;
            this.Battery = null;
            this.TesterChannel = null;
            this.Chamber = null;
            //this.StartTime = null;
            //this.EndTime = DateTime.;
        }
    }
    //public class Requests
    //{
    //    public List<Request> Rs { get; set; }
    //    public void SaveToDB()
    //    { }
    //    public void LoadFromDB()
    //    { }
    //}
    public class RequestedProgram
    {
        public Int32 ProgramID { get; set; }
        public List<RequestedSubProgram> RequestedSubPrograms { get; set; }

        public RequestedProgram(ProgramClass pro)
        {
            this.ProgramID = pro.ProgramID;
            foreach (var sp in pro.SubPrograms)
            {
                RequestedSubPrograms.Add(new RequestedSubProgram(sp));
            }
        }
    }

    public class RequestedSubProgram
    {
        public Int32 SubProgramID { get; set; }
        public List<RequestedRecipe> RequestedRecipes { get; set; }

        public RequestedSubProgram(SubProgram sp)
        {
            this.SubProgramID = sp.SubProgramID;
            foreach (var rec in sp.Recipes)
            {
                RequestedRecipes.Add(new RequestedRecipe(rec));
            }
        }
    }

    public class RequestedRecipe
    {
        public Int32 RecipeID { get; set; }
        public List<ResultClass> Results { get; set; }

        public RequestedRecipe(Recipe rec)
        {
            this.RecipeID = rec.RecipeID;
            this.Results.Add( new ResultClass());
        }
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
            this.rqstPro = new RequestedProgram(this.Program);
            //InitRequestedProgram();
            //Scheduler.Import(rqst);
        }

        //private void InitRequestedProgram()
        //{
        //    List<RequestedRecipe> RequestedRecipes = new List<RequestedRecipe>();
        //    List<RequestedSubProgram> RequestedSubPrograms = new List<RequestedSubProgram>();
 
        //    foreach (var sp in Program.SubPrograms)
        //    {
        //        foreach (var rec in sp.Recipes)
        //        {
        //            RequestedRecipes.Add(new RequestedRecipe(rec));
        //        }
        //        RequestedSubPrograms.Add(new RequestedSubProgram(sp));
        //    }
        //    rqstPro = new RequestedProgram(Program);
        //}
    }
}
