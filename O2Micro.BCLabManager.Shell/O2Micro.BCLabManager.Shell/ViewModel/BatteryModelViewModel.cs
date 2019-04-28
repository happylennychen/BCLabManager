using System;
using System.ComponentModel;
using System.Windows.Input;
using O2Micro.BCLabManager.Shell.DataAccess;
using O2Micro.BCLabManager.Shell.Model;
using O2Micro.BCLabManager.Shell.Properties;

namespace O2Micro.BCLabManager.Shell.ViewModel
{
    /// <summary>
    /// A UI-friendly wrapper for a Customer object.
    /// </summary>
    public class BatteryModelViewModel : WorkspaceViewModel//, IDataErrorInfo
    {
        #region Fields

        readonly BatteryModelClass _batterymodel;
        readonly BatteryModelRepository _batterymodelRepository;
        //bool _isSelected;
        RelayCommand _saveCommand;

        #endregion // Fields

        #region Constructor

        public BatteryModelViewModel(BatteryModelClass batterymodel, BatteryModelRepository batteryModelRepository)
        {
            if (batterymodel == null)
                throw new ArgumentNullException("batterymodel");

            if (batteryModelRepository == null)
                throw new ArgumentNullException("batteryModelRepository");

            _batterymodel = batterymodel;
            _batterymodelRepository = batteryModelRepository;
        }

        #endregion // Constructor

        #region BatteryModelClass Properties

        public string Manufactor
        {
            get { return _batterymodel.Manufactor; }
            set
            {
                if (value == _batterymodel.Manufactor)
                    return;

                _batterymodel.Manufactor = value;

                base.OnPropertyChanged("Manufactor");
            }
        }

        public string Name
        {
            get { return _batterymodel.Name; }
            set
            {
                if (value == _batterymodel.Name)
                    return;

                _batterymodel.Name = value;

                base.OnPropertyChanged("Name");
            }
        }

        public string Material
        {
            get { return _batterymodel.Material; }
            set
            {
                if (value == _batterymodel.Material)
                    return;

                _batterymodel.Material = value;

                base.OnPropertyChanged("Material");
            }
        }

        #endregion // Customer Properties

        #region Presentation Properties


        public override string DisplayName
        {
            get
            {
                if (this.IsNewCustomer)
                {
                    return Resources.BatteryModelViewModel_DisplayName;
                }
                else
                {
                    return _batterymodel.Name;
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
            //if (!_batterymodel.IsValid)
                //throw new InvalidOperationException(Resources.BatteryModelViewModel_Exception_CannotSave);

            if (this.IsNewCustomer)
                _batterymodelRepository.AddItem(_batterymodel);
            
            base.OnPropertyChanged("DisplayName");
        }

        #endregion // Public Methods

        #region Private Helpers

        /// <summary>
        /// Returns true if this customer was created by the user and it has not yet
        /// been saved to the customer repository.
        /// </summary>
        bool IsNewCustomer
        {
            get { return !_batterymodelRepository.ContainsItem(_batterymodel); }
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