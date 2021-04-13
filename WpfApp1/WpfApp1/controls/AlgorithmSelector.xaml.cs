using Microsoft.Win32;
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
    /// Interaction logic for AlgorithmSelector.xaml
    /// </summary>
    public partial class AlgorithmSelector : UserControl
    {
        internal VM_AnomalyReport vm;
       
        public AlgorithmSelector()
        {
            InitializeComponent();

            /*            var dllFile = new System.IO.FileInfo(@"plugins\SimpleAnomalyDLL.dll");
                        System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.LoadFile(dllFile.FullName);
                        Object MyDLLInstance = (Object)myDllAssembly.CreateInstance("AnomalyDLL.AnomalyDetector");
                        vm.VM_AlgorithmSelect = MyDLLInstance;*/
            this.dllBox.Text = "Simple Anomaly Detector Selected";
        }

        private void Simple_Click(object sender, RoutedEventArgs e)
        {
            this.Simple.Background = Brushes.LightCoral;
            this.Hybrid.Background = Brushes.LightGray;
            this.Other.Background = Brushes.LightGray;
            this.dllBox.Text = "Simple Anomaly Detector Selected";
            var dllFile = new System.IO.FileInfo(@"plugins\SimpleAnomalyDLL.dll");
            vm.VM_DllFullPath = dllFile.FullName;

        }

        private void Hybrid_Click(object sender, RoutedEventArgs e)
        {
            this.Simple.Background = Brushes.LightGray;
            this.Hybrid.Background = Brushes.LightCoral;
            this.Other.Background = Brushes.LightGray; 
            this.dllBox.Text = "Hybrid Anomaly Detector Selected";
            /*            var dllFile = new System.IO.FileInfo(@"plugins\HybridAnomalyDLL.dll");
                        System.Reflection.Assembly myDllAssembly = System.Reflection.Assembly.LoadFile(dllFile.FullName);
                        Object MyDLLInstance = (Object)myDllAssembly.CreateInstance("AnomalyDLL.AnomalyDetector");
                        vm.VM_AlgorithmSelect = MyDLLInstance;*/
            var dllFile = new System.IO.FileInfo(@"plugins\HybridAnomalyDLL.dll");
            vm.VM_DllFullPath = dllFile.FullName;
        }

        private void Other_Click(object sender, RoutedEventArgs e)
        {
            this.Simple.Background = Brushes.LightGray;
            this.Hybrid.Background = Brushes.LightGray;
            this.Other.Background = Brushes.LightCoral;
            MessageBox.Show("Choose DLL File");
            // Creating File Dialog object to interact with the files on the system
            OpenFileDialog fDia = new OpenFileDialog();
            fDia.Multiselect = false;
            // Filtering for the relevent extenstions 
            fDia.Filter = "LibraryDLL Files|*.dll";
            Nullable<bool> fDiaOK = fDia.ShowDialog();
            if (fDiaOK == true) // File Dialog opened safely
            {
                dllBox.Text = "Other Anomaly Detector was selected";
            }
            vm.VM_DllFullPath = fDia.FileNames[0];
        }

        private void Simple_DragOver(object sender, DragEventArgs e)
        {
            this.Simple.Background = Brushes.Blue;
        }

        private void Hybrid_DragOver(object sender, DragEventArgs e)
        {
            this.Hybrid.Background = Brushes.Blue;

        }

        private void Other_DragOver(object sender, DragEventArgs e)
        {
            this.Other.Background = Brushes.Blue;
        }
    }
}

