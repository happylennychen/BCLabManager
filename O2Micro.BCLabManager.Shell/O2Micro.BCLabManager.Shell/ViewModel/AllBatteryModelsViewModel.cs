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
    /// BatteryModelRepository.  This class also provides information
    /// related to multiple selected customers.
    /// </summary>
    public class AllBatteryModelsViewModel : WorkspaceViewModel
    {
        #region Fields

        readonly BatteryModelRepository _batterymodelRepository;

        #endregion // Fields

        #region Constructor

        public AllBatteryModelsViewModel(BatteryModelRepository batterymodelRepository)
        {
            if (batterymodelRepository == null)
                throw new ArgumentNullException("batterymodelRepository");

            base.DisplayName = Resources.AllBatteryModelsViewModel_DisplayName;            

            _batterymodelRepository = batterymodelRepository;

            // Subscribe for notifications of when a new customer is saved.
            _batterymodelRepository.ItemAdded += this.OnBatteryModelAddedToRepository;

            // Populate the AllCustomers collection with BatteryModelViewModels.
            this.CreateAllBatteryModels();           
        }

        void CreateAllBatteryModels()
        {
            List<BatteryModelViewModel> all =
                (from bat in _batterymodelRepository.GetItems()
                 select new BatteryModelViewModel(bat, _batterymodelRepository)).ToList();

            //foreach (BatteryModelViewModel batmod in all)
                //batmod.PropertyChanged += this.OnBatteryModelViewModelPropertyChanged;

            this.AllBatteryModels = new ObservableCollection<BatteryModelViewModel>(all);
            //this.AllCustomers.CollectionChanged += this.OnCollectionChanged;
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Returns a collection of all the BatteryModelViewModel objects.
        /// </summary>
        public ObservableCollection<BatteryModelViewModel> AllBatteryModels { get; private set; }


        #endregion // Public Interface

        #region  Base Class Overrides

        protected override void OnDispose()
        {
            foreach (BatteryModelViewModel custVM in this.AllBatteryModels)
                custVM.Dispose();

            this.AllBatteryModels.Clear();
            //this.AllBatteryModels.CollectionChanged -= this.OnCollectionChanged;

            _batterymodelRepository.ItemAdded -= this.OnBatteryModelAddedToRepository;
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

        void OnBatteryModelAddedToRepository(object sender, ItemAddedEventArgs<BatteryModelClass> e)
        {
            var viewModel = new BatteryModelViewModel(e.NewItem, _batterymodelRepository);
            this.AllBatteryModels.Add(viewModel);
        }

        #endregion // Event Handling Methods
    }
}