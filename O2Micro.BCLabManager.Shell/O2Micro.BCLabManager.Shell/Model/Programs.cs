using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell.Model
{
    // Summary:
    //     Represents a test program which can be requested and executed over and over again
    public class ProgramClass
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
        public Int32 ProgramID { get; set; }
        public BatteryTypeClass BatteryType { get; set; }
        public String Name { get; set; }
        public List<SubProgramClass> SubPrograms { get; set; }
        public TimeSpan EstimateDuration { get; set; }

        public ProgramClass(Int32 ProgramID, BatteryTypeClass BatteryType, String Name, List<SubProgramClass> SubPrograms)
        {
            this.ProgramID = ProgramID;
            this.BatteryType = BatteryType;
            this.Name = Name;
            this.SubPrograms = SubPrograms;
            foreach (var SubProgram in SubPrograms)
            {
                SubProgram.EstimateDurationChanged+=new EventHandler(SubProgram_EstimateDurationChanged);
            }
        }
        public ProgramClass(BatteryTypeClass BatteryType, String Name, List<SubProgramClass> SubPrograms)
        {
            this.ProgramID = NextID;
            this.BatteryType = BatteryType;
            this.Name = Name;
            this.SubPrograms = SubPrograms;
            foreach (var SubProgram in SubPrograms)
            {
                SubProgram.EstimateDurationChanged += new EventHandler(SubProgram_EstimateDurationChanged);
            }
        }

        public void SubProgram_EstimateDurationChanged(object sender, EventArgs e)
        {
            try
            {
                //SubProgramClass Executor = (SubProgramClass)sender;
                EstimateDuration = TimeSpan.FromSeconds(this.SubPrograms.Sum(o => o.EstimateDuration.TotalSeconds));
            }
            catch
            {
                //sender is not a SubProgramClass type
                return;
            }
        }
    }

    // Summary:
    //     Represents a sub program which can be united to form a program
    public class SubProgramClass
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
        public Int32 SubProgramID { get; set; }
        public List<RecipeClass> Recipes { get; set; }
        private TimeSpan estimateDuration = new TimeSpan();
        public TimeSpan EstimateDuration
        {
            get
            {
                return estimateDuration;
            }
            set
            {
                if (value != estimateDuration)
                {
                    estimateDuration = value;
                    OnRasieEstimateDurationChangedEvent();
                }
            }
        }

        public event EventHandler EstimateDurationChanged;

        protected virtual void OnRasieEstimateDurationChangedEvent()
        {
            EventHandler handler = EstimateDurationChanged;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public SubProgramClass(Int32 SubProgramID, List<RecipeClass> Recipes)
        {
            this.SubProgramID = SubProgramID;
            this.Recipes = Recipes;
            foreach (var Recipe in Recipes)
            {
                Recipe.EstimateDurationChanged += new EventHandler(Recipe_EstimateDurationChanged);
            }
        }

        public SubProgramClass(List<RecipeClass> Recipes)
        {
            this.SubProgramID = NextID;
            this.Recipes = Recipes;
            foreach (var Recipe in Recipes)
            {
                Recipe.EstimateDurationChanged += new EventHandler(Recipe_EstimateDurationChanged);
            }
        }

        public void Recipe_EstimateDurationChanged(object sender, EventArgs e)
        {
            try
            {
                //RecipeClass Recipe = (RecipeClass)sender;
                EstimateDuration = TimeSpan.FromSeconds(this.Recipes.Sum(o => o.EstimateDuration.TotalSeconds));
            }
            catch
            {
                //sender is not a RecipeClass type
                return;
            }
        }
    }

    // Summary:
    //     Represents a recipe which can be united to form a subprogram
    public class RecipeClass
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
        public Int32 RecipeID { get; set; }
        public TesterRecipeClass TesterRecipe { get; set; }
        public ChamberRecipeClass ChamberRecipe { get; set; }
        private TimeSpan estimateDuration = new TimeSpan();
        public TimeSpan EstimateDuration 
        {
            get
            {
                return estimateDuration;
            }
            set
            {
                if (value != estimateDuration)
                {
                    estimateDuration = value;
                    OnRasieEstimateDurationChangedEvent();
                }
            }
        }

        public event EventHandler EstimateDurationChanged;

        protected virtual void OnRasieEstimateDurationChangedEvent()
        {
            EventHandler handler = EstimateDurationChanged;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }

        public RecipeClass(Int32 RecipeID, TesterRecipeClass TesterRecipe, ChamberRecipeClass ChamberRecipe)
        {
            this.RecipeID = RecipeID;
            this.TesterRecipe = TesterRecipe;
            this.ChamberRecipe = ChamberRecipe;
        }

        public RecipeClass(TesterRecipeClass TesterRecipe, ChamberRecipeClass ChamberRecipe)
        {
            this.RecipeID = NextID;
            this.TesterRecipe = TesterRecipe;
            this.ChamberRecipe = ChamberRecipe;
        }

        public void Executor_StatusChanged(object sender, EventArgs e)
        {
            try
            {
                ExecutorClass Executor = (ExecutorClass)sender;
                switch (Executor.Status)
                {
                    case ExecutorStatus.Completed:
                        EstimateDuration = Executor.EndTime - Executor.StartTime;   //Just use the last executor's value here. A better and more complex way is to use historic average value.
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

    public class TesterRecipeClass
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
        public Int32 TesterRecipeID { get; set; }
        public TesterClass Tester { get; set; }
        public String Name { get; set; }
        public BatteryTypeClass BatteryType { get; set; }
        public String Steps { get; set; }
        public TimeSpan EstimateDuration { get; set; }

        public TesterRecipeClass(Int32 TesterRecipeID, TesterClass Tester, String Name, BatteryTypeClass BatteryType, String Steps)
        {
            this.TesterRecipeID = TesterRecipeID;
            this.Tester = Tester;
            this.Name = Name;
            this.BatteryType = BatteryType;
            this.Steps = Steps;
        }

        public TesterRecipeClass(TesterClass Tester, String Name, BatteryTypeClass BatteryType, String Steps)
        {
            this.TesterRecipeID = NextID;
            this.Tester = Tester;
            this.Name = Name;
            this.BatteryType = BatteryType;
            this.Steps = Steps;
        }
    }

    public class ChamberRecipeClass
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
        public Int32 ChamberRecipeID { get; set; }
        public Int32 Priority { get; set; }
        public ChamberClass Chamber { get; set; }
        public String Name { get; set; }
        public String Steps { get; set; }

        public ChamberRecipeClass(Int32 ChamberRecipeID, Int32 Priority, ChamberClass Chamber, String Name, String Steps)
        {
            this.ChamberRecipeID = ChamberRecipeID;
            this.Priority = Priority;
            this.Chamber = Chamber;
            this.Name = Name;
            this.Steps = Steps;
        }

        public ChamberRecipeClass(Int32 Priority, ChamberClass Chamber, String Name, String Steps)
        {
            this.ChamberRecipeID = NextID;
            this.Priority = Priority;
            this.Chamber = Chamber;
            this.Name = Name;
            this.Steps = Steps;
        }
    }
}
