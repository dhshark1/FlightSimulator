using System;
using System.Collections.Generic;
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
using LiveCharts;//chart
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace WpfApp1.controls
{
    /// <summary>
    /// Interaction logic for chartLive.xaml
    /// </summary>
    public partial class chartLive : UserControl
    {
        /*private ViewModel vm;
        private ChartValues<float>[] V_atributes = new ChartValues<float>[42];
        internal ViewModel vm_chart
        {
            set
            {
                vm = value;
            }
        }
        public ChartValues<float>[] V_Atributes
        {
            get
            {
                return vm.VM_Atributes_atIndex;
            }
        }*/

        public chartLive()
        {
            InitializeComponent();
            
        }
    }
}
