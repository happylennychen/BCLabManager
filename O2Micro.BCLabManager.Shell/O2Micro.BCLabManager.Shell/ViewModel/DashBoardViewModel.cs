using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Linq;
using O2Micro.BCLabManager.Shell.DataAccess;
using O2Micro.BCLabManager.Shell.Properties;
using O2Micro.BCLabManager.Shell.Model;

namespace O2Micro.BCLabManager.Shell.ViewModel
{
    /// <summary>
    /// Represents a container of RequestModelViewModel objects
    /// that has support for staying synchronized with the
    /// ExecutorRepository.  This class also provides information
    /// related to multiple selected customers.
    /// </summary>
    public class DashBoardViewModel : WorkspaceViewModel
    {
        #region Fields

        readonly Repositories _repositories;
        private AllExecutorsViewModel _allExecutorsViewModel;

        #endregion // Fields

        #region Constructor

        public DashBoardViewModel(Repositories repositories)
        {
            if (repositories == null)
                throw new ArgumentNullException("repositories");

            base.DisplayName = Resources.DashBoardViewModel_DisplayName;

            _repositories = repositories;
            _allExecutorsViewModel = new AllExecutorsViewModel(_repositories._requestRepository, _repositories._batteryRepository, _repositories._chamberRepository, _repositories._testerRepository);
        }
        /*void CreateValidExecutors()
        {
            List<ExecutorViewModel> all =
                (from req in _repositories._requestRepository.GetItems()
                 from sub in req.RequestedProgram.RequestedSubPrograms
                 from rec in sub.RequestedRecipes
                 from exe in rec.Executors
                 select new ExecutorViewModel(exe, _repositories._batteryRepository, _repositories._chamberRepository, _repositories._testerRepository)).ToList();

            //foreach (RequestModelViewModel batmod in all)
            //batmod.PropertyChanged += this.OnRequestModelViewModelPropertyChanged;

            this._allExecutors = new ObservableCollection<ExecutorViewModel>(all);
        }*/
        #endregion // Constructor

        #region Public Interface
        #region Assets
        public double BatteryAmount
        {
            get { return _repositories._batteryRepository.GetItems().Count; }
        }

        public double UsingBatteryAmount
        {
            get
            {
                return (from bat in _repositories._batteryRepository.GetItems()
                        where bat.Status == AssetStatusEnum.USING
                        select bat).Count();
            }
        }

        public double BatteryUsingPercent
        {
            get { return (double)UsingBatteryAmount / (double)BatteryAmount; }
        }

        public double ChamberAmount
        {
            get { return _repositories._chamberRepository.GetItems().Count; }
        }

        public double UsingChamberAmount
        {
            get
            {
                return (from cmb in _repositories._chamberRepository.GetItems()
                        where cmb.Status == AssetStatusEnum.USING
                        select cmb).Count();
            }
        }

        public double ChamberUsingPercent
        {
            get { return (double)UsingChamberAmount / (double)ChamberAmount; }
        }

        public double TesterChannelAmount
        {
            get
            {
                return (from tster in _repositories._testerRepository.GetItems()
                        from tstch in tster.TesterChannels
                        select tstch).Count();
            }
        }

        public double UsingTesterChannelAmount
        {
            get
            {
                return (from tster in _repositories._testerRepository.GetItems()
                        from tstch in tster.TesterChannels
                        where tstch.Status == AssetStatusEnum.USING
                        select tstch).Count();
            }
        }

        public double TesterChannelUsingPercent
        {
            get { return (double)UsingTesterChannelAmount / (double)TesterChannelAmount; }
        }
        #endregion
        #region Legend
        public Int32 WaitingExeAmount
        {
            get
            {
                return
                (
                    from req in _repositories._requestRepository.GetItems()
                    from sub in req.RequestedProgram.RequestedSubPrograms
                    from rec in sub.RequestedRecipes
                    from exe in rec.Executors
                    where exe.Status == ExecutorStatus.Waiting
                    select exe
                ).Count();
            }
        }
        public Int32 ReadyExeAmount
        {
            get
            {
                return
                (
                    from req in _repositories._requestRepository.GetItems()
                    from sub in req.RequestedProgram.RequestedSubPrograms
                    from rec in sub.RequestedRecipes
                    from exe in rec.Executors
                    where exe.Status == ExecutorStatus.Ready
                    select exe
                ).Count();
            }
        }
        public Int32 ExecutingExeAmount
        {
            get
            {
                return
                (
                    from req in _repositories._requestRepository.GetItems()
                    from sub in req.RequestedProgram.RequestedSubPrograms
                    from rec in sub.RequestedRecipes
                    from exe in rec.Executors
                    where exe.Status == ExecutorStatus.Executing
                    select exe
                ).Count();
            }
        }
        public Int32 CompletedExeAmount
        {
            get
            {
                return
                (
                    from req in _repositories._requestRepository.GetItems()
                    from sub in req.RequestedProgram.RequestedSubPrograms
                    from rec in sub.RequestedRecipes
                    from exe in rec.Executors
                    where exe.Status == ExecutorStatus.Completed
                    select exe
                ).Count();
            }
        }
        public Int32 InvalidExeAmount
        {
            get
            {
                return
                (
                    from req in _repositories._requestRepository.GetItems()
                    from sub in req.RequestedProgram.RequestedSubPrograms
                    from rec in sub.RequestedRecipes
                    from exe in rec.Executors
                    where exe.Status == ExecutorStatus.Invalid
                    select exe
                ).Count();
            }
        }
        public Int32 AbandonedExeAmount
        {
            get
            {
                return
                (
                    from req in _repositories._requestRepository.GetItems()
                    from sub in req.RequestedProgram.RequestedSubPrograms
                    from rec in sub.RequestedRecipes
                    from exe in rec.Executors
                    where exe.Status == ExecutorStatus.Abandoned
                    select exe
                ).Count();
            }
        }
        public Int32 TotalExeAmount
        {
            get
            {
                return
                (
                    from req in _repositories._requestRepository.GetItems()
                    from sub in req.RequestedProgram.RequestedSubPrograms
                    from rec in sub.RequestedRecipes
                    from exe in rec.Executors
                    select exe
                ).Count();
            }
        }
        public double WaitingExePercentage
        {
            get 
            {
                return (double)WaitingExeAmount / (double)TotalExeAmount;
            }
        }
        public double ReadyExePercentage
        {
            get
            {
                return (double)ReadyExeAmount / (double)TotalExeAmount;
            }
        }
        public double ExecutingExePercentage
        {
            get
            {
                return (double)ExecutingExeAmount / (double)TotalExeAmount;
            }
        }
        public double CompletedExePercentage
        {
            get
            {
                return (double)CompletedExeAmount / (double)TotalExeAmount;
            }
        }
        public double InvalidExePercentage
        {
            get
            {
                return (double)InvalidExeAmount / (double)TotalExeAmount;
            }
        }
        public double AbandonedExePercentage
        {
            get
            {
                return (double)AbandonedExeAmount / (double)TotalExeAmount;
            }
        }
        #endregion
        #region AllExecutors
        public AllExecutorsViewModel allExecutorsViewModel
        {
            get
            {
                return _allExecutorsViewModel;
            }
        }
        #endregion

        #endregion // Public Interface

        #region  Base Class Overrides

        #endregion // Base Class Overrides

        #region Event Handling Methods

        #endregion // Event Handling Methods
    }
}