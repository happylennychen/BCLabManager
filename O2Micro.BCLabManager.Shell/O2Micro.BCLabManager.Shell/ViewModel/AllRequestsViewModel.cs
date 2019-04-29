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
    /// RequestRepository.  This class also provides information
    /// related to multiple selected customers.
    /// </summary>
    public class AllRequestsViewModel : WorkspaceViewModel
    {
        #region Fields

        readonly RequestRepository _RequestRepository;

        #endregion // Fields

        #region Constructor

        public AllRequestsViewModel(RequestRepository RequestRepository)
        {
            if (RequestRepository == null)
                throw new ArgumentNullException("RequestRepository");

            base.DisplayName = Resources.AllRequestsViewModel_DisplayName;

            _RequestRepository = RequestRepository;

            // Subscribe for notifications of when a new customer is saved.
            _RequestRepository.ItemAdded += this.OnRequestAddedToRepository;

            // Populate the AllCustomers collection with RequestModelViewModels.
            this.CreateAllRequests();
        }

        void CreateAllRequests()
        {
            List<RequestViewModel> all =
                (from req in _RequestRepository.GetItems()
                 select new RequestViewModel(req, _RequestRepository)).ToList();

            //foreach (RequestModelViewModel batmod in all)
            //batmod.PropertyChanged += this.OnRequestModelViewModelPropertyChanged;

            this.AllRequests = new ObservableCollection<RequestViewModel>(all);
            //this.AllCustomers.CollectionChanged += this.OnCollectionChanged;
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Returns a collection of all the RequestViewModel objects.
        /// </summary>
        public ObservableCollection<RequestViewModel> AllRequests { get; private set; }


        #endregion // Public Interface

        #region  Base Class Overrides

        protected override void OnDispose()
        {
            foreach (RequestViewModel reqVM in this.AllRequests)
                reqVM.Dispose();

            this.AllRequests.Clear();
            //this.AllRequestModels.CollectionChanged -= this.OnCollectionChanged;

            _RequestRepository.ItemAdded -= this.OnRequestAddedToRepository;
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

        void OnRequestAddedToRepository(object sender, ItemAddedEventArgs<RequestClass> e)
        {
            var viewModel = new RequestViewModel(e.NewItem, _RequestRepository);
            this.AllRequests.Add(viewModel);
        }

        #endregion // Event Handling Methods
    }
}