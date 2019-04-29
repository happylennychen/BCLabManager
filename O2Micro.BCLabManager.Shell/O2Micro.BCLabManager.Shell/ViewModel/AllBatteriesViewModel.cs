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
    /// Represents a container of BatteryModelViewModel objects
    /// that has support for staying synchronized with the
    /// BatteryRepository.  This class also provides information
    /// related to multiple selected customers.
    /// </summary>
    public class AllBatteriesViewModel : WorkspaceViewModel
    {
        #region Fields

        readonly BatteryRepository _batteryRepository;

        #endregion // Fields

        #region Constructor

        public AllBatteriesViewModel(BatteryRepository batteryRepository)
        {
            if (batteryRepository == null)
                throw new ArgumentNullException("batteryRepository");

            base.DisplayName = Resources.AllBatteriesViewModel_DisplayName;

            _batteryRepository = batteryRepository;

            // Subscribe for notifications of when a new customer is saved.
            _batteryRepository.ItemAdded += this.OnBatteryAddedToRepository;

            // Populate the AllCustomers collection with BatteryModelViewModels.
            this.CreateAllBatteries();
        }

        void CreateAllBatteries()
        {
            List<BatteryViewModel> all =
                (from bat in _batteryRepository.GetItems()
                 select new BatteryViewModel(bat, _batteryRepository)).ToList();

            //foreach (BatteryModelViewModel batmod in all)
            //batmod.PropertyChanged += this.OnBatteryModelViewModelPropertyChanged;

            this.AllBatteries = new ObservableCollection<BatteryViewModel>(all);
            //this.AllCustomers.CollectionChanged += this.OnCollectionChanged;
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Returns a collection of all the BatteryViewModel objects.
        /// </summary>
        public ObservableCollection<BatteryViewModel> AllBatteries { get; private set; }


        #endregion // Public Interface

        #region  Base Class Overrides

        protected override void OnDispose()
        {
            foreach (BatteryViewModel custVM in this.AllBatteries)
                custVM.Dispose();

            this.AllBatteries.Clear();
            //this.AllBatteryModels.CollectionChanged -= this.OnCollectionChanged;

            _batteryRepository.ItemAdded -= this.OnBatteryAddedToRepository;
        }

        #endregion // Base Class Overrides

        #region Event Handling Methods

        /*void OnCollectionChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (BatteryModelViewModel batmodVM in e.NewItems)
                    batmodVM.PropertyChanged += this.OnBatteryModelViewModelPropertyChanged;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (BatteryModelViewModel batmodVM in e.OldItems)
                    batmodVM.PropertyChanged -= this.OnBatteryModelViewModelPropertyChanged;
        }*/

        /*void OnBatteryModelViewModelPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            string IsSelected = "IsSelected";

            // Make sure that the property name we're referencing is valid.
            // This is a debugging technique, and does not execute in a Release build.
            (sender as BatteryModelViewModel).VerifyPropertyName(IsSelected);

            // When a customer is selected or unselected, we must let the
            // world know that the TotalSelectedSales property has changed,
            // so that it will be queried again for a new value.
            if (e.PropertyName == IsSelected)
                this.OnPropertyChanged("TotalSelectedSales");
        }*/

        void OnBatteryAddedToRepository(object sender, ItemAddedEventArgs<BatteryClass> e)
        {
            var viewModel = new BatteryViewModel(e.NewItem, _batteryRepository);
            this.AllBatteries.Add(viewModel);
        }

        #endregion // Event Handling Methods
    }
}