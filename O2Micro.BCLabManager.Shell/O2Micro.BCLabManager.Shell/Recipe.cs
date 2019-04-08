using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    // Summary:
    //     Represents a recipe which can be united to form a subprogram
    public class Recipe
    {
        public Int32 RecipeID { get; set; }
        public TesterRecipe TR { get; set; }
        public ChamberRecipe CR { get; set; }

        public Recipe(Int32 RecipeID, TesterRecipe TR, ChamberRecipe CR)
        {
            this.RecipeID = RecipeID;
            this.TR = TR;
            this.CR = CR;
        }
    }

    public class TesterRecipe
    {
        public Int32 TesterRecipeID { get; set; }
        public Int32 TesterID { get; set; }
        public String Name { get; set; }
        public Int32 BatteryModelID { get; set; }
        public String Steps { get; set; }

        public TesterRecipe(Int32 TesterRecipeID, Int32 TesterID, String Name, Int32 BatteryModelID, String Steps)
        {
            this.TesterRecipeID = TesterRecipeID;
            this.TesterID = TesterID;
            this.Name = Name;
            this.BatteryModelID = BatteryModelID;
            this.Steps = Steps;
        }
    }

    public class ChamberRecipe
    {
        public Int32 ChamberRecipeID { get; set; }
        public Int32 ChamberID { get; set; }
        public String Name { get; set; }
        public String Steps { get; set; }

        public ChamberRecipe(Int32 ChamberRecipeID, Int32 ChamberID, String Name, String Steps)
        {
            this.ChamberRecipeID = ChamberRecipeID;
            this.ChamberID = ChamberID;
            this.Name = Name;
            this.Steps = Steps;
        }
    }
}
