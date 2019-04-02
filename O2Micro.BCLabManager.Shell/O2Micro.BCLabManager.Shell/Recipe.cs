using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    public class Recipe
    {        
        public Int32 RecipeID { get; set; }
        public String Name { get; set; }
        public Int32 BatteryModelID { get; set; }
        public String Steps { get; set; }

        public Recipe(Int32 RecipeID, String Name, Int32 BatteryModelID, String Steps)
        {
            this.RecipeID = RecipeID;
            this.Name = Name;
            this.BatteryModelID = BatteryModelID;
            this.Steps = Steps;
        }
    }
}
