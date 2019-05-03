﻿using System;
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
    public class MainWindowViewModel : WorkspaceViewModel
    {
        #region Fields

        ReadOnlyCollection<CommandViewModel> _commands;
        BatteryTypeRepository _batterytypeRepository;
        BatteryRepository _batteryRepository;
        ChamberRepository _chamberRepository;
        TesterRepository _testerRepository;

        SubProgramRepository _subprogramRepository;
        RecipeRepository _recipeRepository;
        ChamberRecipeRepository _chamberrecipeRepository;
        TesterRecipeRepository _testerrecipeRepository;
        ProgramRepository _programRepository;
        RequestRepository _requestRepository = new RequestRepository();
        ObservableCollection<WorkspaceViewModel> _workspaces;


        private List<BatteryTypeClass> BatteryTypes;// { get; set; }
        private List<BatteryClass> Batteries;// { get; set; }
        private List<TesterClass> Testers;// { get; set; }
        private List<ChamberClass> Chambers;// { get; set; }
        private List<ProgramClass> Programs;// { get; set; }
        private List<SubProgramClass> SubPrograms;// { get; set; }
        private List<RecipeClass> Recipes;// { get; set; }
        private List<ChamberRecipeClass> ChamberRecipes;// { get; set; }
        private List<TesterRecipeClass> TesterRecipes;// { get; set; }
        private List<RequestClass> Requests;// { get; set; }

        #endregion // Fields

        #region Constructor

        public MainWindowViewModel(string customerDataFile)
        {
            base.DisplayName = Resources.MainWindowViewModel_DisplayName;

            //string folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BCLabManager Documents\\");
            //dbmanager.DBInit(folder);
            //HistoricData();
            //TestCase4_1();
            //TestCase4_2();
            //TestCase4_3();
            //TestCase5_1();
            //TestCase5_2();
            //TestCase5_3();
            //TestCase5_4();
            //TestCase5_5();
            //TestCase6_1();
            //TestCase6_2();
            //TestCase6_3();
            //TestCase7_1();
            //TestCase7_2();
            //TestCase7_3();
            //TestCase9();
            HistoricRegistration();
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
                    new RelayCommand(param => this.CreateNewRequest()))
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

        #region Private Helpers

        /*void CreateNewItem<TItemModel, TItemViewModel>()
            where TItemModel : new()
            where TItemViewModel:new(TItemModel)
        {
            TItemModel newItem = new TItemModel();
            TItemViewModel workspace = new TItemViewModel(newItem);
            this.Workspaces.Add(workspace);
            this.SetActiveWorkspace(workspace);
        }*/

        void CreateNewBatteryType()
        {
            BatteryTypeClass newBatteryType = new BatteryTypeClass();
            BatteryTypeViewModel workspace = new BatteryTypeViewModel(newBatteryType, _batterytypeRepository);
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
                workspace = new AllBatteryTypesViewModel(_batterytypeRepository);
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        void CreateNewBattery()
        {
            BatteryClass newBattery = new BatteryClass();
            BatteryViewModel workspace = new BatteryViewModel(newBattery, _batteryRepository, _batterytypeRepository);
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
                workspace = new AllBatteriesViewModel(_batteryRepository);
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        void CreateNewRequest()
        {
            RequestClass newRequest = new RequestClass();
            RequestViewModel workspace = new RequestViewModel(newRequest, _requestRepository, _programRepository, _batteryRepository);
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
                workspace = new AllRequestsViewModel(_requestRepository);
                this.Workspaces.Add(workspace);
            }

            this.SetActiveWorkspace(workspace);
        }

        void SetActiveWorkspace(WorkspaceViewModel workspace)
        {
            Debug.Assert(this.Workspaces.Contains(workspace));

            ICollectionView collectionView = CollectionViewSource.GetDefaultView(this.Workspaces);
            if (collectionView != null)
                collectionView.MoveCurrentTo(workspace);
        }

        #endregion // Private Helpers
        #region debugger
        [Conditional("DEBUG")]
        private void TestCase4_1()
        {
            InitAssets();
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(1, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, cbrrec1);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 3);

            TesterRecipeClass tstrec2 = new TesterRecipeClass(Testers[0], "tstrec2", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec2 = new ChamberRecipeClass(1, Chambers[0], "cbrrec2", "Blablabla");
            RecipeClass rec2 = new RecipeClass(tstrec2, null);
            TesterRecipeClass tstrec2_2 = new TesterRecipeClass(Testers[0], "tstrec2_2", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec2_2 = new ChamberRecipeClass(1, Chambers[0], "cbrrec2_2", "Blablabla");
            RecipeClass rec2_2 = new RecipeClass(tstrec2_2, cbrrec2_2);
            SubProgramClass subpro2 = new SubProgramClass(new List<RecipeClass> { rec2, rec2_2 });
            ProgramClass pro2 = new ProgramClass(BatteryTypes[0], "pro2", new List<SubProgramClass> { subpro2 });
            RequestClass req2 = new RequestClass(pro2, "B", DateTime.Now, 2);

            TesterRecipeClass tstrec3 = new TesterRecipeClass(Testers[0], "tstrec3", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec3 = new ChamberRecipeClass(1, Chambers[0], "cbrrec3", "Blablabla");
            RecipeClass rec3 = new RecipeClass(tstrec3, cbrrec3);
            SubProgramClass subpro3 = new SubProgramClass(new List<RecipeClass> { rec3 });
            ProgramClass pro3 = new ProgramClass(BatteryTypes[0], "pro3", new List<SubProgramClass> { subpro3 });
            RequestClass req3 = new RequestClass(pro3, "C", DateTime.Now, 1);

            TesterRecipeClass tstrec4 = new TesterRecipeClass(Testers[0], "tstrec4", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec4 = null;
            RecipeClass rec4 = new RecipeClass(tstrec4, cbrrec4);
            SubProgramClass subpro4 = new SubProgramClass(new List<RecipeClass> { rec4 });
            ProgramClass pro4 = new ProgramClass(BatteryTypes[0], "pro4", new List<SubProgramClass> { subpro4 });
            RequestClass req4 = new RequestClass(pro4, "D", DateTime.Now, 0);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);
            _requestRepository.AddItem(req2);
            _requestRepository.AddItem(req3);
            _requestRepository.AddItem(req4);
            PrintScheduler(sch);
            //sch.OrderTasks();     //pro4,pro3,pro2,pro1
            sch.OrderTasks();
            PrintScheduler(sch);
        }

        [Conditional("DEBUG")]
        private void TestCase4_2()
        {
            InitAssets();
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(3, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, cbrrec1);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 1);

            TesterRecipeClass tstrec2 = new TesterRecipeClass(Testers[0], "tstrec2", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec2 = new ChamberRecipeClass(1, Chambers[0], "cbrrec2", "Blablabla");
            RecipeClass rec2 = new RecipeClass(tstrec2, null);
            TesterRecipeClass tstrec2_2 = new TesterRecipeClass(Testers[0], "tstrec2_2", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec2_2 = new ChamberRecipeClass(2, Chambers[0], "cbrrec2_2", "Blablabla");
            RecipeClass rec2_2 = new RecipeClass(tstrec2_2, cbrrec2_2);
            SubProgramClass subpro2 = new SubProgramClass(new List<RecipeClass> { rec2, rec2_2 });
            ProgramClass pro2 = new ProgramClass(BatteryTypes[0], "pro2", new List<SubProgramClass> { subpro2 });
            RequestClass req2 = new RequestClass(pro2, "B", DateTime.Now, 1);

            TesterRecipeClass tstrec3 = new TesterRecipeClass(Testers[0], "tstrec3", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec3 = new ChamberRecipeClass(1, Chambers[0], "cbrrec3", "Blablabla");
            RecipeClass rec3 = new RecipeClass(tstrec3, cbrrec3);
            SubProgramClass subpro3 = new SubProgramClass(new List<RecipeClass> { rec3 });
            ProgramClass pro3 = new ProgramClass(BatteryTypes[0], "pro3", new List<SubProgramClass> { subpro3 });
            RequestClass req3 = new RequestClass(pro3, "C", DateTime.Now, 1);

            TesterRecipeClass tstrec4 = new TesterRecipeClass(Testers[0], "tstrec4", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec4 = null;
            RecipeClass rec4 = new RecipeClass(tstrec4, cbrrec4);
            SubProgramClass subpro4 = new SubProgramClass(new List<RecipeClass> { rec4 });
            ProgramClass pro4 = new ProgramClass(BatteryTypes[0], "pro4", new List<SubProgramClass> { subpro4 });
            RequestClass req4 = new RequestClass(pro4, "D", DateTime.Now, 1);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);
            _requestRepository.AddItem(req2);
            _requestRepository.AddItem(req3);
            _requestRepository.AddItem(req4);
            PrintScheduler(sch);
            sch.OrderTasks();
            PrintScheduler(sch);
        }

        [Conditional("DEBUG")]
        private void TestCase4_3()
        {
            InitAssets();
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(3, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, cbrrec1);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 1);

            TesterRecipeClass tstrec2 = new TesterRecipeClass(Testers[0], "tstrec2", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec2 = new ChamberRecipeClass(1, Chambers[0], "cbrrec2", "Blablabla");
            RecipeClass rec2 = new RecipeClass(tstrec2, null);
            TesterRecipeClass tstrec2_2 = new TesterRecipeClass(Testers[0], "tstrec2_2", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec2_2 = new ChamberRecipeClass(2, Chambers[0], "cbrrec2_2", "Blablabla");
            RecipeClass rec2_2 = new RecipeClass(tstrec2_2, cbrrec2_2);
            SubProgramClass subpro2 = new SubProgramClass(new List<RecipeClass> { rec2, rec2_2 });
            ProgramClass pro2 = new ProgramClass(BatteryTypes[0], "pro2", new List<SubProgramClass> { subpro2 });
            RequestClass req2 = new RequestClass(pro2, "B", DateTime.Now, 1);

            TesterRecipeClass tstrec3 = new TesterRecipeClass(Testers[0], "tstrec3", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec3 = new ChamberRecipeClass(1, Chambers[0], "cbrrec3", "Blablabla");
            RecipeClass rec3 = new RecipeClass(tstrec3, cbrrec3);
            SubProgramClass subpro3 = new SubProgramClass(new List<RecipeClass> { rec3 });
            ProgramClass pro3 = new ProgramClass(BatteryTypes[0], "pro3", new List<SubProgramClass> { subpro3 });
            RequestClass req3 = new RequestClass(pro3, "C", DateTime.Now, 1);

            TesterRecipeClass tstrec4 = new TesterRecipeClass(Testers[0], "tstrec4", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec4 = null;
            RecipeClass rec4 = new RecipeClass(tstrec4, cbrrec4);
            SubProgramClass subpro4 = new SubProgramClass(new List<RecipeClass> { rec4 });
            ProgramClass pro4 = new ProgramClass(BatteryTypes[0], "pro4", new List<SubProgramClass> { subpro4 });
            RequestClass req4 = new RequestClass(pro4, "D", DateTime.Now, 1);
            //sch.OrderTasks();

            TesterRecipeClass tstrec5 = new TesterRecipeClass(Testers[0], "tstrec5", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec5 = new ChamberRecipeClass(1, Chambers[0], "cbrrec5", "Blablabla");
            RecipeClass rec5 = new RecipeClass(tstrec5, cbrrec5);
            SubProgramClass subpro5 = new SubProgramClass(new List<RecipeClass> { rec5 });
            ProgramClass pro5 = new ProgramClass(BatteryTypes[0], "pro5", new List<SubProgramClass> { subpro5 });
            RequestClass req5 = new RequestClass(pro5, "A", DateTime.Now, 0);

            TesterRecipeClass tstrec6 = new TesterRecipeClass(Testers[0], "tstrec6", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec6 = new ChamberRecipeClass(1, Chambers[0], "cbrrec6", "Blablabla");
            RecipeClass rec6 = new RecipeClass(tstrec6, null);
            TesterRecipeClass tstrec6_2 = new TesterRecipeClass(Testers[0], "tstrec6_2", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec6_2 = new ChamberRecipeClass(1, Chambers[0], "cbrrec6_2", "Blablabla");
            RecipeClass rec6_2 = new RecipeClass(tstrec6_2, cbrrec6_2);
            SubProgramClass subpro6 = new SubProgramClass(new List<RecipeClass> { rec6, rec6_2 });
            ProgramClass pro6 = new ProgramClass(BatteryTypes[0], "pro6", new List<SubProgramClass> { subpro6 });
            RequestClass req6 = new RequestClass(pro6, "B", DateTime.Now, 0);

            TesterRecipeClass tstrec7 = new TesterRecipeClass(Testers[0], "tstrec7", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec7 = new ChamberRecipeClass(1, Chambers[0], "cbrrec7", "Blablabla");
            RecipeClass rec7 = new RecipeClass(tstrec7, cbrrec7);
            SubProgramClass subpro7 = new SubProgramClass(new List<RecipeClass> { rec7 });
            ProgramClass pro7 = new ProgramClass(BatteryTypes[0], "pro7", new List<SubProgramClass> { subpro7 });
            RequestClass req7 = new RequestClass(pro7, "C", DateTime.Now, 0);

            TesterRecipeClass tstrec8 = new TesterRecipeClass(Testers[0], "tstrec8", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec8 = null;
            RecipeClass rec8 = new RecipeClass(tstrec8, cbrrec8);
            SubProgramClass subpro8 = new SubProgramClass(new List<RecipeClass> { rec8 });
            ProgramClass pro8 = new ProgramClass(BatteryTypes[0], "pro8", new List<SubProgramClass> { subpro8 });
            RequestClass req8 = new RequestClass(pro8, "D", DateTime.Now, 0);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);
            _requestRepository.AddItem(req2);
            _requestRepository.AddItem(req3);
            _requestRepository.AddItem(req4);
            _requestRepository.AddItem(req5);
            _requestRepository.AddItem(req6);
            _requestRepository.AddItem(req7);
            _requestRepository.AddItem(req8);
            PrintScheduler(sch);
            sch.OrderTasks();
            PrintScheduler(sch);
        }

        [Conditional("DEBUG")]
        private void TestCase5_1()
        {
            InitAssets();
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[0], "Blablabla");
            //ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(3, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, null);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 1);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);
            PrintAssetsPool();
            sch.AssignAssets(Batteries[0], Chambers[0], Testers[0].TesterChannels[0]);
            PrintAssetsPool();
        }

        private void TestCase5_2()
        {
            InitAssets();
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[0], "Blablabla");
            //ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(3, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, null);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 1);

            TesterRecipeClass tstrec3 = new TesterRecipeClass(Testers[0], "tstrec3", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec3 = new ChamberRecipeClass(1, Chambers[0], "cbrrec3", "Blablabla");
            RecipeClass rec3 = new RecipeClass(tstrec3, cbrrec3);
            SubProgramClass subpro3 = new SubProgramClass(new List<RecipeClass> { rec3 });
            ProgramClass pro3 = new ProgramClass(BatteryTypes[0], "pro3", new List<SubProgramClass> { subpro3 });
            RequestClass req3 = new RequestClass(pro3, "C", DateTime.Now, 1);

            PrintAssetsPool();

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);
            _requestRepository.AddItem(req3);
            sch.AssignAssets(Batteries[0], Chambers[0], Testers[0].TesterChannels[0]);
            PrintAssetsPool();
            sch.AssignAssets(Batteries[0], Chambers[0], Testers[0].TesterChannels[0]);
            PrintAssetsPool();
        }

        [Conditional("DEBUG")]
        private void TestCase5_3()
        {
            InitAssets();
            TesterClass Tester = new TesterClass(2, "XXX", "YYY", null);
            Tester.TesterChannels = new List<TesterChannelClass> { 
                new TesterChannelClass(1,Tester),
                new TesterChannelClass(2,Tester),};
            Testers.Add(Tester);
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[1], "tstrec1", BatteryTypes[0], "Blablabla");
            //ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(3, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, null);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 1);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);
            PrintAssetsPool();
            sch.AssignAssets(Batteries[0], Chambers[0], Testers[0].TesterChannels[0]);
            PrintAssetsPool();
        }

        [Conditional("DEBUG")]
        private void TestCase5_4()
        {
            InitAssets();
            BatteryTypeClass BatteryType = new BatteryTypeClass("Oppo", "xxx", "Li", 4400, 2340, 3700, 2340, 2200);
            BatteryTypes.Add(BatteryType);
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[1], "Blablabla");
            //ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(3, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, null);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 1);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);
            PrintAssetsPool();
            sch.AssignAssets(Batteries[0], Chambers[0], Testers[0].TesterChannels[0]);
            PrintAssetsPool();
        }

        [Conditional("DEBUG")]
        private void TestCase5_5()
        {
            InitAssets();
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[0], "Blablabla");
            //ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(3, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, null);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 1);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);
            PrintAssetsPool();
            sch.AssignAssets(Batteries[0], Chambers[0], Testers[0].TesterChannels[0]);
            sch.AssignAssets(Batteries[0], Chambers[0], Testers[0].TesterChannels[0]);
            PrintAssetsPool();
        }

        [Conditional("DEBUG")]
        private void TestCase6_1()
        {
            InitAssets();
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[0], "Blablabla");
            //ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(3, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, null);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 1);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);

            //PrintAssetsPool();
            PrintScheduler(sch);
            sch.AssignAssets(Batteries[0], null, Testers[0].TesterChannels[0]);
            PrintScheduler(sch);
            sch.Execute(DateTime.Now);
            //PrintAssetsPool();
            PrintScheduler(sch);
        }

        [Conditional("DEBUG")]
        private void TestCase6_2()
        {
            InitAssets();
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(1, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, cbrrec1);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 3);

            TesterRecipeClass tstrec2 = new TesterRecipeClass(Testers[0], "tstrec2", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec2 = new ChamberRecipeClass(1, Chambers[0], "cbrrec2", "Blablabla");
            RecipeClass rec2 = new RecipeClass(tstrec2, null);
            TesterRecipeClass tstrec2_2 = new TesterRecipeClass(Testers[0], "tstrec2_2", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec2_2 = new ChamberRecipeClass(1, Chambers[0], "cbrrec2_2", "Blablabla");
            RecipeClass rec2_2 = new RecipeClass(tstrec2_2, null);
            SubProgramClass subpro2 = new SubProgramClass(new List<RecipeClass> { rec2, rec2_2 });
            ProgramClass pro2 = new ProgramClass(BatteryTypes[0], "pro2", new List<SubProgramClass> { subpro2 });
            RequestClass req2 = new RequestClass(pro2, "B", DateTime.Now, 2);

            TesterRecipeClass tstrec3 = new TesterRecipeClass(Testers[0], "tstrec3", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec3 = new ChamberRecipeClass(1, Chambers[0], "cbrrec3", "Blablabla");
            RecipeClass rec3 = new RecipeClass(tstrec3, null);
            SubProgramClass subpro3 = new SubProgramClass(new List<RecipeClass> { rec3 });
            ProgramClass pro3 = new ProgramClass(BatteryTypes[0], "pro3", new List<SubProgramClass> { subpro3 });
            RequestClass req3 = new RequestClass(pro3, "C", DateTime.Now, 1);

            TesterRecipeClass tstrec4 = new TesterRecipeClass(Testers[0], "tstrec4", BatteryTypes[0], "Blablabla");
            ChamberRecipeClass cbrrec4 = null;
            RecipeClass rec4 = new RecipeClass(tstrec4, cbrrec4);
            SubProgramClass subpro4 = new SubProgramClass(new List<RecipeClass> { rec4 });
            ProgramClass pro4 = new ProgramClass(BatteryTypes[0], "pro4", new List<SubProgramClass> { subpro4 });
            RequestClass req4 = new RequestClass(pro4, "D", DateTime.Now, 0);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);
            _requestRepository.AddItem(req2);
            _requestRepository.AddItem(req3);
            _requestRepository.AddItem(req4);

            //PrintAssetsPool();
            PrintScheduler(sch);
            sch.AssignAssets(Batteries[0], Chambers[0], Testers[0].TesterChannels[0]);
            PrintScheduler(sch);
            sch.AssignAssets(Batteries[1], null, Testers[0].TesterChannels[1]);
            PrintScheduler(sch);
            sch.AssignAssets(Batteries[2], null, Testers[0].TesterChannels[2]);
            PrintScheduler(sch);
            sch.AssignAssets(Batteries[3], null, Testers[0].TesterChannels[3]);
            PrintScheduler(sch);
            sch.Execute(DateTime.Now);
            //PrintAssetsPool();
            PrintScheduler(sch);
        }

        [Conditional("DEBUG")]
        private void TestCase6_3()
        {
            InitAssets();
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[0], "Blablabla");
            //ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(3, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, null);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 1);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);

            //PrintAssetsPool();
            PrintScheduler(sch);
            //sch.AssignAssets(Batteries[0], null, Testers[0].TesterChannels[0]);
            //PrintScheduler(sch);
            sch.Execute(DateTime.Now);
            //PrintAssetsPool();
            PrintScheduler(sch);
        }

        [Conditional("DEBUG")]
        private void TestCase7_1()
        {
            InitAssets();
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[0], "Blablabla");
            //ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(3, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, null);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 1);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);

            //PrintAssetsPool();
            PrintScheduler(sch);
            sch.AssignAssets(Batteries[0], null, Testers[0].TesterChannels[0]);
            //PrintScheduler(sch);
            sch.Execute(DateTime.Now);
            //PrintAssetsPool();
            PrintScheduler(sch);
            PrintAssetsPool();
            sch.RunningList[0].TopRunningRequestedRecipe.ValidExecutor.Commit(ExecutorStatus.Completed, DateTime.Now, "blablabla");
            PrintScheduler(sch);
            PrintAssetsPool();
        }

        [Conditional("DEBUG")]
        private void TestCase7_2()
        {
            InitAssets();
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[0], "Blablabla");
            //ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(3, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, null);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 1);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);

            //PrintAssetsPool();
            PrintScheduler(sch);
            sch.AssignAssets(Batteries[0], null, Testers[0].TesterChannels[0]);
            //PrintScheduler(sch);
            sch.Execute(DateTime.Now);
            //PrintAssetsPool();
            PrintScheduler(sch);
            PrintAssetsPool();
            sch.RunningList[0].TopRunningRequestedRecipe.ValidExecutor.Commit(ExecutorStatus.Invalid, DateTime.Now, "blablabla");
            PrintScheduler(sch);
            PrintAssetsPool();
        }

        [Conditional("DEBUG")]
        private void TestCase7_3()
        {
            InitAssets();
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[0], "Blablabla");
            //ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(3, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, null);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 1);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);

            //PrintAssetsPool();
            PrintScheduler(sch);
            sch.AssignAssets(Batteries[0], null, Testers[0].TesterChannels[0]);
            //PrintScheduler(sch);
            sch.Execute(DateTime.Now);
            //PrintAssetsPool();
            PrintScheduler(sch);
            PrintAssetsPool();
            ExecutorClass exe = sch.RunningList[0].TopRunningRequestedRecipe.ValidExecutor;
            exe.Commit(ExecutorStatus.Waiting, DateTime.Now, "blablabla");
            exe.Commit(ExecutorStatus.Ready, DateTime.Now, "blablabla");
            exe.Commit(ExecutorStatus.Executing, DateTime.Now, "blablabla");
            exe.Commit(ExecutorStatus.Abandoned, DateTime.Now, "blablabla");
            exe.Commit(ExecutorStatus.Completed, DateTime.Now, "blablabla");
            exe.Commit(ExecutorStatus.Completed, DateTime.Now, "blablabla");
            PrintScheduler(sch);
            PrintAssetsPool();
        }

        [Conditional("DEBUG")]
        private void TestCase7_4()
        {
            InitAssets();
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[0], "Blablabla");
            //ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(3, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, null);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 1);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);

            //PrintAssetsPool();
            PrintScheduler(sch);
            PrintAssetsPool();
            sch.WaitingList[0].TopWaitingRequestedRecipe.ValidExecutor.Commit(ExecutorStatus.Completed, DateTime.Now, "blablabla");
            PrintScheduler(sch);
            PrintAssetsPool();
            sch.AssignAssets(Batteries[0], null, Testers[0].TesterChannels[0]);
            PrintScheduler(sch);
            PrintAssetsPool();
            sch.ReadyList[0].TopReadyRequestedRecipe.ValidExecutor.Commit(ExecutorStatus.Completed, DateTime.Now, "blablabla");
            //PrintScheduler(sch);
            //sch.Execute();
            //PrintAssetsPool();
            //sch.WaitingList[0].TopWaitingRequestedRecipe.ValidExecutor.Commit(ExecutorStatus.Completed, DateTime.Now, "blablabla");
            PrintScheduler(sch);
            PrintAssetsPool();
        }

        [Conditional("DEBUG")]
        private void TestCase9()
        {
            InitAssets();
            TesterRecipeClass tstrec1 = new TesterRecipeClass(Testers[0], "tstrec1", BatteryTypes[0], "Blablabla");
            //ChamberRecipeClass cbrrec1 = new ChamberRecipeClass(3, Chambers[0], "cbrrec", "Blablabla");
            RecipeClass rec1 = new RecipeClass(tstrec1, null);
            TesterRecipeClass tstrec2 = new TesterRecipeClass(Testers[0], "tstrec2", BatteryTypes[0], "Blablabla");
            RecipeClass rec2 = new RecipeClass(tstrec2, null);
            SubProgramClass subpro1 = new SubProgramClass(new List<RecipeClass> { rec1, rec2 });
            SubProgramClass subpro2 = new SubProgramClass(new List<RecipeClass> { rec1, rec2 });
            ProgramClass pro1 = new ProgramClass(BatteryTypes[0], "pro1", new List<SubProgramClass> { subpro1, subpro2 });
            RequestClass req1 = new RequestClass(pro1, "A", DateTime.Now, 1);

            Scheduler sch = new Scheduler(_requestRepository);
            _requestRepository.AddItem(req1);

            //PrintAssetsPool();
            PrintScheduler(sch);
            PrintAssetsPool();
            sch.AssignAssets(Batteries[0], null, Testers[0].TesterChannels[0]);
            sch.AssignAssets(Batteries[1], null, Testers[0].TesterChannels[1]);
            PrintScheduler(sch);
            PrintAssetsPool();
            sch.Execute(DateTime.Now);
            PrintScheduler(sch);
            PrintAssetsPool();
            RequestedRecipeClass rr = sch.RunningList[0].TopRunningRequestedRecipe;
            rr.ValidExecutor.Commit(ExecutorStatus.Completed, DateTime.Now, "blablabla");
            PrintScheduler(sch);
            PrintAssetsPool();
            rr.ValidExecutor.Invalidate();
            PrintScheduler(sch);
            PrintAssetsPool();
            sch.RunningList[0].TopRunningRequestedRecipe.ValidExecutor.Invalidate();
            PrintScheduler(sch);
            PrintAssetsPool();
            rr.ValidExecutor.Invalidate();
            PrintScheduler(sch);
            PrintAssetsPool();
            //sch.AssignAssets(Batteries[2], null, Testers[0].TesterChannels[2]);
            //sch.AssignAssets(Batteries[3], null, Testers[0].TesterChannels[3]);
            //PrintAssetsPool();
            //sch.WaitingList[0].TopWaitingRequestedRecipe.ValidExecutor.Commit(ExecutorStatus.Completed, DateTime.Now, DateTime.Now, "blablabla");
            //PrintScheduler(sch);
            //PrintAssetsPool();
        }

        private void PrintAssetList(String ListName, List<AssetClass> AssetList)
        {
            Debug.WriteLine(ListName);
            foreach (var asset in AssetList)
            {
                string str = "\t\t---------------------------------\n";
                switch (asset.GetType().Name)
                {
                    case "BatteryClass":
                        BatteryClass battery = (BatteryClass)asset;
                        str += "\t\t" + "Battery ID:" + battery.BatteryID + "\n";
                        str += "\t\t" + "Battery Name:" + battery.Name + "\n";
                        str += "\t\t" + "Battery Model:" + battery.BatteryType.Name + "\n";
                        str += "\t\t" + "Battery Cycle:" + battery.CycleCount.ToString() + "\n";
                        str += "\t\t" + "Battery Status:" + battery.Status + "\n";
                        break;

                    case "ChamberClass":
                        ChamberClass chamber = (ChamberClass)asset;
                        str += "\t\t" + "Chamber ID:" + chamber.ChamberID + "\n";
                        str += "\t\t" + "Chamber Name:" + chamber.Name + "\n";
                        str += "\t\t" + "Chamber Status:" + chamber.Status + "\n";
                        break;

                    case "TesterChannelClass":
                        TesterChannelClass testerChannel = (TesterChannelClass)asset;
                        str += "\t\t" + "TesterChannel ID:" + testerChannel.TesterChannelID + "\n";
                        str += "\t\t" + "Tester Name:" + testerChannel.Tester.Name + "\n";
                        str += "\t\t" + "TesterChannel Status:" + testerChannel.Status + "\n";
                        break;

                }
                str += "\t\t---------------------------------";
                Debug.WriteLine(str);
            }
        }
        private void PrintAssetsPool()
        {
            Debug.WriteLine("-------------------Assets Pool Printer-----------------");
            //PrintAssetList("All Assets", AssetsPool.AllAssets);

            PrintAssetList("Idle Assets", AssetsPool.IdleAssets);

            PrintAssetList("Assigned Assets", AssetsPool.AssignedAssets);

            PrintAssetList("Using Assets", AssetsPool.UsingAssets);
        }

        private void PrintSchedulerList(String ListName, List<RequestedSubProgramClass> Tasks)
        {
            Debug.WriteLine(ListName);

            foreach (var subpro in Tasks)
            {
                string str = "\t\t---------------------------------\n";
                str += "\t\t" + "SP ID:" + subpro.SubProgram.SubProgramID.ToString() + "\n";
                foreach (var rec in subpro.RequestedRecipes)
                {
                    str += "\t\t\t" + "REC ID:" + rec.Recipe.RecipeID.ToString() + "\n";
                    str += "\t\t\t" + "Tester Recipe:" + rec.Recipe.TesterRecipe.Name.ToString() + "\n";
                    if (rec.Recipe.ChamberRecipe != null)
                        str += "\t\t\t" + "Chamber Recipe:" + rec.Recipe.ChamberRecipe.Name.ToString() + "\n";
                    else
                        str += "\t\t\t" + "Chamber Recipe:" + "null" + "\n";
                    foreach (var exe in rec.Executors)
                    {
                        str += "\t\t\t\t" + "Executor Status:" + exe.Status + "\n";
                    }
                }
                str += "\t\t---------------------------------";
                Debug.WriteLine(str);
            }
        }
        private void PrintScheduler(Scheduler sch)
        {
            Debug.WriteLine("-------------------Scheduler Printer-----------------");
            PrintSchedulerList("Waiting List:", sch.WaitingList);

            PrintSchedulerList("Ready List:", sch.ReadyList);

            PrintSchedulerList("Running List:", sch.RunningList);

            PrintSchedulerList("Completed List:", sch.CompletedList);
        }

        [Conditional("DEBUG")]
        private void HistoricData()
        {
            HistoricRegistration();
            HistoricOperation();
            FakeView();
        }

        private void HistoricRegistration()
        {
            InitAssets();
            InitPrograms();
            InitRequest();
        }

        private void InitAssets()
        {
            BatteryTypes = new List<BatteryTypeClass>();
            BatteryTypeClass bm = new BatteryTypeClass(1, "Oppo", "BLP663", "Li-on", 4400, 3350, 3700, 3450, 3200);
            BatteryTypes.Add(bm);

            Batteries = new List<BatteryClass>();
            BatteryClass bat = new BatteryClass(1, "pack1", BatteryTypes[0]);
            Batteries.Add(bat);
            bat = new BatteryClass(2, "pack2", BatteryTypes[0]);
            Batteries.Add(bat);
            bat = new BatteryClass(3, "pack3", BatteryTypes[0]);
            Batteries.Add(bat);
            bat = new BatteryClass(4, "pack4", BatteryTypes[0]);
            Batteries.Add(bat);
            bat = new BatteryClass(5, "pack5", BatteryTypes[0]);
            Batteries.Add(bat);

            Chambers = new List<ChamberClass>();
            ChamberClass chm = new ChamberClass(1, "Hongzhan", "PUL-80", 40, 150);
            Chambers.Add(chm);

            Testers = new List<TesterClass>();
            TesterClass Tester = new TesterClass(1, "Chroma", "17200", null);
            Tester.TesterChannels = new List<TesterChannelClass> { 
                new TesterChannelClass(1,Tester),
                new TesterChannelClass(2,Tester),
                new TesterChannelClass(3,Tester),
                new TesterChannelClass(4,Tester)};
            Testers.Add(Tester);

            IEnumerable<AssetClass> batteries = from battery in Batteries select battery;
            IEnumerable<AssetClass> chambers = from chamber in Chambers select chamber;
            IEnumerable<AssetClass> testerchannels = from testerchannel in Tester.TesterChannels select testerchannel;
            AssetsPool.AllAssets = (batteries.ToList().Concat(chambers.ToList()).Concat(testerchannels.ToList())).ToList();

            _batterytypeRepository = new BatteryTypeRepository(BatteryTypes);
            _batteryRepository = new BatteryRepository(Batteries);
            _chamberRepository = new ChamberRepository(Chambers);
            _testerRepository = new TesterRepository(Testers);

        }
        private void InitPrograms()
        {
            TesterRecipes = new List<TesterRecipeClass>();
            TesterRecipeClass tr = new TesterRecipeClass(1, Testers[0], "Charge", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(2, Testers[0], "500mA_CD", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(3, Testers[0], "1700mA_CD", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(4, Testers[0], "3000mA_CD", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(5, Testers[0], "500mA_D", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(6, Testers[0], "1700mA_D", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(7, Testers[0], "3000mA_D", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(8, Testers[0], "3500mA_D", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(9, Testers[0], "3500mA_CD", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(10, Testers[0], "D01_CD", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(11, Testers[0], "D02_CD", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(12, Testers[0], "D01_D", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(13, Testers[0], "D02_D", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(14, Testers[0], "2000mA_CD", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(15, Testers[0], "2000mA_D", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(16, Testers[0], "D03_CD", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(17, Testers[0], "D03_D", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(18, Testers[0], "1200mA_CD", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(19, Testers[0], "3450mA_CD_500", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(20, Testers[0], "500mA_CD_5", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(21, Testers[0], "2000mA_CD_5", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(22, Testers[0], "D03_CD_5", BatteryTypes[0], "1234");
            TesterRecipes.Add(tr);

            ChamberRecipes = new List<ChamberRecipeClass>();
            ChamberRecipeClass cr = new ChamberRecipeClass(3, Chambers[0], "Stable -5", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipeClass(2, Chambers[0], "Stable 35", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipeClass(3, Chambers[0], "Stable -10", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipeClass(2, Chambers[0], "Stable 0", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipeClass(2, Chambers[0], "Stable 10", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipeClass(2, Chambers[0], "Stable 20", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipeClass(2, Chambers[0], "Stable 30", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipeClass(2, Chambers[0], "Stable 40", "1234");
            ChamberRecipes.Add(cr);

            Recipes = new List<RecipeClass>();
            RecipeClass rec = new RecipeClass(TesterRecipes[0], null);   //Room Charge
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[4], ChamberRecipes[0]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[5], ChamberRecipes[0]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[6], ChamberRecipes[0]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[1], null);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[2], null);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[3], null);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[1], ChamberRecipes[1]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[2], ChamberRecipes[1]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[3], ChamberRecipes[1]);
            Recipes.Add(rec);

            rec = new RecipeClass(TesterRecipes[11], ChamberRecipes[0]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[12], ChamberRecipes[0]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[9], null);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[10], null);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[9], ChamberRecipes[1]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[10], ChamberRecipes[1]);
            Recipes.Add(rec);

            rec = new RecipeClass(TesterRecipes[7], ChamberRecipes[2]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[8], ChamberRecipes[3]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[8], ChamberRecipes[4]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[8], ChamberRecipes[5]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[8], ChamberRecipes[6]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[8], ChamberRecipes[7]);
            Recipes.Add(rec);

            rec = new RecipeClass(TesterRecipes[14], ChamberRecipes[0]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[13], null);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[13], ChamberRecipes[1]);
            Recipes.Add(rec);

            rec = new RecipeClass(TesterRecipes[16], ChamberRecipes[0]);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[15], null);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[15], ChamberRecipes[1]);
            Recipes.Add(rec);

            rec = new RecipeClass(TesterRecipes[17], null);
            Recipes.Add(rec);

            rec = new RecipeClass(TesterRecipes[18], null);
            Recipes.Add(rec);


            rec = new RecipeClass(TesterRecipes[19], null);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[20], null);
            Recipes.Add(rec);
            rec = new RecipeClass(TesterRecipes[21], null);
            Recipes.Add(rec);

            SubPrograms = new List<SubProgramClass>();
            SubProgramClass subPro = new SubProgramClass(1, new List<RecipeClass> { Recipes[0], Recipes[1] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(2, new List<RecipeClass> { Recipes[0], Recipes[2] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(3, new List<RecipeClass> { Recipes[0], Recipes[3] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(4, new List<RecipeClass> { Recipes[4] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(5, new List<RecipeClass> { Recipes[5] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(6, new List<RecipeClass> { Recipes[6] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(7, new List<RecipeClass> { Recipes[7] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(8, new List<RecipeClass> { Recipes[8] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[9] });
            SubPrograms.Add(subPro);

            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[0], Recipes[10] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[0], Recipes[11] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[12] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[13] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[14] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[15] });
            SubPrograms.Add(subPro);

            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[0], Recipes[16] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[17] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[18] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[19] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[20] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[21] });
            SubPrograms.Add(subPro);

            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[0], Recipes[22] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[23] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[24] });
            SubPrograms.Add(subPro);

            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[0], Recipes[25] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[26] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[27] });
            SubPrograms.Add(subPro);

            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[28] });
            SubPrograms.Add(subPro);

            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[29] });
            SubPrograms.Add(subPro);

            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[30] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[31] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(new List<RecipeClass> { Recipes[32] });
            SubPrograms.Add(subPro);

            Programs = new List<ProgramClass>();
            ProgramClass pro = new ProgramClass(BatteryTypes[0], "Static Test", new List<SubProgramClass> { SubPrograms[0], SubPrograms[1], SubPrograms[2], SubPrograms[3], SubPrograms[4], SubPrograms[5], SubPrograms[6], SubPrograms[7], SubPrograms[8] });
            Programs.Add(pro);
            pro = new ProgramClass(BatteryTypes[0], "Dynamic Test", new List<SubProgramClass> { SubPrograms[9], SubPrograms[10], SubPrograms[11], SubPrograms[12], SubPrograms[13], SubPrograms[14] });
            Programs.Add(pro);
            pro = new ProgramClass(BatteryTypes[0], "RC", new List<SubProgramClass> { SubPrograms[15], SubPrograms[16], SubPrograms[17], SubPrograms[18], SubPrograms[19], SubPrograms[20] });
            Programs.Add(pro);
            pro = new ProgramClass(BatteryTypes[0], "Static Test-2", new List<SubProgramClass> { SubPrograms[21], SubPrograms[22], SubPrograms[23] });
            Programs.Add(pro);
            pro = new ProgramClass(BatteryTypes[0], "Dynamic Test-2", new List<SubProgramClass> { SubPrograms[24], SubPrograms[25], SubPrograms[26] });
            Programs.Add(pro);
            pro = new ProgramClass(BatteryTypes[0], "Static Test-3", new List<SubProgramClass> { SubPrograms[27] });
            Programs.Add(pro);
            pro = new ProgramClass(BatteryTypes[0], "500 Cycle", new List<SubProgramClass> { SubPrograms[28] });
            Programs.Add(pro);
            pro = new ProgramClass(BatteryTypes[0], "5 Cycle", new List<SubProgramClass> { SubPrograms[29], SubPrograms[30], SubPrograms[31] });
            Programs.Add(pro);

            _testerrecipeRepository = new TesterRecipeRepository(TesterRecipes);
            _chamberrecipeRepository = new ChamberRecipeRepository(ChamberRecipes);
            _recipeRepository = new RecipeRepository(Recipes);
            _subprogramRepository = new SubProgramRepository(SubPrograms);
            _programRepository = new ProgramRepository(Programs);
        }
        private void InitRequest()
        {
            Requests = new List<RequestClass>();
            RequestClass Request = new RequestClass(Programs[0], "Francis", DateTime.Now, 2);
            Requests.Add(Request);
            Request = new RequestClass(Programs[1], "Francis", DateTime.Now, 2);
            Requests.Add(Request);
            Request = new RequestClass(Programs[2], "Francis", DateTime.Now, 2);
            Requests.Add(Request);
            Request = new RequestClass(Programs[3], "Francis", DateTime.Now, 1);
            Requests.Add(Request);
            Request = new RequestClass(Programs[4], "Francis", DateTime.Now, 1);
            Requests.Add(Request);
            Request = new RequestClass(Programs[5], "Francis", DateTime.Now, 1);
            Requests.Add(Request);
            Request = new RequestClass(Programs[6], "Francis", DateTime.Now, 1, Batteries[0]);
            Requests.Add(Request);
            Request = new RequestClass(Programs[7], "Francis", DateTime.Now, 1, Batteries[3]);
            Requests.Add(Request);
            _requestRepository = new RequestRepository(Requests);
        }

        private void HistoricOperation()
        {

            Scheduler sch = new Scheduler(_requestRepository);
            foreach(var r in Requests)
                _requestRepository.AddItem(r);

            sch.AssignAssets(Batteries[0], Chambers[0], Testers[0].TesterChannels[0]);
            //sch.Run();
            sch.AssignAssets(Batteries[0], Chambers[0], Testers[0].TesterChannels[0]);
            sch.Execute(DateTime.Now);
            sch.RunningList[0].RequestedRecipes[0].ValidExecutor.Commit(ExecutorStatus.Completed, DateTime.Now);
            sch.RunningList[0].RequestedRecipes[0].ValidExecutor.Commit(ExecutorStatus.Completed, DateTime.Now);
            sch.WaitingList[2].RequestedRecipes[0].ValidExecutor.Abandon();
            sch.CompletedList[0].RequestedRecipes[0].ValidExecutor.Invalidate();
            sch.OrderTasks();
            sch.AssignAssets(Batteries[0], Chambers[0], Testers[0].TesterChannels[0]);
            sch.Execute(DateTime.Now);
            //sch.FinishSubProgram(ref ExecutorClass Executor, 
            //sch.RunningRequestedSubPrograms[0].RequestedRecipes[0].Executors[0].Status = TestStatus.Completed;
            //sch.CloseRunningTask();
        }

        private void FakeView()
        {
            Debug.WriteLine("-------------------This is a fake view-----------------");
            Debug.WriteLine("Assets:");
            Debug.WriteLine("\tBatteryTypes:");
            PrintProperties(BatteryTypes);

            Debug.WriteLine("\tBatteries:");
            PrintProperties(Batteries);

            Debug.WriteLine("\tChambers:");
            PrintProperties(Chambers);

            Debug.WriteLine("\tTesters:");
            PrintProperties(Testers);


            Debug.WriteLine("Programs:");
            Debug.WriteLine("\tPrograms:");
            PrintProperties(Programs);

            Debug.WriteLine("\tSubPrograms:");
            PrintProperties(SubPrograms);

            Debug.WriteLine("\tRecipes:");
            PrintProperties(Recipes);

            Debug.WriteLine("\tChamberRecipes:");
            PrintProperties(ChamberRecipes);

            Debug.WriteLine("\tTesterRecipes:");
            PrintProperties(TesterRecipes);

            Debug.WriteLine("Requests:");
            Debug.WriteLine("\tRequests:");
            PrintProperties(Requests);
        }

        private void PrintProperties<T>(List<T> x)
        {

            foreach (var item in x)
            {
                string str = "\t\t---------------------------------\n";
                foreach (var property in item.GetType().GetProperties())
                {
                    str += "\t\t" + property.Name + ":" + property.GetValue(item, null) + "\n";
                }
                str += "\t\t---------------------------------";
                Debug.WriteLine(str);
            }
        }
        #endregion //debugger
    }
}