﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using O2Micro.BCLabManager.Shell.DataAccess;

namespace O2Micro.BCLabManager.Shell.Model
{
    public class Scheduler
    {
        readonly RequestRepository _requestRepository;
        private List<RequestedSubProgramClass> waitingRequestedSubPrograms = new List<RequestedSubProgramClass>();
        public List<RequestedSubProgramClass> WaitingList
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
        public RequestedSubProgramClass TopWaitingRequestedSubProgram
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
        private List<RequestedSubProgramClass> runningRequestedSubPrograms = new List<RequestedSubProgramClass>();
        public List<RequestedSubProgramClass> RunningList
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
        private List<RequestedSubProgramClass> readyRequestedSubPrograms = new List<RequestedSubProgramClass>();
        public List<RequestedSubProgramClass> ReadyList
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
        private List<RequestedSubProgramClass> completedRequestedSubPrograms = new List<RequestedSubProgramClass>();
        public List<RequestedSubProgramClass> CompletedList
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

        public Scheduler(RequestRepository requestRepository)
        {
            _requestRepository = requestRepository;
            _requestRepository.ItemAdded += this.OnRequestAddedToRepository;
        }

        public void ImportTasks(List<RequestedSubProgramClass> newlist)
        {
            foreach (var sp in newlist)
            {
                foreach (var RequestedRecipe in sp.RequestedRecipes)
                    RequestedRecipe.ValidExecutor.StatusChanged += new EventHandler(Executor_StatusChanged);
                WaitingList.Add(sp);
            }
            //WaitingList.AddRange(newlist);
        }

        public void OrderTasks()
        {
            if (WaitingList.Count == 0) //Prompt warning
            {
                System.Windows.MessageBox.Show("No waiting tasks to be ordered!");
                return;
            }
            WaitingList = WaitingList.OrderBy(o => o.Priority).ThenBy(o => o.TopWaitingRequestedRecipe.Priority).ToList();
            //WaitingList = WaitingList.OrderBy(o => o.Priority).ToList();
        }

        public void AssignAssets(BatteryClass Battery, ChamberClass Chamber, TesterChannelClass TesterChannel)
        {
            //Check if the assets is in use or not first
            if (WaitingList.Count == 0) //Prompt warning
            {
                System.Windows.MessageBox.Show("No waiting tasks to assign assets to!");
                return;
            }
            RequestedSubProgramClass RequestedSubProgram = TopWaitingRequestedSubProgram;
            ExecutorClass validExecutor = TopWaitingRequestedSubProgram.TopWaitingRequestedRecipe.ValidExecutor;
            validExecutor.AssignAssets(Battery, Chamber, TesterChannel);
        }

        public void Execute(DateTime StartTime)
        {
            if (ReadyList.Count == 0) //Prompt warning
            {
                System.Windows.MessageBox.Show("No ready tasks to be executed!");
                return;
            }
            //Int32 count = ReadyList.Count;
            RequestedSubProgramClass[] ReadyArray = ReadyList.ToArray();
            foreach (var RequestedSubProgram in ReadyArray)
            {
                //RequestedSubProgramClass RequestedSubProgram = ReadyList[i];
                //ExecutorClass validExecutor = RequestedSubProgram.TopWaitingRequestedRecipe.ValidExecutor;
                foreach (var RequestedRecipe in RequestedSubProgram.RequestedRecipes)
                {
                    if (RequestedRecipe.ValidExecutor.Status == ExecutorStatus.Ready)
                        RequestedRecipe.ValidExecutor.Execute(StartTime);
                }
            }
        }

        #region Event Handling Methods

        private void OnRequestAddedToRepository(object sender, ItemAddedEventArgs<RequestClass> e)
        {
            ImportTasks(e.NewItem.RequestedProgram.RequestedSubPrograms);
        }

        private void Executor_StatusChanged(object sender, EventArgs e)
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
                        else if (RunningList.Contains(root))
                            RunningList.Remove(root);
                        else if (CompletedList.Contains(root))
                            CompletedList.Remove(root);
                        break;
                    case ExecutorStatus.Invalid:
                        Executor.RequestedRecipe.AddExecutor();
                        if (CompletedList.Contains(root))
                        {
                            CompletedList.Remove(root);
                            WaitingList.Insert(0, root);    //back to the top by default
                        }
                        else if (RunningList.Contains(root))
                        {
                            RunningList.Remove(root);
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

        #endregion // Event Handling Methods
    }
}
