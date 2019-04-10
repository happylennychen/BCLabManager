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
            FakeInput();
        }

        private void FakeInput()
        {
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

            TesterRecipes = new List<TesterRecipeClass>();
            TesterRecipeClass tr = new TesterRecipeClass(1, Testers[0], "500mA_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(2, Testers[0], "1700mA_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(3, Testers[0], "3000mA_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(4, Testers[0], "D01_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(5, Testers[0], "D02_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(6, Testers[0], "C", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(7, Testers[0], "D02_D", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(8, Testers[0], "D03_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(9, Testers[0], "D03_D", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(10, Testers[0], "2000mA_CD", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(11, Testers[0], "3450_CD_500", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipeClass(12, Testers[0], "2000mA_CD_5", BatteryModels[0], "1234");
            TesterRecipes.Add(tr);

            ChamberRecipes = new List<ChamberRecipeClass>();
            ChamberRecipeClass cr = new ChamberRecipeClass(1, Chambers[0], "-10deg", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipeClass(1, Chambers[0], "-10 deg", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipeClass(2, Chambers[0], "0 deg", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipeClass(3, Chambers[0], "10 deg", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipeClass(4, Chambers[0], "20 deg", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipeClass(5, Chambers[0], "30 deg", "1234");
            ChamberRecipes.Add(cr);

            Recipes = new List<RecipeClass>();
            RecipeClass rec = new RecipeClass(1, TesterRecipes[0], ChamberRecipes[0]);
            Recipes.Add(rec);
            rec = new RecipeClass(2, TesterRecipes[1], ChamberRecipes[0]);
            Recipes.Add(rec);
            rec = new RecipeClass(3, TesterRecipes[2], ChamberRecipes[0]);
            Recipes.Add(rec);
            rec = new RecipeClass(4, TesterRecipes[3], ChamberRecipes[0]);
            Recipes.Add(rec);
            rec = new RecipeClass(5, TesterRecipes[4], ChamberRecipes[0]);
            Recipes.Add(rec);
            rec = new RecipeClass(6, TesterRecipes[5], null);
            Recipes.Add(rec);
            rec = new RecipeClass(7, TesterRecipes[1], ChamberRecipes[1]);
            Recipes.Add(rec);
            rec = new RecipeClass(8, TesterRecipes[2], ChamberRecipes[1]);
            Recipes.Add(rec);
            rec = new RecipeClass(9, TesterRecipes[3], ChamberRecipes[1]);
            Recipes.Add(rec);
            rec = new RecipeClass(10, TesterRecipes[4], ChamberRecipes[1]);
            Recipes.Add(rec);
            rec = new RecipeClass(11, TesterRecipes[5], ChamberRecipes[1]);
            Recipes.Add(rec);
            rec = new RecipeClass(12, TesterRecipes[1], ChamberRecipes[2]);
            Recipes.Add(rec);
            rec = new RecipeClass(13, TesterRecipes[2], ChamberRecipes[2]);
            Recipes.Add(rec);
            rec = new RecipeClass(14, TesterRecipes[3], ChamberRecipes[2]);
            Recipes.Add(rec);
            rec = new RecipeClass(15, TesterRecipes[4], ChamberRecipes[2]);
            Recipes.Add(rec);
            rec = new RecipeClass(16, TesterRecipes[5], ChamberRecipes[2]);
            Recipes.Add(rec);
            rec = new RecipeClass(17, TesterRecipes[6], ChamberRecipes[3]);
            Recipes.Add(rec);
            rec = new RecipeClass(18, TesterRecipes[6], null);
            Recipes.Add(rec);

            SubPrograms = new List<SubProgramClass>();
            SubProgramClass subPro = new SubProgramClass(1, new List<RecipeClass> { Recipes[0] });
            SubPrograms.Add(subPro); 
            subPro = new SubProgramClass(2, new List<RecipeClass> { Recipes[1] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(3, new List<RecipeClass> { Recipes[2] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(4, new List<RecipeClass> { Recipes[3] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(5, new List<RecipeClass> { Recipes[4] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(6, new List<RecipeClass> { Recipes[5] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(7, new List<RecipeClass> { Recipes[6], Recipes[7] });
            SubPrograms.Add(subPro);
            subPro = new SubProgramClass(8, new List<RecipeClass> { Recipes[8] });
            SubPrograms.Add(subPro);

            Programs = new List<ProgramClass>();
            ProgramClass pro = new ProgramClass(1, BatteryModels[0], "RC table generation", new List<SubProgramClass> { SubPrograms[0], SubPrograms[1], SubPrograms[2], SubPrograms[3], SubPrograms[4] });
            Programs.Add(pro);
            pro = new ProgramClass(2, BatteryModels[0], "Static", new List<SubProgramClass> { SubPrograms[5], SubPrograms[6]});
            Programs.Add(pro);
            pro = new ProgramClass(3, BatteryModels[0], "Dynamic", new List<SubProgramClass> { SubPrograms[0], SubPrograms[7] });
            Programs.Add(pro);

            Requests = new List<RequestClass>();
            RequestClass Request = new RequestClass(1, Programs[0], "Francis", DateTime.Now, 2);
            Requests.Add(Request); 
            Request = new RequestClass(2, Programs[1], "Francis", DateTime.Now, 3);
            Requests.Add(Request);
            Request = new RequestClass(3, Programs[2], "Francis", DateTime.Now, 1, Batteries[0]);
            Requests.Add(Request);

            Scheduler.OrderTasks();

            //Scheduler.AutoRun();
            Scheduler.AssignAssets(Batteries[0],Chambers[0],Testers[0].TesterChannels[0]);
            Requests[0].RequestedProgram.RequestedSubPrograms[0].RequestedRecipes[0].Results[0].Status = TestStatus.Completed;
            //Scheduler.CloseRunningTask();
            Requests[0].RequestedProgram.RequestedSubPrograms[1].RequestedRecipes[0].Results[0].Status = TestStatus.Completed;
            Requests[0].RequestedProgram.RequestedSubPrograms[2].RequestedRecipes[0].Results[0].Status = TestStatus.Completed;
            Requests[1].RequestedProgram.RequestedSubPrograms[0].RequestedRecipes[0].Results[0].Status = TestStatus.Completed;

            Requests[0].RequestedProgram.RequestedSubPrograms[0].RequestedRecipes[0].Results[0].Status = TestStatus.Invalid;
        }
    }
}
