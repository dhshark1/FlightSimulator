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
        bool first = true;
        int size = 0;
        int flag = 1;
        private string last_SelectedItem = "*";
        public AnomalyReportList()
        {
            InitializeComponent();
           
        } 

        private void AnomalyReportList_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            /*if (this.AnomalyReportListBox.SelectedItem != null && flag == 1)
            {
                size = vm.VM_AnomalyReportList.Count;
                flag = 0;
            }
            
            if (this.AnomalyReportListBox.SelectedItem != null)
            {
                if (!first)
                {
                    
                    vm.VM_InvestigatedAnomaly = AnomalyReportListBox.SelectedItem.ToString();
                }
                else
                {
                    --size;
                    if (size == 0)
                    {
                        first = false;
                    }
                }
            }*/
            /* if (!first)
             {

                 vm.VM_InvestigatedAnomaly = AnomalyReportListBox.SelectedItem.ToString();
                 //this.AnomalyReportListBox.UnselectAll();
             }
             else
             {
                 first = false;
                 //this.AnomalyReportListBox.UnselectAll();
             }*/
            if(AnomalyReportListBox.SelectedItem != null)
            {
                string new_SelectedItem = AnomalyReportListBox.SelectedItem.ToString();
                if (!new_SelectedItem.Equals(last_SelectedItem))
                {
                    vm.VM_InvestigatedAnomaly = AnomalyReportListBox.SelectedItem.ToString();
                    last_SelectedItem = new_SelectedItem;
                }
                this.AnomalyReportListBox.UnselectAll();
            }
            
        }
    }
}
