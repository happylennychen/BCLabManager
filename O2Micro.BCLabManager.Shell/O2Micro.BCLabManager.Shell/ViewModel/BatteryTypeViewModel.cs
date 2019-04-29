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
    public class BatteryTypeViewModel : WorkspaceViewModel//, IDataErrorInfo
    {
        #region Fields

        readonly BatteryTypeClass _batterytype;
        readonly BatteryTypeRepository _batterytypeRepository;
        //bool _isSelected;
        RelayCommand _saveCommand;

        #endregion // Fields

        #region Constructor

        public BatteryTypeViewModel(BatteryTypeClass batterytype, BatteryTypeRepository batteryModelRepository)
        {
            if (batterytype == null)
                throw new ArgumentNullException("batterytype");

            if (batteryModelRepository == null)
                throw new ArgumentNullException("batteryModelRepository");

            _batterytype = batterytype;
            _batterytypeRepository = batteryModelRepository;
        }

        #endregion // Constructor

        #region BatteryTypeClass Properties

        public string Manufactor
        {
            get { return _batterytype.Manufactor; }
            set
            {
                if (value == _batterytype.Manufactor)
                    return;

                _batterytype.Manufactor = value;

                base.OnPropertyChanged("Manufactor");
            }
        }

        public string Name
        {
            get { return _batterytype.Name; }
            set
            {
                if (value == _batterytype.Name)
                    return;

                _batterytype.Name = value;

                base.OnPropertyChanged("Name");
            }
        }

        public string Material
        {
            get { return _batterytype.Material; }
            set
            {
                if (value == _batterytype.Material)
                    return;

                _batterytype.Material = value;

                base.OnPropertyChanged("Material");
            }
        }

        #endregion // Customer Properties

        #region Presentation Properties


        public override string DisplayName
        {
            get
            {
                if (this.IsNewBatteryType)
                {
                    return Resources.BatteryTypeViewModel_DisplayName;
                }
                else
                {
                    return _batterytype.Name;
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
            //if (!_batterytype.IsValid)
                //throw new InvalidOperationException(Resources.BatteryTypeViewModel_Exception_CannotSave);

            if (this.IsNewBatteryType)
                _batterytypeRepository.AddItem(_batterytype);
            
            base.OnPropertyChanged("DisplayName");
        }

        #endregion // Public Methods

        #region Private Helpers

        /// <summary>
        /// Returns true if this customer was created by the user and it has not yet
        /// been saved to the customer repository.
        /// </summary>
        bool IsNewBatteryType
        {
            get { return !_batterytypeRepository.ContainsItem(_batterytype); }
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