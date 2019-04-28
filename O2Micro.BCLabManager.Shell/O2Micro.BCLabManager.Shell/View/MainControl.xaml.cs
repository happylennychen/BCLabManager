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
using O2Micro.BCLabManager.Shell.ViewModel;

namespace O2Micro.BCLabManager.Shell.View
{
    /// <summary>
    /// Interaction logic for UserControl1.xaml
    /// </summary>
    public partial class MainControl : UserControl
    {

        public MainControl()
        {
            InitializeComponent();

            // Create the ViewModel to which 
            // the main window binds.
            string path = "Data/customers.xml";
            var viewModel = new MainWindowViewModel(path);


            // Allow all controls in the window to 
            // bind to the ViewModel by setting the 
            // DataContext, which propagates down 
            // the element tree.
            DataContext = viewModel;
        }
    }
}
