using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell.Model
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
        private static Int32 nextID = 1;
        private Int32 NextID
        {
            get
            {
                nextID += 1;
                return nextID - 1;
            }
        }
        public Int32 ExecutorID { get; set; }
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
            this.ExecutorID = NextID;
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

            if (Battery.Status != AssetStatusEnum.IDLE)
            {
                //Todo: prompt warning message
                System.Windows.MessageBox.Show("Battery is in use!");
                return;
            }

            if (Chamber != null)
            {
                if (Chamber.Status != AssetStatusEnum.IDLE)
                {
                    //Todo: prompt warning message
                    System.Windows.MessageBox.Show("Chamber is in use!");
                    return;
                }
                if (this.RequestedRecipe.Recipe.ChamberRecipe == null)
                {
                    //Todo: prompt warning message but do not return
                    System.Windows.MessageBox.Show("No chamber recipe but chamber is provided!");
                    //return;
                }
            }
            else if (this.RequestedRecipe.Recipe.ChamberRecipe != null)
            {
                //Todo: prompt warning message
                System.Windows.MessageBox.Show("No chamber to be assigned to chamber recipe!");
                return;
            }

            if (TesterChannel.Status != AssetStatusEnum.IDLE)
            {
                //Todo: prompt warning message
                System.Windows.MessageBox.Show("Test Channel is in use!");
                return;
            }

            if (TesterChannel.Tester != this.RequestedRecipe.Recipe.TesterRecipe.Tester)
            {
                //Todo: prompt warning message
                System.Windows.MessageBox.Show("Recipe and Tester are not compatible!");
                return;
            }

            if (Battery.BatteryType != this.RequestedRecipe.Recipe.TesterRecipe.BatteryType)
            {
                //Todo: prompt warning message
                System.Windows.MessageBox.Show("Recipe and Battery are not compatible!");
                return;
            }

            if (this.Status == ExecutorStatus.Waiting)
            {
                this.Battery = Battery;
                Battery.Status = AssetStatusEnum.ASSIGNED;
                if (this.RequestedRecipe.Recipe.ChamberRecipe != null) //If there is no chamber recipe, then we don't need to assign chamber at all.
                {
                    this.Chamber = Chamber;
                    Chamber.Status = AssetStatusEnum.ASSIGNED;
                }
                this.TesterChannel = TesterChannel;
                TesterChannel.Status = AssetStatusEnum.ASSIGNED;
                this.Status = ExecutorStatus.Ready;
            }
            else
            {
                //Todo: prompt warning message
                System.Windows.MessageBox.Show("Only waiting task can be assign assets to!");
                return;
            }
        }

        public void Execute(DateTime StartTime)
        {
            if (this.Status == ExecutorStatus.Ready)
            {
                this.StartTime = StartTime;
                Battery.Status = AssetStatusEnum.USING;
                Battery.UpdateRecords(StartTime, Battery.Status);
                if (Chamber != null)
                {
                    Chamber.Status = AssetStatusEnum.USING;
                    Chamber.UpdateRecords(StartTime, Chamber.Status);
                }
                TesterChannel.Status = AssetStatusEnum.USING;
                TesterChannel.UpdateRecords(StartTime, TesterChannel.Status);
                this.Status = ExecutorStatus.Executing;
            }
            else
            {
                System.Windows.MessageBox.Show("Only ready tasks can be executed!");
            }
        }

        public void Commit(ExecutorStatus Status, DateTime EndTime, String Description = "")  //Need to check the Executor Status to make sure it is executing
        {
            if (Status != ExecutorStatus.Completed && Status != ExecutorStatus.Invalid)
            {
                System.Windows.MessageBox.Show("Only Completed and Invalid are available status to be commited");
                return;
            }
            if (this.Status == ExecutorStatus.Executing)
            {
                this.Description = Description;
                this.EndTime = EndTime;
                if (Chamber != null)
                {
                    Chamber.Status = AssetStatusEnum.IDLE;
                    Chamber.UpdateRecords(StartTime, Chamber.Status);
                }
                TesterChannel.Status = AssetStatusEnum.IDLE;
                TesterChannel.UpdateRecords(StartTime, TesterChannel.Status);
                Battery.Status = AssetStatusEnum.IDLE;
                Battery.UpdateRecords(StartTime, Battery.Status);
                this.Status = Status;   //this has to be the last assignment because it will raise StatusChanged event so that duration will be updated using StartTime and EndTime
            }
            else
            {
                System.Windows.MessageBox.Show("Only executing tasks can be commited!");
            }
        }

        public void Abandon()
        {
            if (this.Status == ExecutorStatus.Abandoned)
            {
                System.Windows.MessageBox.Show("Abandoning Abandoned tasks is meaningless!");
                return;
            }
            else if (this.Status == ExecutorStatus.Executing)
            {
                System.Windows.MessageBox.Show("Commit running task before abandon it!");
                return;
            }
            this.Battery.Status = AssetStatusEnum.IDLE;
            if (this.Chamber != null)
                this.Chamber.Status = AssetStatusEnum.IDLE;
            this.TesterChannel.Status = AssetStatusEnum.IDLE;
            this.Status = ExecutorStatus.Abandoned;
        }

        public void Invalidate()
        {
            if (this.Status == ExecutorStatus.Completed || this.Status == ExecutorStatus.Executing)
            {
                this.Battery.Status = AssetStatusEnum.IDLE;
                if (this.Chamber != null)
                    this.Chamber.Status = AssetStatusEnum.IDLE;
                this.TesterChannel.Status = AssetStatusEnum.IDLE;
                this.Status = ExecutorStatus.Invalid;
            }
            else
            {
                System.Windows.MessageBox.Show("Only completed or executing tasks can be Invalidated!");
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
        public RequestedRecipeClass TopReadyRequestedRecipe   //
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
                    return requestedRecipes.First(rec => rec.ValidExecutor.Status == ExecutorStatus.Ready);
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
                RequestedRecipeClass RequestedRecipe = requestedRecipes.First(rec => rec.ValidExecutor.Status == ExecutorStatus.Ready);
            }
        }
        public RequestedRecipeClass TopRunningRequestedRecipe   //When commit task, we need to find the top running requested recipe
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
                    return requestedRecipes.First(rec => rec.ValidExecutor.Status == ExecutorStatus.Executing);
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
                RequestedRecipeClass RequestedRecipe = requestedRecipes.First(rec => rec.ValidExecutor.Status == ExecutorStatus.Executing);
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
                    where executor.Status == ExecutorStatus.Abandoned
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

        public RequestClass()
        { }

        public void CommitRequest()
        {
            this.RequestID = NextID;
            this.RequestedProgram = new RequestedProgramClass(this, this.Program, this.Priority);
        }

        public RequestClass(Int32 RequestID, ProgramClass Program, String Requester, DateTime RequestDate, Int32 Priority, BatteryClass Battery = null)
        {
            this.RequestID = RequestID;
            this.Program = Program;
            this.Requester = Requester;
            this.RequestDate = RequestDate;
            this.Priority = Priority;
            this.Battery = Battery;
            this.RequestedProgram = new RequestedProgramClass(this, this.Program, this.Priority);
            //sch.ImportTasks(RequestedProgram.RequestedSubPrograms);
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
            //sch.ImportTasks(RequestedProgram.RequestedSubPrograms);
        }
    }
}
