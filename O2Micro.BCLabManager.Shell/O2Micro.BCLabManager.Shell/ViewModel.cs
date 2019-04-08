using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace O2Micro.BCLabManager.Shell
{
    public class ViewModel
    {
        List<Battery> Batteries = new List<Battery>();
        List<BatteryModel> Models = new List<BatteryModel>();
        List<BatteryTestPlan> TestPlans = new List<BatteryTestPlan>();
        List<CurrRecipe> Recipes = new List<CurrRecipe>();
    }
}
