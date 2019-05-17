using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Diagnostics;
using System.Linq;
using System.Windows.Data;
using O2Micro.BCLabManager.Shell.DataAccess;
using O2Micro.BCLabManager.Shell.Model;
using O2Micro.BCLabManager.Shell.Properties;

namespace O2Micro.BCLabManager.Shell.ViewModel
{
    /// <summary>
    /// The ViewModel for the application's main window.
    /// </summary>
    public class MainWindowViewModel : ViewModelBase
    {
        #region Fields

        ReadOnlyCollection<CommandViewModel> _commands;
        ObservableCollection<WorkspaceViewModel> _workspaces;
        Repositories _repositories;

        #endregion // Fields

        #region Constructor

        public MainWindowViewModel(string customerDataFile)
        {
            base.DisplayName = Resources.MainWindowViewModel_DisplayName;
            _repositories = new Repositories();
        }

        #endregion // Constructor

        #region Commands

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
                    Resources.MainWindowViewModel_Command_ViewAllBatteryTypes,
                    new RelayCommand(param => this.ShowAllBatteryTypes())),

                new CommandViewModel(
                    Resources.MainWindowViewModel_Command_CreateNewBatteryType,
                    new RelayCommand(param => this.CreateNewBatteryType())),

                new CommandViewModel(
                    Resources.MainWindowViewModel_Command_ViewAllBatteries,
                    new RelayCommand(param => this.ShowAllBatteries())),

                new CommandViewModel(
                    Resources.MainWindowViewModel_Command_CreateNewBattery,
                    new RelayCommand(param => this.CreateNewBattery())),
                    
                new CommandViewModel(
                    Resources.MainWindowViewModel_Command_ViewAllRequests,
                    new RelayCommand(param => this.ShowAllRequests())),

                new CommandViewModel(
                    Resources.MainWindowViewModel_Command_CreateNewRequest,
                    new RelayCommand(param => this.CreateNewRequest())),

                new CommandViewModel(
                    Resources.MainWindowViewModel_Command_ViewAllExecutors,
                    new RelayCommand(param => this.ShowAllExecutors()))
            };
        }

        #endregion // Commands

        #region Workspaces

        /// <summary>
        /// Returns the collection of available workspaces to display.
        /// A 'workspace' is a ViewModel that can request to be closed.
        /// </summary>
        public ObservableCollection<WorkspaceViewModel> Workspaces
        {
            get
            {
                if (_workspaces == null)
                {
                    _workspaces = new ObservableCollection<WorkspaceViewModel>();
                    _workspaces.CollectionChanged += this.OnWorkspacesChanged;
                }
                return _workspaces;
            }
        }

        void OnWorkspacesChanged(object sender, NotifyCollectionChangedEventArgs e)
        {
            if (e.NewItems != null && e.NewItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.NewItems)
                    workspace.RequestClose += this.OnWorkspaceRequestClose;

            if (e.OldItems != null && e.OldItems.Count != 0)
                foreach (WorkspaceViewModel workspace in e.OldItems)
                    workspace.RequestClose -= this.OnWorkspaceRequestClose;
        }

        void OnWorkspaceRequestClose(object sender, EventArgs e)
        {
            WorkspaceViewModel workspace = sender as WorkspaceViewModel;
            workspace.Dispose();
            this.Workspaces.Remove(workspace);
        }

        #endregion // Workspaces

        #region public interface
        public void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        #endregion

        #region Private Helpers

        void CreateNewBatteryType()
        {
            BatteryTypeClass newBatteryType = new BatteryTypeClass();
            BatteryTypeViewModel workspace = new BatteryTypeViewModel(newBatteryType, _repositories._batterytypeRepository);
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        void ShowAllBatteryTypes()
        {
            AllBatteryTypesViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllBatteryTypesViewModel)
                as AllBatteryTypesViewModel;

            if (workspace == null)
            {
                workspace = new AllBatteryTypesViewModel(this, _repositories._batterytypeRepository);
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        void CreateNewBattery()
        {
            BatteryClass newBattery = new BatteryClass();
            BatteryViewModel workspace = new BatteryViewModel(newBattery, _repositories._batteryRepository, _repositories._batterytypeRepository);
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        void ShowAllBatteries()
        {
            AllBatteriesViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllBatteriesViewModel)
                as AllBatteriesViewModel;

            if (workspace == null)
            {
                workspace = new AllBatteriesViewModel(_repositories._batteryRepository);
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        void CreateNewRequest()
        {
            RequestClass newRequest = new RequestClass();
            RequestViewModel workspace = new RequestViewModel(newRequest, _repositories._requestRepository, _repositories._programRepository, _repositories._batteryRepository);
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }

        void ShowAllRequests()
        {
            AllRequestsViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllRequestsViewModel)
                as AllRequestsViewModel;

            if (workspace == null)
            {
                workspace = new AllRequestsViewModel(_repositories._requestRepository);
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        void ShowAllExecutors()
        {
            AllExecutorsViewModel workspace =
                this.Workspaces.FirstOrDefault(vm => vm is AllExecutorsViewModel)
                as AllExecutorsViewModel;

            if (workspace == null)
            {
                workspace = new AllExecutorsViewModel(_repositories._requestRepository, _repositories._batteryRepository, _repositories._chamberRepository, _repositories._testerRepository);
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        #endregion // Private Helpers
    }
}