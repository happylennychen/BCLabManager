using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    //public class Programs
    //{
    //    public List<Program> Ps { get; set; }
    //    public void SaveToDB()
    //    { }
    //    public void LoadFromDB()
    //    { }
    //}
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

    //public class SubPrograms
    //{
    //    public List<SubProgram> Ss { get; set; }
    //    public void SaveToDB()
    //    { }
    //    public void LoadFromDB()
    //    { }
    //}
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
        public TesterRecipe TR { get; set; }
        public ChamberRecipe CR { get; set; }
        public TimeSpan EstimateDuration { get; set; }

        public RecipeClass(Int32 RecipeID, TesterRecipe TR, ChamberRecipe CR)
        {
            this.RecipeID = RecipeID;
            this.TR = TR;
            this.CR = CR;
        }
    }

    public class TesterRecipe
    {
        public Int32 TesterRecipeID { get; set; }
        public TesterClass Tester { get; set; }
        public String Name { get; set; }
        public BatteryModelClass BatteryModel { get; set; }
        public String Steps { get; set; }
        public TimeSpan EstimateDuration { get; set; }

        public TesterRecipe(Int32 TesterRecipeID, TesterClass Tester, String Name, BatteryModelClass BatteryModel, String Steps)
        {
            this.TesterRecipeID = TesterRecipeID;
            this.Tester = Tester;
            this.Name = Name;
            this.BatteryModel = BatteryModel;
            this.Steps = Steps;
        }
    }

    public class ChamberRecipe
    {
        public Int32 ChamberRecipeID { get; set; }
        public ChamberClass Chamber { get; set; }
        public String Name { get; set; }
        public String Steps { get; set; }

        public ChamberRecipe(Int32 ChamberRecipeID, ChamberClass Chamber, String Name, String Steps)
        {
            this.ChamberRecipeID = ChamberRecipeID;
            this.Chamber = Chamber;
            this.Name = Name;
            this.Steps = Steps;
        }
    }
}
