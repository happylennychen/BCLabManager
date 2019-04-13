using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
//using O2Micro.BCLabManager.Database;
using System.Diagnostics;

namespace O2Micro.BCLabManager.Shell
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        //private DBManager dbmanager = new DBManager();
        public List<BatteryModelClass> BatteryModels { get; set; }
        public List<BatteryClass> Batteries { get; set; }
        public List<TesterClass> Testers { get; set; }
        public List<ChamberClass> Chambers { get; set; }
        public List<ProgramClass> Programs { get; set; }
        public List<SubProgramClass> SubPrograms { get; set; }
        public List<RecipeClass> Recipes { get; set; }
        public List<ChamberRecipeClass> ChamberRecipes { get; set; }
        public List<TesterRecipeClass> TesterRecipes { get; set; }
        public List<RequestClass> Requests { get; set; }

        public MainControl()
        {
            InitializeComponent();
            //string folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BCLabManager Documents\\");
            //dbmanager.DBInit(folder);
            FakeProcess();
        }

        [Conditional("DEBUG")]
        private void FakeProcess()
        {
            FakeInput();
            FakeAction();
            FakeView();
        }

        private void FakeInput()
        {
            #region init assets
            BatteryModels = new List<BatteryModelClass>();
            BatteryModelClass bm = new BatteryModelClass(1, "Oppo", "BLP663", "Li-on", 4400, 3350, 3700, 3450, 3200);
            BatteryModels.Add(bm);

            Batteries = new List<BatteryClass>();
            BatteryClass bat = new BatteryClass(1, "pack1", BatteryModels[0]);
            Batteries.Add(bat);
            bat = new BatteryClass(2, "pack2", BatteryModels[0]);
            Batteries.Add(bat);
            bat = new BatteryClass(3, "pack3", BatteryModels[0]);
            Batteries.Add(bat);
            bat = new BatteryClass(4, "pack4", BatteryModels[0]);
            Batteries.Add(bat);
            bat = new BatteryClass(5, "pack5", BatteryModels[0]);
            Batteries.Add(bat);

            Chambers = new List<ChamberClass>();
            ChamberClass chm = new ChamberClass(1, "Hongzhan", "PUL-80", "40~150");
            Chambers.Add(chm);
            
            Testers = new List<TesterClass>();
            TesterClass Tester = new TesterClass(1, "Chroma", "17200",null);
            Tester.TesterChannels = new List<TesterChannelClass> { 
                new TesterChannelClass(1,Tester),
                new TesterChannelClass(2,Tester),
                new TesterChannelClass(3,Tester),
                new TesterChannelClass(4,Tester)};
            Testers.Add(Tester);
            #endregion

            #region init program
            TesterRecipes = new List<TesterRecipeClass>();
            TesterRecipeClass tr = new TesterRecipeClass(1, Testers[0], "Charge", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(2, Testers[0], "500mA_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(3, Testers[0], "1700mA_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(4, Testers[0], "3000mA_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(5, Testers[0], "500mA_D", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(6, Testers[0], "1700mA_D", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(7, Testers[0], "3000mA_D", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(8, Testers[0], "3500mA_D", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(9, Testers[0], "3500mA_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(10, Testers[0], "D01_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(11, Testers[0], "D02_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(12, Testers[0], "D01_D", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(13, Testers[0], "D02_D", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(14, Testers[0], "2000mA_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(15, Testers[0], "2000mA_D", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(16, Testers[0], "D03_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(17, Testers[0], "D03_D", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(18, Testers[0], "1200mA_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(19, Testers[0], "3450mA_CD_500", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(20, Testers[0], "500mA_CD_5", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(21, Testers[0], "2000mA_CD_5", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(22, Testers[0], "D03_CD_5", BatteryModels[0], "1234");
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
            ProgramClass pro = new ProgramClass(BatteryModels[0], "Static Test", new List<SubProgramClass> { SubPrograms[0], SubPrograms[1], SubPrograms[2], SubPrograms[3], SubPrograms[4], SubPrograms[5], SubPrograms[6], SubPrograms[7], SubPrograms[8] });
            Programs.Add(pro);
            pro = new ProgramClass(BatteryModels[0], "Dynamic Test", new List<SubProgramClass> { SubPrograms[9], SubPrograms[10], SubPrograms[11], SubPrograms[12], SubPrograms[13], SubPrograms[14] });
            Programs.Add(pro);
            pro = new ProgramClass(BatteryModels[0], "RC", new List<SubProgramClass> { SubPrograms[15], SubPrograms[16], SubPrograms[17], SubPrograms[18], SubPrograms[19], SubPrograms[20] });
            Programs.Add(pro);
            pro = new ProgramClass(BatteryModels[0], "Static Test-2", new List<SubProgramClass> { SubPrograms[21], SubPrograms[22], SubPrograms[23] });
            Programs.Add(pro);
            pro = new ProgramClass(BatteryModels[0], "Dynamic Test-2", new List<SubProgramClass> { SubPrograms[24], SubPrograms[25], SubPrograms[26] });
            Programs.Add(pro);
            pro = new ProgramClass(BatteryModels[0], "Static Test-3", new List<SubProgramClass> { SubPrograms[27] });
            Programs.Add(pro);
            pro = new ProgramClass(BatteryModels[0], "500 Cycle", new List<SubProgramClass> { SubPrograms[28] });
            Programs.Add(pro);
            pro = new ProgramClass(BatteryModels[0], "5 Cycle", new List<SubProgramClass> { SubPrograms[29], SubPrograms[30], SubPrograms[31] });
            Programs.Add(pro);
            #endregion

            #region init request
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
            #endregion
        }

        private void FakeAction()
        { 

            #region init scheduler
            Scheduler.OrderTasks();

            Scheduler.AssignAssets(Batteries[0],Chambers[0],Testers[0].TesterChannels[0]);
            //Scheduler.Run();
            Scheduler.AssignAssets(Batteries[0], Chambers[0], Testers[0].TesterChannels[0]);
            Scheduler.Execute();
            Scheduler.RunningList[0].RequestedRecipes[0].ValidExecutor.Commit(ExecutorStatus.Completed, DateTime.Now, DateTime.Now);
            Scheduler.RunningList[0].RequestedRecipes[0].ValidExecutor.Commit(ExecutorStatus.Completed, DateTime.Now, DateTime.Now);
            Scheduler.WaitingList[2].RequestedRecipes[0].ValidExecutor.Abandon();
            Scheduler.CompletedList[0].RequestedRecipes[0].ValidExecutor.Invalid();
            Scheduler.OrderTasks();
            Scheduler.AssignAssets(Batteries[0], Chambers[0], Testers[0].TesterChannels[0]);
            Scheduler.Execute();
            //Scheduler.FinishSubProgram(ref ExecutorClass Executor, 
            //Scheduler.RunningRequestedSubPrograms[0].RequestedRecipes[0].Executors[0].Status = TestStatus.Completed;
            //Scheduler.CloseRunningTask();
            #endregion
        }

        private void FakeView()
        {
            Debug.WriteLine("-------------------This is a fake view-----------------");
            Debug.WriteLine("Assets:");
            Debug.WriteLine("\tBatteryModels:");
            PrintProperties(BatteryModels);

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
    }
}
