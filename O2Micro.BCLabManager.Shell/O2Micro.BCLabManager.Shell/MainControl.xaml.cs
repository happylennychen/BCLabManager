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
using O2Micro.BCLabManager.Database;

namespace O2Micro.BCLabManager.Shell
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {
        private DBManager dbmanager = new DBManager();
        public List<BatteryModel> BatteryModels { get; set; }
        public List<Battery> Batteries { get; set; }
        public List<Program> Programs { get; set; }
        public List<SubProgram> SubPrograms { get; set; }
        public List<Recipe> Recipes { get; set; }
        public TestItemScheduler Scheduler { get; set; }
        public MainControl()
        {
            InitializeComponent();
            string folder = System.IO.Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments), "BCLabManager Documents\\");
            dbmanager.DBInit(folder);
            List<BatteryModel> BatteryModels = new List<BatteryModel>();
            BatteryModel bm = new BatteryModel(1, "Oppo", "BLP663", "Li-on", 4400, 3350, 3700, 3450, 3200);
            BatteryModels.Add(bm);
            bm.AddTestPlan("RC", "Francis", DateTime.Now, new List<Double> { 500, 1700, 3000 }, new List<Double> { -5, Double.NaN, 35 });
            bm.TestPlans[0].CreateTestItems();
        }
    }
}
