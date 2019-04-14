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
        public BatteryClass Battery { get; set; }
        //public TesterClass Tester { get; set; }
        public TesterChannelClass TesterChannel { get; set; }
        public ChamberClass Chamber { get; set; }
        public DateTime StartTime { get; set; }
        public DateTime EndTime { get; set; }
        public String Description { get; set; }

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
            this.Description = String.Empty;
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
                if(this.RequestedRecipe.Recipe.ChamberRecipe!=null) //If there is no chamber recipe, then we don't need to assign chamber at all.
                    this.Chamber = Chamber;
                this.TesterChannel = TesterChannel;
                this.Status = ExecutorStatus.Ready;
            }
        }

        public void Execute()
        {
            if (this.Status == ExecutorStatus.Ready)
            {
                Battery.Status = AssetStatusEnum.Using;
                if(Chamber != null)
                    Chamber.Status = AssetStatusEnum.Using;
                TesterChannel.Status = AssetStatusEnum.Using;
                Status = ExecutorStatus.Executing;
            }
        }

        public void Commit(ExecutorStatus Status, DateTime StartTime, DateTime EndTime, String Description = "")  //Need to check the Executor Status to make sure it is executing
        {
            if (this.Status == ExecutorStatus.Executing)
            {
                this.Description = Description;
                this.StartTime = StartTime;
                this.EndTime = EndTime;
                if (Chamber != null)
                    Chamber.Status = AssetStatusEnum.Idle;
                TesterChannel.Status = AssetStatusEnum.Idle;
                Battery.Status = AssetStatusEnum.Idle;
                if (Status == ExecutorStatus.Invalid || Status == ExecutorStatus.Completed)
                {
                    this.Status = Status;   //this has to be the last assignment because it will raise StatusChanged event so that duration will be updated using StartTime and EndTime
                }
            }
        }

        public void Abandon()
        {
            if (this.Status == ExecutorStatus.Waiting || this.Status == ExecutorStatus.Ready)
            {
                Status = ExecutorStatus.Abandoned;
            }
        }

        public void Invalidated()
        {
            if (this.Status == ExecutorStatus.Completed || this.Status == ExecutorStatus.Executing)
            {
                Status = ExecutorStatus.Invalid;
            }
        }
    }

    public class RequestedRecipeClass
    {
        public RequestedSubProgramClass RequestedSubProgram { get; set; }
        public RecipeClass Recipe { get; set; }
        public Int32 Priority { get; set; }   //Inherited from its chamber recipe
        public List<ExecutorClass> Executors { get; set; }
        public ExecutorClass ValidExecutor      //When set current executor status to Invalid, a new Executor will be created and added into Executors
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

        public RequestedRecipeClass(RequestedSubProgramClass RequestedSubProgram, RecipeClass Recipe)
        {
            this.RequestedSubProgram = RequestedSubProgram;
            this.Recipe = Recipe;
            //this.Priority = Priority;
            if (Recipe.ChamberRecipe != null)
                this.Priority = Recipe.ChamberRecipe.Priority;
            else
                this.Priority = 0;      //If no chamber recipe, then make it the most priority task
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
        public RequestedRecipeClass TopWaitingRequestedRecipe   //When assign assets to requested subprogram, we need to find the top waiting requested recipe
        {
            get 
            {
                //foreach (var rec in requestedRecipes)
                //{
                //    if (rec.ValidExecutor.Status == ExecutorStatus.Waiting)
                //        return rec;
                //}
                //return null;    //No valid Executor, we may consider to throw exception here
                try
                {
                    return requestedRecipes.First(rec => rec.ValidExecutor.Status == ExecutorStatus.Waiting);
                }
                catch
                {
                    return null;
                }
            }
            set
            {
                //RequestedRecipeClass RequestedRecipe;
                //foreach (var rec in requestedRecipes)
                //{
                //    if (rec.ValidExecutor.Status == ExecutorStatus.Waiting)
                //        RequestedRecipe = rec;
                //}
                //RequestedRecipe = value;
                RequestedRecipeClass RequestedRecipe = requestedRecipes.First(rec => rec.ValidExecutor.Status == ExecutorStatus.Waiting);
            }
        }

        public RequestedSubProgramClass(RequestedProgramClass RequestedProgram, SubProgramClass sp, Int32 Priority)
        {
            this.RequestedProgram = RequestedProgram;
            this.SubProgram = sp;
            this.Priority = Priority;   //Inherited from requested program
            //RequestedRecipes = new List<RequestedRecipeClass>();
            foreach (var rec in sp.Recipes)
            {
                RequestedRecipeClass RequestedRecipe = new RequestedRecipeClass(this, rec);
                //RequestedRecipe.ValidExecutor.StatusChanged += new EventHandler(Executor_StatusChanged);  //Scheduler subscribe this event instead of request
                RequestedRecipes.Add(RequestedRecipe);
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
            this.Priority = Priority;      //Inherited from request
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
        public Double InvalidRate { get { return InvalidNumber / SubProgramNumber; } }

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
        public Int32 InvalidNumber
        {
            get
            {
                return
                    (
                    from subpro in RequestedProgram.RequestedSubPrograms
                    from recipe in subpro.RequestedRecipes
                    from executor in recipe.Executors
                    where executor.Status == ExecutorStatus.Invalid
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
