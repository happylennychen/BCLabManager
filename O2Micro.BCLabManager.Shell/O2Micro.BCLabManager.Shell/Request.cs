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
        private TestStatus status = TestStatus.Waiting;
        public TestStatus Status 
        {
            get
            {
                return status;
            }
            set
            {
                if (value != status)
                {
                    status = value;
                    OnRasieStatusChangedEvent();
                }
            }
        }
        public String RedoReason { get; set; }
        public BatteryClass Battery { get; set; }
        //public TesterClass Tester { get; set; }
        public TesterChannelClass TesterChannel { get; set; }
        public ChamberClass Chamber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }

        public event EventHandler StatusChanged;

        protected virtual void OnRasieStatusChangedEvent()
        {
            EventHandler handler = StatusChanged;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

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

    public class RequestedRecipeClass
    {
        public RecipeClass Recipe { get; set; }
        public Int32 Priority { get; set; }
        public List<ResultClass> Results { get; set; }
        public ResultClass ValidResult
        {
            get
            {
                return Results[Results.Count - 1];
            }
            set
            {
                Results[Results.Count - 1] = value;
            }
        }

        public RequestedRecipeClass(RecipeClass Recipe, Int32 Priority)
        {
            this.Recipe = Recipe;
            this.Priority = Priority;
            this.Results = new List<ResultClass>();
            ResultClass Result = new ResultClass();
            Result.StatusChanged += new EventHandler(Recipe.Result_StatusChanged);
            this.Results.Add(Result);
        }
    }

    public class RequestedSubProgramClass
    {
        public SubProgramClass SubProgram { get; set; }
        public Int32 Priority { get; set; }
        public List<RequestedRecipeClass> RequestedRecipes { get; set; }

        public RequestedSubProgramClass(SubProgramClass sp, Int32 Priority)
        {
            this.SubProgram = sp;
            this.Priority = Priority;
            RequestedRecipes = new List<RequestedRecipeClass>();
            foreach (var rec in sp.Recipes)
            {
                RequestedRecipeClass RequestedRecipe = new RequestedRecipeClass(rec, this.Priority);
                RequestedRecipe.ValidResult.StatusChanged += new EventHandler(Result_StatusChanged);
                RequestedRecipes.Add(RequestedRecipe);
            }
        }

        public void Result_StatusChanged(object sender, EventArgs e)
        {
            try
            {
                ResultClass Result = (ResultClass)sender;
            }
            catch
            {
                //sender is not a ResultClass type
                return;
            }
        }
    }

    public class RequestedProgramClass
    {
        public ProgramClass Program { get; set; }
        public Int32 Priority { get; set; }
        public List<RequestedSubProgramClass> RequestedSubPrograms { get; set; }

        public RequestedProgramClass(ProgramClass pro, Int32 Priority)
        {
            this.Program = pro;
            this.Priority = Priority;
            this.RequestedSubPrograms = new List<RequestedSubProgramClass>();
            foreach (var sp in pro.SubPrograms)
            {
                RequestedSubPrograms.Add(new RequestedSubProgramClass(sp, this.Priority));
            }
        }
    }

    // Summary:
    //     Represents a test request
    public class RequestClass
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
        public Int32 RequestID { get; set; }
        public ProgramClass Program { get; set; }
        public BatteryClass Battery { get; set; }        //0 means the program will run on a model, otherwise means run on a battery
        public String Requester { get; set; }
        public DateTime RequestDate { get; set; }
        public Int32 Priority { get; set; }
        public RequestedProgramClass RequestedProgram { get; set; }

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

        public RequestClass(Int32 RequestID, ProgramClass Program, String Requester, DateTime RequestDate, Int32 Priority, BatteryClass Battery = null)
        {
            this.RequestID = RequestID;
            this.Program = Program;
            this.Requester = Requester;
            this.RequestDate = RequestDate;
            this.Priority = Priority;
            this.Battery = Battery;
            this.RequestedProgram = new RequestedProgramClass(this.Program, this.Priority);
            Scheduler.ImportTasks(RequestedProgram.RequestedSubPrograms);
        }
        public RequestClass(ProgramClass Program, String Requester, DateTime RequestDate, Int32 Priority, BatteryClass Battery = null)
        {
            this.RequestID = NextID;
            this.Program = Program;
            this.Requester = Requester;
            this.RequestDate = RequestDate;
            this.Priority = Priority;
            this.Battery = Battery;
            this.RequestedProgram = new RequestedProgramClass(this.Program, this.Priority);
            Scheduler.ImportTasks(RequestedProgram.RequestedSubPrograms);
        }
    }
}
