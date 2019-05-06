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
        readonly ExecutorRepository _executorRepository;
        readonly BatteryRepository _batteryRepository;
        readonly ChamberRepository _chamberRepository;
        readonly TesterRepository _testerRepository;
        string _battery;
        string _chamber;
        string _tester;
        string _testerchannel;
        RelayCommand _saveCommand;
        ReadOnlyCollection<CommandViewModel> _commands;

        #endregion // Fields

        #region Constructor

        public ExecutorViewModel(ExecutorClass executor, ExecutorRepository executorRepository, BatteryRepository batteryRepository, ChamberRepository chamberRepository, TesterRepository testerRepository)     //ExecutorView需要
        {
            if (executor == null)
                throw new ArgumentNullException("executor");

            if (executorRepository == null)
                throw new ArgumentNullException("executorRepository");

            if (batteryRepository == null)
                throw new ArgumentNullException("batteryRepository");

            if (chamberRepository == null)
                throw new ArgumentNullException("chamberRepository");

            if (testerRepository == null)
                throw new ArgumentNullException("testerRepository");

            _executor = executor;
            _executorRepository = executorRepository;
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

        public ExecutorStatus Status
        {
            get
            {
                return _executor.Status;
            }
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
                    select i.Name).ToList();

                return new ObservableCollection<string>(allstring);
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

                _executor.TesterChannel.Tester = _testerRepository.GetItems().First(i => i.Name == _tester);

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

                _executor.TesterChannel = _testerRepository.GetItems().First(i => i.Name == _executor.TesterChannel.Tester.Name).TesterChannels.First(j=>j.TesterChannelID == _executor.TesterChannel.TesterChannelID);

                base.OnPropertyChanged("TesterChannel");
            }
        }

        public ObservableCollection<string> AllTesterChannels
        {
            get
            {
                if (_testerchannel == string.Empty || _testerchannel == null)
                    return null;
                else
                {
                    List<TesterChannelClass> all = _testerRepository.GetItems().First(i => i.Name == _testerchannel).TesterChannels;
                    List<string> allstring = (
                        from i in all
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
        /// Returns a command that saves the customer.
        /// </summary>
        public ICommand SaveCommand
        {
            get
            {
                if (_saveCommand == null)
                {
                    _saveCommand = new RelayCommand(
                        param => { this.Save(); base.OnRequestClose(); },
                        param => this.CanSave
                        );
                }
                return _saveCommand;
            }
        }

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
                    new RelayCommand(param => _executor.Abandon())),

                new CommandViewModel(
                    Resources.ExecutorViewModel_Command_AssignAssets,
                    new RelayCommand(param => _executor.AssignAssets(_executor.Battery, _executor.Chamber, _executor.TesterChannel))),

                new CommandViewModel(
                    Resources.ExecutorViewModel_Command_Commit,
                    new RelayCommand(param => _executor.Commit(_executor.Status, _executor.EndTime, _executor.Description))),

                new CommandViewModel(
                    Resources.ExecutorViewModel_Command_Execute,
                    new RelayCommand(param => _executor.Execute(_executor.StartTime))),
                    
                new CommandViewModel(
                    Resources.ExecutorViewModel_Command_Invalidate,
                    new RelayCommand(param => _executor.Invalidate())),
            };
        }
        #endregion //Commands
        #region Public Methods

        /// <summary>
        /// Saves the customer to the repository.  This method is invoked by the SaveCommand.
        /// </summary>
        public void Save()
        {
            //if (!_batterymodel.IsValid)
            //throw new InvalidOperationException(Resources.BatteryViewModel_Exception_CannotSave);

            if (this.IsNewExecutor)
                _executorRepository.AddItem(_executor);

            base.OnPropertyChanged("DisplayName");
        }

        #endregion // Public Methods

        #region Private Helpers

        /// <summary>
        /// Returns true if this customer was created by the user and it has not yet
        /// been saved to the customer repository.
        /// </summary>
        bool IsNewExecutor
        {
            get { return !_executorRepository.ContainsItem(_executor); }
        }

        /// <summary>
        /// Returns true if the customer is valid and can be saved.
        /// </summary>
        bool CanSave
        {
            get { return true; }
        }

        #endregion // Private Helpers
    }
}
