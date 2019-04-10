using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    // Summary:
    //     Represents a test program which can be requested and executed over and over again
    public class ProgramClass
    {
        public Int32 ProgramID { get; set; }
        public BatteryModelClass BatteryModel { get; set; }
        public String Name { get; set; }
        public List<SubProgramClass> SubPrograms { get; set; }
        public TimeSpan EstimateDuration { get; set; }

        public ProgramClass(Int32 ProgramID, BatteryModelClass BatteryModel, String Name, List<SubProgramClass> SubPrograms)
        {
            this.ProgramID = ProgramID;
            this.BatteryModel = BatteryModel;
            this.Name = Name;
            this.SubPrograms = SubPrograms;
        }
    }

    // Summary:
    //     Represents a sub program which can be united to form a program
    public class SubProgramClass
    {
        public Int32 SubProgramID { get; set; }
        public List<RecipeClass> Recipes { get; set; }
        public TimeSpan EstimateDuration { get; set; }

        public SubProgramClass(Int32 SubProgramID, List<RecipeClass> Recipes)
        {
            this.SubProgramID = SubProgramID;
            this.Recipes = Recipes;
        }
    }

    // Summary:
    //     Represents a recipe which can be united to form a subprogram
    public class RecipeClass
    {
        public Int32 RecipeID { get; set; }
        public TesterRecipeClass TesterRecipe { get; set; }
        public ChamberRecipeClass ChamberRecipe { get; set; }
        public TimeSpan EstimateDuration { get; set; }

        public RecipeClass(Int32 RecipeID, TesterRecipeClass TesterRecipe, ChamberRecipeClass ChamberRecipe)
        {
            this.RecipeID = RecipeID;
            this.TesterRecipe = TesterRecipe;
            this.ChamberRecipe = ChamberRecipe;
        }
    }

    public class TesterRecipeClass
    {
        public Int32 TesterRecipeID { get; set; }
        public TesterClass Tester { get; set; }
        public String Name { get; set; }
        public BatteryModelClass BatteryModel { get; set; }
        public String Steps { get; set; }
        public TimeSpan EstimateDuration { get; set; }

        public TesterRecipeClass(Int32 TesterRecipeID, TesterClass Tester, String Name, BatteryModelClass BatteryModel, String Steps)
        {
            this.TesterRecipeID = TesterRecipeID;
            this.Tester = Tester;
            this.Name = Name;
            this.BatteryModel = BatteryModel;
            this.Steps = Steps;
        }
    }

    public class ChamberRecipeClass
    {
        public Int32 ChamberRecipeID { get; set; }
        public ChamberClass Chamber { get; set; }
        public String Name { get; set; }
        public String Steps { get; set; }

        public ChamberRecipeClass(Int32 ChamberRecipeID, ChamberClass Chamber, String Name, String Steps)
        {
            this.ChamberRecipeID = ChamberRecipeID;
            this.Chamber = Chamber;
            this.Name = Name;
            this.Steps = Steps;
        }
    }
}
