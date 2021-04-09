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

namespace WpfApp1.controls
{
    /// <summary>
    /// Interaction logic for AnomalyReportList.xaml
    /// </summary>
    public partial class AnomalyReportList : UserControl
    {
        internal VM_AnomalyReport vm;
        public AnomalyReportList()
        {
            InitializeComponent();
            vm = new VM_AnomalyReport(new MyFlightModel());
        }

        private void AnomalyReportList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            vm.VM_InvestigatedAnomaly = AnomalyReportListBox.SelectedItem.ToString();
        }
    }
}
