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
    /// Class AlgorithmSelector handles with selecting the algorithm and loading the proper dll
    /// the default Algorithm loaded will be SimpleAnomaly Detector
    /// </summary>
    public partial class AlgorithmSelector : UserControl
    {
        internal VM_AnomalyReport vm;
       
        // Constructor the default Algorithm is set to simple AnomalyDetector.
        public AlgorithmSelector()
        {
            InitializeComponent();

            
            this.dllBox.Text = "Simple Anomaly Detector Selected";
        }
        // Selecting the simple anomaly detector
        private void Simple_Click(object sender, RoutedEventArgs e)
        {
            // Coloring out the other selection and marking the selected algorithm
            this.Simple.Background = Brushes.LightCoral;
            this.Hybrid.Background = Brushes.LightGray;
            this.Other.Background = Brushes.LightGray;
            this.dllBox.Text = "Simple Anomaly Detector Selected";
            var dllFile = new System.IO.FileInfo(@"plugins\SimpleAnomalyDLL.dll");
            vm.VM_DllFullPath = dllFile.FullName;

        }
        // Selecting the simple anomaly detector
        private void Hybrid_Click(object sender, RoutedEventArgs e)
        {
            // Coloring out the other selection and marking the selected algorithm
            this.Simple.Background = Brushes.LightGray;
            this.Hybrid.Background = Brushes.LightCoral;
            this.Other.Background = Brushes.LightGray; 
            this.dllBox.Text = "Hybrid Anomaly Detector Selected";
            
            var dllFile = new System.IO.FileInfo(@"plugins\HybridAnomalyDLL.dll");
            vm.VM_DllFullPath = dllFile.FullName;
        }
        // Selecting the simple anomaly detector
        private void Other_Click(object sender, RoutedEventArgs e)
        {
            // Coloring out the other selection and marking the selected algorithm
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
        // Highlight the button upon drag
        private void Simple_DragOver(object sender, DragEventArgs e)
        {
            this.Simple.Background = Brushes.Blue;
        }
        // Highlight the button upon drag
        private void Hybrid_DragOver(object sender, DragEventArgs e)
        {
            this.Hybrid.Background = Brushes.Blue;

        }
        // Highlight the button upon drag
        private void Other_DragOver(object sender, DragEventArgs e)
        {
            this.Other.Background = Brushes.Blue;
        }
    }
}

