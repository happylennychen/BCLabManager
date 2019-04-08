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
        public List<BatteryModel> BatteryModels { get; set; }
        public List<Battery> Batteries { get; set; }
        public List<Tester> Testers { get; set; }
        public List<Chamber> Chambers { get; set; }
        public List<Program> Programs { get; set; }
        public List<SubProgram> SubPrograms { get; set; }
        public List<Recipe> Recipes { get; set; }
        public List<ChamberRecipe> ChamberRecipes { get; set; }
        public List<TesterRecipe> TesterRecipes { get; set; }
        public List<Request> Requests { get; set; }
        public TestItemScheduler Scheduler { get; set; }
        public MainControl()
        {
            InitializeComponent();
            //string folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BCLabManager Documents\\");
            //dbmanager.DBInit(folder);
            FakeData();
        }

        private void FakeData()
        {
            BatteryModels = new List<BatteryModel>();
            BatteryModel bm = new BatteryModel(1, "Oppo", "BLP663", "Li-on", 4400, 3350, 3700, 3450, 3200);
            BatteryModels.Add(bm);

            Batteries = new List<Battery>();
            Battery bat = new Battery(1, "pack1", BatteryModels[0].BatteryModelID, 0);
            Batteries.Add(bat);
            bat = new Battery(2, "pack2", BatteryModels[0].BatteryModelID, 0);
            Batteries.Add(bat);
            bat = new Battery(3, "pack3", BatteryModels[0].BatteryModelID, 0);
            Batteries.Add(bat);
            bat = new Battery(4, "pack4", BatteryModels[0].BatteryModelID, 0);
            Batteries.Add(bat);

            Chambers = new List<Chamber>();
            Chamber chm = new Chamber();
            Chambers.Add(chm);

            Testers = new List<Tester>();
            Tester tst = new Tester();
            Testers.Add(tst);

            TesterRecipes = new List<TesterRecipe>();
            TesterRecipe tr = new TesterRecipe(1, Testers[0].TesterID, "500mA_CD", 1, "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipe(2, Testers[0].TesterID, "1700mA_CD", 1, "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipe(3, Testers[0].TesterID, "3000mA_CD", 1, "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipe(4, Testers[0].TesterID, "D01_CD", 1, "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipe(5, Testers[0].TesterID, "D02_CD", 1, "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipe(6, Testers[0].TesterID, "C", 1, "1234");
            TesterRecipes.Add(tr);
            tr = new TesterRecipe(7, Testers[0].TesterID, "D02_D", 1, "1234");
            TesterRecipes.Add(tr);

            ChamberRecipes = new List<ChamberRecipe>();
            ChamberRecipe cr = new ChamberRecipe(1, Chambers[0].ChamberID, "-10deg", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipe(1, Chambers[0].ChamberID, "-10 deg", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipe(2, Chambers[0].ChamberID, "0 deg", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipe(3, Chambers[0].ChamberID, "10 deg", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipe(4, Chambers[0].ChamberID, "20 deg", "1234");
            ChamberRecipes.Add(cr);
            cr = new ChamberRecipe(5, Chambers[0].ChamberID, "30 deg", "1234");
            ChamberRecipes.Add(cr);

            Recipes = new List<Recipe>();
            Recipe rec = new Recipe(1, TesterRecipes[0], ChamberRecipes[0]);
            Recipes.Add(rec);
            rec = new Recipe(2, TesterRecipes[0], ChamberRecipes[0]);
            Recipes.Add(rec);

            SubPrograms = new List<SubProgram>();
            SubProgram subPro = new SubProgram(1, new List<Recipe> { Recipes[0] });
            SubPrograms.Add(subPro);

            Programs = new List<Program>();
            Program pro = new Program(1, 1, "RC table generation", new List<SubProgram> { SubPrograms[0] });
            Programs.Add(pro);

            Requests = new List<Request>();
            Request rqst = new Request(1, Programs[0].ProgramID, "Francis", DateTime.Now);
            Requests.Add(rqst);

            Scheduler.OrderItems();

            Scheduler.AutoRun();

            Scheduler.CloseRunningTask();
        }
    }
}
