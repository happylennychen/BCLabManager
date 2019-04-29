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
    public class AllBatteryTypesViewModel : WorkspaceViewModel
    {
        #region Fields

        readonly BatteryTypeRepository _batterytypeRepository;

        #endregion // Fields

        #region Constructor

        public AllBatteryTypesViewModel(BatteryTypeRepository batterytypeRepository)
        {
            if (batterytypeRepository == null)
                throw new ArgumentNullException("batterytypeRepository");

            base.DisplayName = Resources.AllBatteryTypesViewModel_DisplayName;            

            _batterytypeRepository = batterytypeRepository;

            // Subscribe for notifications of when a new customer is saved.
            _batterytypeRepository.ItemAdded += this.OnBatteryModelAddedToRepository;

            // Populate the AllCustomers collection with BatteryModelViewModels.
            this.CreateAllBatteryModels();           
        }

        void CreateAllBatteryModels()
        {
            List<BatteryTypeViewModel> all =
                (from bat in _batterytypeRepository.GetItems()
                 select new BatteryTypeViewModel(bat, _batterytypeRepository)).ToList();

            //foreach (BatteryModelViewModel batmod in all)
                //batmod.PropertyChanged += this.OnBatteryModelViewModelPropertyChanged;

            this.AllBatteryModels = new ObservableCollection<BatteryTypeViewModel>(all);
            //this.AllCustomers.CollectionChanged += this.OnCollectionChanged;
        }

        #endregion // Constructor

        #region Public Interface

        /// <summary>
        /// Returns a collection of all the BatteryModelViewModel objects.
        /// </summary>
        public ObservableCollection<BatteryTypeViewModel> AllBatteryModels { get; private set; }


        #endregion // Public Interface

        #region  Base Class Overrides

        protected override void OnDispose()
        {
            foreach (BatteryTypeViewModel custVM in this.AllBatteryModels)
                custVM.Dispose();

            this.AllBatteryModels.Clear();
            //this.AllBatteryModels.CollectionChanged -= this.OnCollectionChanged;

            _batterytypeRepository.ItemAdded -= this.OnBatteryModelAddedToRepository;
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

        void OnBatteryModelAddedToRepository(object sender, ItemAddedEventArgs<BatteryTypeClass> e)
        {
            var viewModel = new BatteryTypeViewModel(e.NewItem, _batterytypeRepository);
            this.AllBatteryModels.Add(viewModel);
        }

        #endregion // Event Handling Methods
    }
}