using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Windows.Input;
using System.Linq;
using System.Collections.ObjectModel;
using O2Micro.BCLabManager.Shell.DataAccess;
using O2Micro.BCLabManager.Shell.Model;
using O2Micro.BCLabManager.Shell.Properties;

namespace O2Micro.BCLabManager.Shell.ViewModel
{
    /// <summary>
    /// A UI-friendly wrapper for a Customer object.
    /// </summary>
    public class ExecutorViewModel : WorkspaceViewModel//, IDataErrorInfo
    {
        #region Fields

        readonly ExecutorClass _executor;
        //readonly ExecutorRepository _executorRepository;
        readonly BatteryRepository _batteryRepository;
        readonly ChamberRepository _chamberRepository;
        readonly TesterRepository _testerRepository;
        string _battery;
        string _chamber;
        string _tester;
        string _testerchannel;
        //RelayCommand _saveCommand;
        ReadOnlyCollection<CommandViewModel> _commands;

        #endregion // Fields

        #region Constructor

        public ExecutorViewModel(ExecutorClass executor, /*ExecutorRepository executorRepository,*/ BatteryRepository batteryRepository, ChamberRepository chamberRepository, TesterRepository testerRepository)     //ExecutorView需要
        {
            if (executor == null)
                throw new ArgumentNullException("executor");

            //if (executorRepository == null)
            //throw new ArgumentNullException("executorRepository");

            if (batteryRepository == null)
                throw new ArgumentNullException("batteryRepository");

            if (chamberRepository == null)
                throw new ArgumentNullException("chamberRepository");

            if (testerRepository == null)
                throw new ArgumentNullException("testerRepository");

            _executor = executor;
            //_executorRepository = executorRepository;
            _batteryRepository = batteryRepository;
            _chamberRepository = chamberRepository;
            _testerRepository = testerRepository;
        }

        /*public ExecutorViewModel(ExecutorClass executor, ExecutorRepository executorRepository)     //AllExecutorView需要
        {
            if (executor == null)
                throw new ArgumentNullException("executor");

            if (executorRepository == null)
                throw new ArgumentNullException("executorRepository");

            _executor = executor;
            _executorRepository = executorRepository;
        }*/

        /*void CreateAllBatteryTypes()
        {
            List<BatteryTypeClass> all = _batterytypeRepository.GetItems();

            this.AllBatteryTypes = new ObservableCollection<BatteryTypeClass>(all);
        }*/

        #endregion // Constructor

        #region ExecutorClass Properties

        public Int32 ExecutorID
        {
            get
            {
                return _executor.ExecutorID;
            }
        }

        public string Status
        {
            get
            {
                return _executor.Status.ToString();
            }
            /*set
            {
                if (value == _executor.Status.ToString())
                    return;
                switch (value)
                {
                    case "Abandoned":
                        _executor.Status = ExecutorStatus.Abandoned;
                        break;
                    case "Completed":
                        _executor.Status = ExecutorStatus.Completed;
                        break;
                    case "Executing":
                        _executor.Status = ExecutorStatus.Executing;
                        break;
                    case "Invalid":
                        _executor.Status = ExecutorStatus.Invalid;
                        break;
                    case "Ready":
                        _executor.Status = ExecutorStatus.Ready;
                        break;
                    case "Waiting":
                        _executor.Status = ExecutorStatus.Waiting;
                        break;
                }

                base.OnPropertyChanged("Status");
            }*/
        }

        public string Description
        {
            get
            {
                return _executor.Description;
            }
            set
            {
                if (value == _executor.Description)
                    return;

                _executor.Description = value;

                base.OnPropertyChanged("Description");
            }
        }

        public DateTime StartTime
        {
            get { return _executor.StartTime; }
            set
            {
                if (value == _executor.StartTime)
                    return;

                _executor.StartTime = value;

                base.OnPropertyChanged("StartTime");
            }
        }

        public DateTime EndTime
        {
            get { return _executor.EndTime; }
            set
            {
                if (value == _executor.EndTime)
                    return;

                _executor.EndTime = value;

                base.OnPropertyChanged("EndTime");
            }
        }
        #endregion // ExecutorClass Properties

        #region Presentation Properties

        public String Battery
        {
            get
            {
                if (_battery == null)
                {
                    if (_executor.Battery == null)
                        _battery = string.Empty;
                    else
                        _battery = _executor.Battery.Name;
                }
                return _battery;
            }
            set
            {
                if (value == _battery || String.IsNullOrEmpty(value))
                    return;

                _battery = value;

                _executor.Battery = _batteryRepository.GetItems().First(i => i.Name == _battery);

                base.OnPropertyChanged("Battery");
            }
        }

        public ObservableCollection<string> AllBatteries
        {
            get
            {
                List<BatteryClass> all = _batteryRepository.GetItems();
                List<string> allstring = (
                    from i in all
                    where i.Status == AssetStatusEnum.IDLE
                    select i.Name).ToList();

                return new ObservableCollection<string>(allstring);
            }
        }

        public String Chamber
        {
            get
            {
                if (_chamber == null)
                {
                    if (_executor.Chamber == null)
                        _chamber = string.Empty;
                    else
                        _chamber = _executor.Chamber.Name;
                }
                return _chamber;
            }
            set
            {
                if (value == _chamber || String.IsNullOrEmpty(value))
                    return;

                _chamber = value;

                _executor.Chamber = _chamberRepository.GetItems().First(i => i.Name == _chamber);

                base.OnPropertyChanged("Chamber");
            }
        }

        public ObservableCollection<string> AllChambers
        {
            get
            {
                List<ChamberClass> all = _chamberRepository.GetItems();
                List<string> allstring = (
                    from i in all
                    where i.Status == AssetStatusEnum.IDLE
                    select i.Name).ToList();

                return new ObservableCollection<string>(allstring);
            }
        }

        public bool IsChamberNeeded
        {
            get
            {
                return this._executor.RequestedRecipe.Recipe.ChamberRecipe != null;
            }
        }
        public String Tester
        {
            get
            {
                if (_tester == null)
                {
                    if (_executor.TesterChannel == null)
                        _tester = string.Empty;
                    else if (_executor.TesterChannel.Tester == null)
                        _tester = string.Empty;
                    else
                        _tester = _executor.TesterChannel.Tester.Name;
                }
                return _tester;
            }
            set
            {
                if (value == _tester || String.IsNullOrEmpty(value))
                    return;

                _tester = value;

                //_executor.TesterChannel.Tester = _testerRepository.GetItems().First(i => i.Name == _tester);

                base.OnPropertyChanged("Tester");
            }
        }

        public ObservableCollection<string> AllTesters
        {
            get
            {
                List<TesterClass> all = _testerRepository.GetItems();
                List<string> allstring = (
                    from i in all
                    select i.Name).ToList();

                return new ObservableCollection<string>(allstring);
            }
        }

        public String TesterChannel
        {
            get
            {
                if (_testerchannel == null)
                {
                    if (_executor.TesterChannel == null)
                        _testerchannel = string.Empty;
                    else
                        _testerchannel = _executor.TesterChannel.TesterChannelID.ToString();
                }
                return _testerchannel;
            }
            set
            {
                if (value == _testerchannel || String.IsNullOrEmpty(value))
                    return;

                _testerchannel = value;

                var tester = _testerRepository.GetItems().First(i => i.Name == _tester);
                _executor.TesterChannel = tester.TesterChannels.First(j => j.TesterChannelID == Int32.Parse(_testerchannel));
                //_executor.TesterChannel.Tester = tester;
                base.OnPropertyChanged("TesterChannel");
            }
        }

        public ObservableCollection<string> AllTesterChannels
        {
            get
            {
                if (_tester == string.Empty || _tester == null)
                    return null;
                else
                {
                    List<TesterChannelClass> all = _testerRepository.GetItems().First(i => i.Name == _tester).TesterChannels;
                    List<string> allstring = (
                        from i in all
                        where i.Status == AssetStatusEnum.IDLE
                        select i.TesterChannelID.ToString()).ToList();

                    return new ObservableCollection<string>(allstring);
                }
            }
        }

        public Int32 RequestID
        {
            get
            {
                return _executor.RequestedRecipe.RequestedSubProgram.RequestedProgram.Request.RequestID;
            }
        }

        public Int32 ProgramID
        {
            get
            {
                return _executor.RequestedRecipe.RequestedSubProgram.RequestedProgram.Program.ProgramID;
            }
        }

        public Int32 SubProgramID
        {
            get
            {
                return _executor.RequestedRecipe.RequestedSubProgram.SubProgram.SubProgramID;
            }
        }

        public Int32 RecipeID
        {
            get
            {
                return _executor.RequestedRecipe.Recipe.RecipeID;
            }
        }


        /*public override string DisplayName
        {
            get
            {
                if (this.IsNewExecutor)
                {
                    return Resources.BatteryViewModel_DisplayName;
                }
                else
                {
                    return "Executor#" + _executor.ExecutorID.ToString();
                }
            }
        }*/

        #endregion // Presentation Properties

        #region Commands

        /// <summary>
        /// Returns a read-only list of commands 
        /// that the UI can display and execute.
        /// </summary>
        public ReadOnlyCollection<CommandViewModel> Commands
        {
            get
            {
                if (_commands == null)
                {
                    List<CommandViewModel> cmds = this.CreateCommands();
                    _commands = new ReadOnlyCollection<CommandViewModel>(cmds);
                }
                return _commands;
            }
        }

        List<CommandViewModel> CreateCommands()
        {
            return new List<CommandViewModel>
            {
                new CommandViewModel(
                    Resources.ExecutorViewModel_Command_Abandon,
                    new RelayCommand(param => this.Abandon(),param => this.CanAbandon())),

                new CommandViewModel(
                    Resources.ExecutorViewModel_Command_AssignAssets,
                    new RelayCommand(param => this.AssignAssets(), param=>this.CanAssign())),

                new CommandViewModel(
                    Resources.ExecutorViewModel_Command_Commit,
                    new RelayCommand(param => this.Commit(), param=>this.CanCommit())),

                new CommandViewModel(
                    Resources.ExecutorViewModel_Command_Execute,
                    new RelayCommand(param => this.Execute(), param=>this.CanExecute())),
                    
                new CommandViewModel(
                    Resources.ExecutorViewModel_Command_Invalidate,
                    new RelayCommand(param => this.Invalidate(), param=>this.CanInvalidate())),
            };
        }
        #endregion //Commands

        #region Public Methods

        #endregion // Public Methods

        #region Event
        /*public event EventHandler InvalidateInvoked;

        protected virtual void OnRasieInvalidateInvokedEvent()
        {
            EventHandler handler = InvalidateInvoked;

            if (handler != null)
            {
                handler(this, EventArgs.Empty);
            }
        }*/
        #endregion  //Event
        #region Private Helpers
        private void Abandon()
        {
            this._executor.Abandon();
            base.OnPropertyChanged("Status");
        }
        private bool CanAbandon()
        {
            if (this._executor.Status == ExecutorStatus.Abandoned)
                return false;
            else
                return true;
        }
        private void AssignAssets()
        {
            this._executor.AssignAssets(_executor.Battery, _executor.Chamber, _executor.TesterChannel);
            base.OnPropertyChanged("Status");
        }
        private bool CanAssign()
        {
            if (this._executor.Status == ExecutorStatus.Waiting
                && this._executor.Battery != null
                && ((this._executor.Chamber != null) ^ (this._executor.RequestedRecipe.Recipe.ChamberRecipe == null))
                && this._executor.TesterChannel != null)
                return true;
            else
                return false;
        }
        private void Commit()
        {
            this._executor.Commit(ExecutorStatus.Completed, _executor.EndTime, _executor.Description);
            base.OnPropertyChanged("Status");
        }
        private bool CanCommit()
        {
            if (this._executor.Status == ExecutorStatus.Executing
                && this._executor.EndTime != null)
                return true;
            else
                return false;
        }
        private void Execute()
        {
            this._executor.Execute(_executor.StartTime);
            base.OnPropertyChanged("Status");
        }
        private bool CanExecute()
        {
            if (this._executor.Status == ExecutorStatus.Ready
                && this._executor.StartTime != null)
                return true;
            else
                return false;
        }
        private void Invalidate()
        {
            this._executor.Invalidate();
            base.OnPropertyChanged("Status");
            //this.OnRasieInvalidateInvokedEvent();
        }
        private bool CanInvalidate()
        {
            if (this._executor.Status == ExecutorStatus.Executing || this._executor.Status == ExecutorStatus.Completed)
                return true;
            else
                return false;
        }
        #endregion // Private Helpers
    }
}
