using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    public static class Scheduler
    {
        private static List<RequestedSubProgramClass> waitingRequestedSubPrograms = new List<RequestedSubProgramClass>();
        public static List<RequestedSubProgramClass> WaitingRequestedSubPrograms 
        {
            get 
            {
                return waitingRequestedSubPrograms;
            }
            set
            {
                waitingRequestedSubPrograms = value;
            }
        }
        public static RequestedSubProgramClass TopRequestedSubProgram 
        {
            get
            {
                return waitingRequestedSubPrograms[0];
            }
            set
            {
                waitingRequestedSubPrograms[0] = value;
            }
        }
        public static void ImportTasks(List<RequestedSubProgramClass> newlist)
        {
            foreach (var sp in newlist)
                WaitingRequestedSubPrograms.Add(sp);
        }

        public static void OrderTasks()
        {
            waitingRequestedSubPrograms.OrderBy(o => o.Priority).ThenBy(o => o.RequestedRecipes[0].Recipe.ChamberRecipe);
        }

        public static void AssignAssets(BatteryClass Battery, ChamberClass Chamber, TesterChannelClass TesterChannel)
        {
        }
    }
}
