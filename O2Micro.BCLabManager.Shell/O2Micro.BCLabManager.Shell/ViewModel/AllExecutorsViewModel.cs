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

        readonly RequestRepository _requestRepository;
        readonly ExecutorRepository _executorRepository;

        #endregion // Fields

        #region Constructor

        public AllExecutorsViewModel(ExecutorRepository executorRepository, RequestRepository requestRepository)
        {
            if (executorRepository == null)
                throw new ArgumentNullException("executorRepository");

            if (requestRepository == null)
                throw new ArgumentNullException("requestRepository");

            base.DisplayName = Resources.AllExecutorsViewModel_DisplayName;

            _executorRepository = executorRepository;

            // Subscribe for notifications of when a new customer is saved.
            _executorRepository.ItemAdded += this.OnExecutorAddedToRepository;

            _requestRepository = requestRepository;

            // Subscribe for notifications of when a new customer is saved.
            _requestRepository.ItemAdded += this.OnRequestAddedToRepository;

            // Populate the AllCustomers collection with RequestModelViewModels.
            this.CreateAllExecutors();
        }

        void CreateAllExecutors()
        {
            List<ExecutorViewModel> all =
                (from exe in _executorRepository.GetItems()
                 select new ExecutorViewModel(exe, _executorRepository)).ToList();

            //foreach (RequestModelViewModel batmod in all)
            //batmod.PropertyChanged += this.OnRequestModelViewModelPropertyChanged;

            this.AllExecutors = new ObservableCollection<ExecutorViewModel>(all);
            //this.AllCustomers.CollectionChanged += this.OnCollectionChanged;
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Returns a collection of all the ExecutorViewModel objects.
        /// </summary>
        public ObservableCollection<ExecutorViewModel> AllExecutors { get; private set; }


        #endregion // Public Interface

        #region  Base Class Overrides

        protected override void OnDispose()
        {
            foreach (ExecutorViewModel reqVM in this.AllExecutors)
                reqVM.Dispose();

            this.AllExecutors.Clear();
            //this.AllRequestModels.CollectionChanged -= this.OnCollectionChanged;

            _executorRepository.ItemAdded -= this.OnExecutorAddedToRepository;
        }

        #endregion // Base Class Overrides

        #region Event Handling Methods

        /*void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (RequestModelViewModel batmodVM in e.NewItems)
                    batmodVM.PropertyChanged += this.OnRequestModelViewModelPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (RequestModelViewModel batmodVM in e.OldItems)
                    batmodVM.PropertyChanged -= this.OnRequestModelViewModelPropertyChanged;
        }*/

        /*void OnRequestModelViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            // Make sure that the property name we're referencing is valid.
            // This is a debugging technique, and does not execute in a Release build.
            (sender as RequestModelViewModel).VerifyPropertyName(IsSelected);

            // When a customer is selected or unselected, we must let the
            // world know that the TotalSelectedSales property has changed,
            // so that it will be queried again for a new value.
            if (e.PropertyName == IsSelected)
                this.OnPropertyChanged("TotalSelectedSales");
        }*/

        void OnExecutorAddedToRepository(object sender, ItemAddedEventArgs<ExecutorClass> e)
        {
            var viewModel = new ExecutorViewModel(e.NewItem, _executorRepository);
            this.AllExecutors.Add(viewModel);
        }

        void OnRequestAddedToRepository(object sender, ItemAddedEventArgs<RequestClass> e)
        {
            //var viewModel = new RequestViewModel(e.NewItem, _requestRepository);
            //this.AllRequests.Add(viewModel);
            var executors = (from sp in e.NewItem.RequestedProgram.RequestedSubPrograms
             from rec in sp.RequestedRecipes
             from exe in rec.Executors
             select exe).ToList();
            foreach(var exe in executors)
                _executorRepository.AddItem(exe);
        }

        #endregion // Event Handling Methods
    }
}