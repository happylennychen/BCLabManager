using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    public enum ExecutorStatus
    {
        Waiting,
        Abandoned,
        Ready,
        Executing,
        Failed,
        Completed,
        Invalid,
    }

    public class ExecutorClass
    {
        public RequestedRecipeClass RequestedRecipe { get; set; }
        private ExecutorStatus status = ExecutorStatus.Waiting;
        public ExecutorStatus Status 
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

        public ExecutorClass(RequestedRecipeClass RequestedRecipe)
        {
            this.RequestedRecipe = RequestedRecipe;
            this.Status = ExecutorStatus.Waiting;
            this.RedoReason = String.Empty;
            this.Battery = null;
            this.TesterChannel = null;
            this.Chamber = null;
            //this.StartTime = null;
            //this.EndTime = DateTime.;
        }

        public void AssignAssets(BatteryClass Battery, ChamberClass Chamber, TesterChannelClass TesterChannel)
        {
            if (this.Status == ExecutorStatus.Waiting)
            {
                this.Battery = Battery;
                this.Chamber = Chamber;
                this.TesterChannel = TesterChannel;
                this.Status = ExecutorStatus.Ready;
            }
        }

        public void Execute()
        {
            if (this.Status == ExecutorStatus.Waiting)
            {
                Battery.Stauts = StatusEnum.Using;
                if(Chamber != null)
                    Chamber.Stauts = StatusEnum.Using;
                TesterChannel.Stauts = StatusEnum.Using;
                Status = ExecutorStatus.Executing;
            }
        }

        public void Commit(ExecutorStatus Status, DateTime StartTime, DateTime EndTime, String RedoReason = "")  //Need to check the Executor Status to make sure it is a executing
        {
            if (this.Status == ExecutorStatus.Executing)
            {
                this.RedoReason = RedoReason;
                this.StartTime = StartTime;
                this.EndTime = EndTime;
                if (Chamber != null)
                    Chamber.Stauts = StatusEnum.Idle;
                TesterChannel.Stauts = StatusEnum.Idle;
                Battery.Stauts = StatusEnum.Idle;
                this.Status = Status;   //this has to be the last assignment because it will raise StatusChanged event so that duration will be updated using StartTime and EndTime
            }
        }

        public void Abandon()
        {
            Status = ExecutorStatus.Abandoned;
        }

        public void Invalid()
        {
            Status = ExecutorStatus.Invalid;
        }
    }

    public class RequestedRecipeClass
    {
        public RequestedSubProgramClass RequestedSubProgram { get; set; }
        public RecipeClass Recipe { get; set; }
        public Int32 Priority { get; set; }
        public List<ExecutorClass> Executors { get; set; }
        public ExecutorClass ValidExecutor
        {
            get
            {
                return Executors[Executors.Count - 1];
            }
            set
            {
                Executors[Executors.Count - 1] = value;
            }
        }

        public RequestedRecipeClass(RequestedSubProgramClass RequestedSubProgram, RecipeClass Recipe, Int32 Priority)
        {
            this.RequestedSubProgram = RequestedSubProgram;
            this.Recipe = Recipe;
            this.Priority = Priority;
            this.Executors = new List<ExecutorClass>();
            AddExecutor();
        }

        public void AddExecutor()
        {
            ExecutorClass Executor = new ExecutorClass(this);
            Executor.StatusChanged += new EventHandler(Recipe.Executor_StatusChanged);
            this.Executors.Add(Executor);
        }
    }

    public class RequestedSubProgramClass
    {
        public RequestedProgramClass RequestedProgram { get; set; }
        public SubProgramClass SubProgram { get; set; }
        public Int32 Priority { get; set; }
        private List<RequestedRecipeClass> requestedRecipes = new List<RequestedRecipeClass>();
        public List<RequestedRecipeClass> RequestedRecipes 
        {
            get
            {
                return requestedRecipes;
            }
            set
            {
                requestedRecipes = value;
            }
        }
        public RequestedRecipeClass TopRequestedRecipe 
        {
            get 
            {
                foreach (var rec in requestedRecipes)
                {
                    if (rec.ValidExecutor.Status == ExecutorStatus.Waiting)
                        return rec;
                }
                return null;    //No valid Executor, we may consider to throw exception here
            }
            set
            {
                RequestedRecipeClass RequestedRecipe;// = requestedRecipes.Where(o=>o.ValidExecutor.Status == TestStatus.Waiting).ToArray()[0];
                foreach (var rec in requestedRecipes)
                {
                    if (rec.ValidExecutor.Status == ExecutorStatus.Waiting)
                        RequestedRecipe = rec;
                }
                RequestedRecipe = value;
            }
        }

        public RequestedSubProgramClass(RequestedProgramClass RequestedProgram, SubProgramClass sp, Int32 Priority)
        {
            this.RequestedProgram = RequestedProgram;
            this.SubProgram = sp;
            this.Priority = Priority;
            //RequestedRecipes = new List<RequestedRecipeClass>();
            foreach (var rec in sp.Recipes)
            {
                RequestedRecipeClass RequestedRecipe = new RequestedRecipeClass(this, rec, this.Priority);
                RequestedRecipe.ValidExecutor.StatusChanged += new EventHandler(Executor_StatusChanged);
                RequestedRecipes.Add(RequestedRecipe);
            }
        }

        public void Executor_StatusChanged(object sender, EventArgs e)
        {
            try
            {
                ExecutorClass Executor = (ExecutorClass)sender;
                switch (Executor.Status)
                {
                    case ExecutorStatus.Invalid:
                        foreach (var recipe in RequestedRecipes)
                        {
                            recipe.AddExecutor();
                        }
                        if (!Scheduler.WaitingRequestedSubPrograms.Contains(this))
                        {
                            Scheduler.ImportTasks(new List<RequestedSubProgramClass> { this });
                        }
                        break;
                }
            }
            catch
            {
                //sender is not a ExecutorClass type
                return;
            }
        }
    }

    public class RequestedProgramClass
    {
        public RequestClass Request { get; set; }
        public ProgramClass Program { get; set; }
        public Int32 Priority { get; set; }
        public List<RequestedSubProgramClass> RequestedSubPrograms { get; set; }

        public RequestedProgramClass(RequestClass Request, ProgramClass pro, Int32 Priority)
        {
            this.Program = pro;
            this.Priority = Priority;
            this.RequestedSubPrograms = new List<RequestedSubProgramClass>();
            foreach (var sp in pro.SubPrograms)
            {
                RequestedSubPrograms.Add(new RequestedSubProgramClass(this, sp, this.Priority));
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
        public BatteryClass Battery { get; set; }        //null means the program will run on a model, otherwise means run on a battery
        public String Requester { get; set; }
        public DateTime RequestDate { get; set; }
        public Int32 Priority { get; set; }
        public RequestedProgramClass RequestedProgram { get; set; }

        public Int32 SubProgramNumber 
        {
            get
            {
                return RequestedProgram.RequestedSubPrograms.Count;
            }
        }
        public Int32 RedoNumber { get; set; }
        public Double RedoRate { get { return RedoNumber / SubProgramNumber; } }
        public Int32 CompletedNumber 
        {
            get
            {
                return 
                    (
                    from subpro in RequestedProgram.RequestedSubPrograms
                    from recipe in subpro.RequestedRecipes
                    from executor in recipe.Executors
                    where executor.Status == ExecutorStatus.Completed
                    select executor
                    ).Count();
            }
        }
        public Int32 ExecutingNumber
        {
            get
            {
                return
                    (
                    from subpro in RequestedProgram.RequestedSubPrograms
                    from recipe in subpro.RequestedRecipes
                    from executor in recipe.Executors
                    where executor.Status == ExecutorStatus.Executing
                    select executor
                    ).Count();
            }
        }
        public Int32 WaitingNumber
        {
            get
            {
                return
                    (
                    from subpro in RequestedProgram.RequestedSubPrograms
                    from recipe in subpro.RequestedRecipes
                    from executor in recipe.Executors
                    where executor.Status == ExecutorStatus.Waiting
                    select executor
                    ).Count();
            }
        }
        public Int32 AbandonedNumber
        {
            get
            {
                return
                    (
                    from subpro in RequestedProgram.RequestedSubPrograms
                    from recipe in subpro.RequestedRecipes
                    from executor in recipe.Executors
                    where executor.Status == ExecutorStatus.Waiting
                    select executor
                    ).Count();
            }
        }
        public Int32 FailedNumber
        {
            get
            {
                return
                    (
                    from subpro in RequestedProgram.RequestedSubPrograms
                    from recipe in subpro.RequestedRecipes
                    from executor in recipe.Executors
                    where executor.Status == ExecutorStatus.Failed
                    select executor
                    ).Count();
            }
        }
        public DateTime CompleteDate { get; set; }

        public RequestClass(Int32 RequestID, ProgramClass Program, String Requester, DateTime RequestDate, Int32 Priority, BatteryClass Battery = null)
        {
            this.RequestID = RequestID;
            this.Program = Program;
            this.Requester = Requester;
            this.RequestDate = RequestDate;
            this.Priority = Priority;
            this.Battery = Battery;
            this.RequestedProgram = new RequestedProgramClass(this, this.Program, this.Priority);
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
            this.RequestedProgram = new RequestedProgramClass(this, this.Program, this.Priority);
            Scheduler.ImportTasks(RequestedProgram.RequestedSubPrograms);
        }
    }
}
