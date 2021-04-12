﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using OxyPlot;
using OxyPlot.Series;
using WpfApp1.controls;
using Microsoft.Win32; // FileDialog 
using System.ComponentModel;
using System.Threading;

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        internal ViewModel vm;
        internal VM_FileDialog vm_FD;
        internal VM_Attributes vm_A;
        internal VM_Plot vm_P;
        internal MyFlightModel fm;
        internal VM_AnomalyReport vm_ANR;
       
        
        public MainWindow()
        {
            InitializeComponent();
            fm = new MyFlightModel();
            vm = new ViewModel(fm);

            this.plot.DataContext = vm;
            
            
            vm_FD = new VM_FileDialog(fm);
            this.FilesDialog.vm = vm_FD;
            this.FilesDialog.DataContext = vm_FD;

            vm_A = new VM_Attributes(fm);
            this.Attributes.vm = vm_A;
            this.Attributes.DataContext = vm_A;

            vm_ANR = new VM_AnomalyReport(fm);
            this.AnomalyReportList.vm = vm_ANR;
            this.AnomalyReportList.DataContext = vm_ANR;

            this.anomalyDLL.vm = vm_ANR;
            this.anomalyDLL.DataContext = vm_ANR;

            vm_P = new VM_Plot(fm);
            this.plot.vm = vm_P;
            this.plot.DataContext = vm_P;
            this.plot.vm.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                if (e.PropertyName.Equals("VM_Investigated_Annotation"))
                {
                    //this.Attributes.listbox.ItemsSource;
                    /*Thread t = new Thread(this.plot.listeningFunc);
                    t.SetApartmentState(ApartmentState.STA);
                    t.Start();*/
                    //((DispatcherTimer)sender).this.plot.listeningFunc();
                    this.plot.listeningFunc();
                }
            };
            

            this.DataContext = vm;

            vm_A.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                if(e.PropertyName.Equals("VM_XmlNameList"))
                {
                    //this.Attributes.listbox.ItemsSource;
                    this.Attributes.addEventHendler2Attributes();
                    
                }
            };
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (vm_FD.VM_CsvPath != "")
            {
                vm.VM_ProgressDirection = 1;
                vm.VM_Play = true;
                vm.VM_PlaySpeed = "1";
            }
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            vm.VM_Play = false;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            vm.VM_Play = false;
            vm.VM_CurrentLine = 0;
            vm.VM_PlaySpeed = "1";
        }


        private void FastForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (vm.VM_Play)
            {
                vm.VM_ProgressDirection = 1;
                vm.VM_PlaySpeed = "2";
            }
        }

        private void FastBackwardsButton_Click(object sender, RoutedEventArgs e)
        {
            if (vm.VM_Play)
            {
                vm.VM_ProgressDirection = -1;
                vm.VM_PlaySpeed = "2";
            }
        }


    }
}
