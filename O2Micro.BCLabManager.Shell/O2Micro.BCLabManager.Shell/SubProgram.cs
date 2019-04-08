using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
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
    public class SubProgram
    {
        public Int32 SubProgramID { get; set; }
        public List<Recipe> Recipes { get; set; }

        public SubProgram(Int32 SubProgramID, List<Recipe> Recipes)
        {
            this.SubProgramID = SubProgramID;
            this.Recipes = Recipes;
        }
    }
}
