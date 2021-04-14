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
    /// a class to hold all the anomaly reports 
    /// this servs to investigate a specific anomaly.
    /// </summary>
    public partial class AnomalyReportList : UserControl
    {
        internal VM_AnomalyReport vm;
        bool first = true;
        int size = 0;
        int flag = 1;
        private string last_SelectedItem = "*";
        public AnomalyReportList()
        {
            InitializeComponent();
           
        } 
        // the function triggers the change upon clicking the relevant anomaly
        // this will notify and produce new information upon the graph control panle.
        private void AnomalyReportList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if(AnomalyReportListBox.SelectedItem != null)
            {
                string new_SelectedItem = AnomalyReportListBox.SelectedItem.ToString();               
                 if (!new_SelectedItem.Equals(last_SelectedItem)) // o(1) check inorder not to load alot of information to the screen
                {
                    vm.VM_InvestigatedAnomaly = AnomalyReportListBox.SelectedItem.ToString();
                    last_SelectedItem = new_SelectedItem;
                }
                this.AnomalyReportListBox.UnselectAll();
            }
            
        }
    }
}
