﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    public static class Scheduler
    {
        private static List<RequestedSubProgramClass> waitingRequestedSubPrograms = new List<RequestedSubProgramClass>();
        public static List<RequestedSubProgramClass> WaitingList
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
        public static RequestedSubProgramClass TopWaitingRequestedSubProgram
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
        public static List<RequestedSubProgramClass> RunningList
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
        public static List<RequestedSubProgramClass> ReadyList
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
        private static List<RequestedSubProgramClass> completedRequestedSubPrograms = new List<RequestedSubProgramClass>();
        public static List<RequestedSubProgramClass> CompletedList
        {
            get
            {
                return completedRequestedSubPrograms;
            }
            set
            {
                completedRequestedSubPrograms = value;
            }
        }
        public static void ImportTasks(List<RequestedSubProgramClass> newlist)
        {
            foreach (var sp in newlist)
            {
                foreach (var RequestedRecipe in sp.RequestedRecipes)
                    RequestedRecipe.ValidExecutor.StatusChanged += new EventHandler(Executor_StatusChanged);
                WaitingList.Add(sp);
            }
        }
        private static void Executor_StatusChanged(object sender, EventArgs e)
        {
            try
            {
                ExecutorClass Executor = (ExecutorClass)sender;
                RequestedSubProgramClass root = Executor.RequestedRecipe.RequestedSubProgram;
                switch (Executor.Status)
                {
                    case ExecutorStatus.Ready:
                        WaitingList.Remove(root);
                        ReadyList.Add(root);
                        break;
                    case ExecutorStatus.Executing:
                        ReadyList.Remove(root);
                        RunningList.Add(root);
                        break;
                    case ExecutorStatus.Completed:
                        RunningList.Remove(root);
                        if (root.RequestedRecipes.Count == 1)
                            CompletedList.Add(root);
                        else if (root.RequestedRecipes.Count > 1)
                            WaitingList.Insert(0, root);
                        break;
                    case ExecutorStatus.Abandoned:
                        if (ReadyList.Contains(root))
                            ReadyList.Remove(root);
                        else if (WaitingList.Contains(root))
                            WaitingList.Remove(root);
                        break;
                    case ExecutorStatus.Invalid:
                        Executor.RequestedRecipe.AddExecutor();
                        if (CompletedList.Contains(root))
                        {
                            CompletedList.Remove(root);
                            WaitingList.Insert(0, root);    //back to the top by default
                        }
                        break;
                }
            }
            catch
            {
                //sender is not a ExecutorClass type
                return;
            }
        }

        public static void OrderTasks()
        {
            //waitingRequestedSubPrograms = waitingRequestedSubPrograms.OrderBy(o => o.Priority).ThenBy(o=>o.RequestedRecipes[0].Recipe.ChamberRecipe.Priority).ToList();
            waitingRequestedSubPrograms = waitingRequestedSubPrograms.OrderBy(o => o.Priority).ToList();
        }

        public static void AssignAssets(BatteryClass Battery, ChamberClass Chamber, TesterChannelClass TesterChannel)
        {
            //Check if the assets is in use or not first
            RequestedSubProgramClass RequestedSubProgram = TopWaitingRequestedSubProgram;
            ExecutorClass validExecutor = TopWaitingRequestedSubProgram.TopWaitingRequestedRecipe.ValidExecutor;
            validExecutor.AssignAssets(Battery, Chamber, TesterChannel);
        }

        public static void Execute()
        {
            //Int32 count = ReadyList.Count;
            RequestedSubProgramClass[] ReadyArray = ReadyList.ToArray();
            foreach (var RequestedSubProgram in ReadyArray)
            {
                //RequestedSubProgramClass RequestedSubProgram = ReadyList[i];
                //ExecutorClass validExecutor = RequestedSubProgram.TopWaitingRequestedRecipe.ValidExecutor;
                foreach (var RequestedRecipe in RequestedSubProgram.RequestedRecipes)
                {
                    if (RequestedRecipe.ValidExecutor.Status == ExecutorStatus.Ready)
                        RequestedRecipe.ValidExecutor.Execute();
                }
            }
        }
    }
}
