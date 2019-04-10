using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    public static class Scheduler
    {
        private static List<RequestedSubProgramClass> requestedSubPrograms = new List<RequestedSubProgramClass>();
        public static List<RequestedSubProgramClass> RequestedSubPrograms 
        {
            get 
            {
                return requestedSubPrograms;
            }
            set
            {
                requestedSubPrograms = value;
            }
        }
        public static RequestedRecipeClass TopRequestedRecipe { get; set; }
        public static void ImportTasks(List<RequestedSubProgramClass> newlist)
        {
            foreach (var sp in newlist)
                RequestedSubPrograms.Add(sp);
        }

        public static void OrderTasks()
        { 
        }

        public static void AssignAssets(BatteryClass Battery, ChamberClass Chamber, TesterChannelClass TesterChannel)
        {
        }
    }
}
