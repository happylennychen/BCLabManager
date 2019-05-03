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
    public class RequestViewModel : WorkspaceViewModel//, IDataErrorInfo
    {
        #region Fields

        readonly RequestClass _request;
        readonly RequestRepository _requestRepository;
        readonly ProgramRepository _programRepository;
        readonly BatteryRepository _batteryRepository;
        RelayCommand _saveCommand;
        string _program;
        string _battery;

        #endregion // Fields

        #region Constructor

        public RequestViewModel(RequestClass requestmodel, RequestRepository requestRepository)     //AllRequestsView需要
        {
            if (requestmodel == null)
                throw new ArgumentNullException("requestmodel");

            if (requestRepository == null)
                throw new ArgumentNullException("requestRepository");

            _request = requestmodel;
            _requestRepository = requestRepository;
        }

        public RequestViewModel(RequestClass requestmodel, RequestRepository requestRepository, ProgramRepository programRepository, BatteryRepository batteryRepository)     //RequestView需要
        {
            if (requestmodel == null)
                throw new ArgumentNullException("Requestmodel");

            if (requestRepository == null)
                throw new ArgumentNullException("RequestRepository");

            if (programRepository == null)
                throw new ArgumentNullException("programRepository");

            if (batteryRepository == null)
                throw new ArgumentNullException("batteryRepository");

            _request = requestmodel;
            _requestRepository = requestRepository;
            _programRepository = programRepository;
            _batteryRepository = batteryRepository;
        }

        #endregion // Constructor

        #region RequestClass Properties

        public string Requester
        {
            get { return _request.Requester; }
            set
            {
                if (value == _request.Requester)
                    return;

                _request.Requester = value;

                base.OnPropertyChanged("Requester");
            }
        }

        public DateTime RequestDate
        {
            get { return _request.RequestDate; }
            set
            {
                if (value == _request.RequestDate)
                    return;

                _request.RequestDate = value;

                base.OnPropertyChanged("RequestDate");
            }
        }

        public Int32 Priority
        {
            get { return _request.Priority; }
            set
            {
                if (value == _request.Priority)
                    return;

                _request.Priority = value;

                base.OnPropertyChanged("Priority");
            }
        }

        #endregion // Customer Properties

        #region Presentation Properties


        public String Program
        {
            get
            {
                if (_program == null)
                {
                    if (_request.Program == null)
                        _program = string.Empty;
                    else
                        _program = _request.Program.Name;
                }
                return _program;
            }
            set
            {
                if (value == _program || String.IsNullOrEmpty(value))
                    return;

                _program = value;

                _request.Program = _programRepository.GetItems().First(i => i.Name == _program);

                base.OnPropertyChanged("Program");
            }
        }

        public ObservableCollection<string> AllPrograms
        {
            get
            {
                List<ProgramClass> all = _programRepository.GetItems();
                List<string> allstring = (
                    from i in all
                    select i.Name).ToList();

                return new ObservableCollection<string>(allstring);
            }
        }


        public String Battery
        {
            get
            {
                if (_battery == null)
                {
                    if (_request.Battery == null)
                        _battery = string.Empty;
                    else
                        _battery = _request.Battery.Name;
                }
                return _battery;
            }
            set
            {
                if (value == _battery || String.IsNullOrEmpty(value))
                    return;

                _battery = value;

                _request.Battery = _batteryRepository.GetItems().First(i => i.Name == _battery);

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


        public Int32 TotalNumber
        {
            get
            {
                return _request.RequestedProgram.RequestedSubPrograms.Count;
            }
        }
        public Double InvalidRate { get { return InvalidNumber / TotalNumber; } }

        public Int32 CompletedNumber
        {
            get
            {
                return
                    (
                    from subpro in _request.RequestedProgram.RequestedSubPrograms
                    from recipe in subpro.RequestedRecipes
                    from executor in recipe.Executors
                    where executor.Status == ExecutorStatus.Completed
                    select executor
                    ).Count();
            }
        }
        public Int32 ExecutingNumber
        {
            get
            {
                return
                    (
                    from subpro in _request.RequestedProgram.RequestedSubPrograms
                    from recipe in subpro.RequestedRecipes
                    from executor in recipe.Executors
                    where executor.Status == ExecutorStatus.Executing
                    select executor
                    ).Count();
            }
        }
        public Int32 WaitingNumber
        {
            get
            {
                return
                    (
                    from subpro in _request.RequestedProgram.RequestedSubPrograms
                    from recipe in subpro.RequestedRecipes
                    from executor in recipe.Executors
                    where executor.Status == ExecutorStatus.Waiting
                    select executor
                    ).Count();
            }
        }
        public Int32 AbandonedNumber
        {
            get
            {
                return
                    (
                    from subpro in _request.RequestedProgram.RequestedSubPrograms
                    from recipe in subpro.RequestedRecipes
                    from executor in recipe.Executors
                    where executor.Status == ExecutorStatus.Abandoned
                    select executor
                    ).Count();
            }
        }
        public Int32 InvalidNumber
        {
            get
            {
                return
                    (
                    from subpro in _request.RequestedProgram.RequestedSubPrograms
                    from recipe in subpro.RequestedRecipes
                    from executor in recipe.Executors
                    where executor.Status == ExecutorStatus.Invalid
                    select executor
                    ).Count();
            }
        }

        public override string DisplayName
        {
            get
            {
                if (this.IsNewRequest)
                {
                    return Resources.RequestViewModel_DisplayName;
                }
                else
                {
                    return "Request#" + _request.RequestID.ToString();
                }
            }
        }

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

        #endregion // Presentation Properties

        #region Public Methods

        /// <summary>
        /// Saves the customer to the repository.  This method is invoked by the SaveCommand.
        /// </summary>
        public void Save()
        {
            //if (!_Requestmodel.IsValid)
            //throw new InvalidOperationException(Resources.RequestViewModel_Exception_CannotSave);

            if (this.IsNewRequest)
            {
                _request.CommitRequest();
                _requestRepository.AddItem(_request);
            }

            base.OnPropertyChanged("DisplayName");
        }

        #endregion // Public Methods

        #region Private Helpers

        /// <summary>
        /// Returns true if this customer was created by the user and it has not yet
        /// been saved to the customer repository.
        /// </summary>
        bool IsNewRequest
        {
            get { return !_requestRepository.ContainsItem(_request); }
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

