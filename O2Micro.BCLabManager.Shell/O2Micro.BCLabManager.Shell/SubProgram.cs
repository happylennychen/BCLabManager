using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    class SubProgram
    {
        public Int32 SubProgramID { get; set; }
        public Int32 ProgramID { get; set; }
        //public Int32 BatteryID { get; set; }
        //public Double Temperature { get; set; }
        //public Int32 RecipeID { get; set; }
        public List<Int32[]> Recipes { get; set; }

        //public SubProgram(Int32 SubProgramID,Int32 ProgramID,Int32 BatteryID,Double Temperature,Int32 RecipeID)
        public SubProgram(Int32 SubProgramID, Int32 ProgramID, List<Int32[]> Recipes)
        {
            this.SubProgramID = SubProgramID;
            this.ProgramID = ProgramID;
            //this.BatteryID = BatteryID;
            //this.Temperature = Temperature;
            //this.RecipeID = RecipeID;
            this.Recipes = Recipes;
        }
    }
}
