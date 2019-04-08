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
        public CurrRecipe CR { get; set; }
        public TempRecipe TR { get; set; }

        public Recipe(CurrRecipe CR, TempRecipe TR)
        {
            this.CR = CR;
            this.TR = TR;
        }
    }

    public class CurrRecipe
    {        
        public Int32 CurrRecipeID { get; set; }
        public String Name { get; set; }
        public Int32 BatteryModelID { get; set; }
        public String Steps { get; set; }

        public CurrRecipe(Int32 CurrRecipeID, String Name, Int32 BatteryModelID, String Steps)
        {
            this.CurrRecipeID = CurrRecipeID;
            this.Name = Name;
            this.BatteryModelID = BatteryModelID;
            this.Steps = Steps;
        }
    }

    public class TempRecipe
    {
        public Int32 TempRecipeID { get; set; }
        public String Name { get; set; }
        public String Steps { get; set; }

        public TempRecipe(Int32 TempRecipeID, String Name, String Steps)
        {
            this.TempRecipeID = TempRecipeID;
            this.Name = Name;
            this.Steps = Steps;
        }
    }
}
