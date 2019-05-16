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
    public class AllExecutorsViewModel : WorkspaceViewModel
    {
        #region Fields
        ObservableCollection<ExecutorViewModel> _allExecutors;
        //ObservableCollection<ExecutorViewModel> _waitingExecutors;

        readonly RequestRepository _requestRepository;
        readonly BatteryRepository _batteryRepository;
        readonly ChamberRepository _chamberRepository;
        readonly TesterRepository _testerRepository;

        #endregion // Fields

        #region Constructor

        public AllExecutorsViewModel(RequestRepository requestRepository, BatteryRepository batteryRepository, ChamberRepository chamberRepository, TesterRepository testerRepository)
        {
            if (requestRepository == null)
                throw new ArgumentNullException("requestRepository");

            if (batteryRepository == null)
                throw new ArgumentNullException("batteryRepository");

            if (chamberRepository == null)
                throw new ArgumentNullException("chamberRepository");

            if (testerRepository == null)
                throw new ArgumentNullException("testerRepository");

            base.DisplayName = Resources.AllExecutorsViewModel_DisplayName;

            _requestRepository = requestRepository;

            _batteryRepository = batteryRepository;

            _chamberRepository = chamberRepository;

            _testerRepository = testerRepository;

            // Subscribe for notifications of when a new customer is saved.
            _requestRepository.ItemAdded += this.OnRequestAddedToRepository;

            // Populate the AllCustomers collection with RequestModelViewModels.
            this.CreateAllExecutors();
        }
        void CreateAllExecutors()
        {
            List<ExecutorViewModel> all =
                (from req in _requestRepository.GetItems()
                 from sub in req.RequestedProgram.RequestedSubPrograms
                 from rec in sub.RequestedRecipes
                 from exe in rec.Executors
                 select new ExecutorViewModel(exe, _batteryRepository, _chamberRepository, _testerRepository)).ToList();

            //foreach (RequestModelViewModel batmod in all)
            //batmod.PropertyChanged += this.OnRequestModelViewModelPropertyChanged;

            this._allExecutors = new ObservableCollection<ExecutorViewModel>(all);
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Returns a collection of all the ExecutorViewModel objects.
        /// </summary>
        public ObservableCollection<ExecutorViewModel> AllExecutors 
        {
            get
            {
                /*if (_allExecutors == null)
                {
                    List<ExecutorViewModel> all =
                        (from req in _requestRepository.GetItems()
                         from sub in req.RequestedProgram.RequestedSubPrograms
                         from rec in sub.RequestedRecipes
                         from exe in rec.Executors
                         select new ExecutorViewModel(exe, _batteryRepository, _chamberRepository, _testerRepository)).ToList();

                    this._allExecutors = new ObservableCollection<ExecutorViewModel>(all);
                }*/
                return _allExecutors;
            }
        }
        /*
        public ObservableCollection<ExecutorViewModel> WatingExecutors
        {
            get
            {
                if (_waitingExecutors == null)
                {
                    List<ExecutorViewModel> all =
                        (from req in _requestRepository.GetItems()
                         from sub in req.RequestedProgram.RequestedSubPrograms
                         from rec in sub.RequestedRecipes
                         from exe in rec.Executors
                         where exe.Status == ExecutorStatus.Waiting
                         select new ExecutorViewModel(exe, _batteryRepository, _chamberRepository, _testerRepository)).ToList();

                    this._waitingExecutors = new ObservableCollection<ExecutorViewModel>(all);
                }
                return _waitingExecutors;
            }
        }*/


        #endregion // Public Interface

        #region  Base Class Overrides

        protected override void OnDispose()
        {
            foreach (ExecutorViewModel reqVM in this.AllExecutors)
                reqVM.Dispose();

            _requestRepository.ItemAdded -= this.OnRequestAddedToRepository;

            this.AllExecutors.Clear();
        }

        #endregion // Base Class Overrides

        #region Event Handling Methods


        void OnRequestAddedToRepository(object sender, ItemAddedEventArgs<RequestClass> e)
        {
            var viewModels = 
                        (from sub in e.NewItem.RequestedProgram.RequestedSubPrograms
                         from rec in sub.RequestedRecipes
                         from exe in rec.Executors
                         select new ExecutorViewModel(exe, _batteryRepository, _chamberRepository, _testerRepository)).ToList();
            foreach (var vm in viewModels)
                this.AllExecutors.Add(vm);
        }

        #endregion // Event Handling Methods
    }
}