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
        private static List<RequestedSubProgramClass> runningRequestedSubPrograms = new List<RequestedSubProgramClass>();
        public static List<RequestedSubProgramClass> RunningRequestedSubPrograms
        {
            get
            {
                return runningRequestedSubPrograms;
            }
            set
            {
                runningRequestedSubPrograms = value;
            }
        }
        private static List<RequestedSubProgramClass> readyRequestedSubPrograms = new List<RequestedSubProgramClass>();
        public static List<RequestedSubProgramClass> ReadyRequestedSubPrograms
        {
            get
            {
                return readyRequestedSubPrograms;
            }
            set
            {
                readyRequestedSubPrograms = value;
            }
        }
        public static void ImportTasks(List<RequestedSubProgramClass> newlist)
        {
            foreach (var sp in newlist)
                WaitingRequestedSubPrograms.Add(sp);
        }

        public static void OrderTasks()
        {
            //waitingRequestedSubPrograms = waitingRequestedSubPrograms.OrderBy(o => o.Priority).ThenBy(o=>o.RequestedRecipes[0].Recipe.ChamberRecipe.Priority).ToList();
            waitingRequestedSubPrograms = waitingRequestedSubPrograms.OrderBy(o => o.Priority).ToList();
        }

        public static void AssignAssets(BatteryClass Battery, ChamberClass Chamber, TesterChannelClass TesterChannel)
        {
            //Check if the assets is in use or not first
            RequestedSubProgramClass RequestedSubProgram = TopRequestedSubProgram;
            ExecutorClass validExecutor = TopRequestedSubProgram.TopRequestedRecipe.ValidExecutor;
            validExecutor.AssignAssets(Battery, Chamber, TesterChannel);
            ReadyRequestedSubPrograms.Add(RequestedSubProgram);
            WaitingRequestedSubPrograms.Remove(RequestedSubProgram);
        }

        public static void Run()
        {
            foreach (var RequestedSubProgram in ReadyRequestedSubPrograms)
            {
                ExecutorClass validExecutor = RequestedSubProgram.TopRequestedRecipe.ValidExecutor;
                validExecutor.Execute();
                RunningRequestedSubPrograms.Add(RequestedSubProgram);
            }
            ReadyRequestedSubPrograms.Clear();
        }
    }
}
