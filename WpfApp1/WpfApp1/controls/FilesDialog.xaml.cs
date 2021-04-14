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
using Microsoft.Win32; // FileDialog 
namespace WpfApp1.controls
{
    /// <summary>
    /// FileDialog class that invokes the path for specific files
    /// PLAYBACK_SMALL and CSV
    /// </summary>
    public partial class FilesDialog : UserControl
    {
        internal VM_FileDialog vm;
        public FilesDialog()
        {
            InitializeComponent();
        }
        /*
        * This method opens the path to the CSV file using a dialog box
        */
        private void OpenFileDialog_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Choose CSV file");
            // Creating File Dialog object to interact with the files on the system
            OpenFileDialog fDia = new OpenFileDialog();
            fDia.Multiselect = false;
            // Filtering for the relevent extenstions 
            fDia.Filter = "CSV Files|*.csv| Excel Files|*.xlsx";

            Nullable<bool> fDiaOK = fDia.ShowDialog();
            if (fDiaOK == true) // File Dialog opened safely
            {
                FilePathBox.Text = fDia.FileNames[0];
                // Turn upload button visible once a path was created.
                UploadFileBox.Visibility = Visibility.Visible;
            }
            MessageBox.Show("Choose xml file");
            OpenFileDialog fDiaXml = new OpenFileDialog();
            fDia.Multiselect = false;
            // Filtering for the relevent extenstions 
            fDia.Filter = "CSV Files|*.csv| Excel Files|*.xlsx";

            Nullable<bool> fDiaXmlOK = fDiaXml.ShowDialog();
            if (fDiaOK == true) // File Dialog opened safely
            {
                FileXmlPathBox.Text = fDiaXml.FileNames[0];
                // Turn upload button visible once a path was created.
                UploadFileBox.Visibility = Visibility.Visible;
            }
        }
        // a routine to handle the case if the FG is not opened and the client
        // asks to run a file.
        private void Button_Click(object sender, RoutedEventArgs e)
        {
            System.Diagnostics.Process[] pname = System.Diagnostics.Process.GetProcessesByName("fgfs");
            if (pname.Length == 0)
            {
                MessageBox.Show("FlightGear is not up yet");
            }
            else
            {
                vm.disconnect();
                if (vm.start(FileXmlPathBox.Text, FilePathBox.Text))
                    this.UploadFileBox.Visibility = System.Windows.Visibility.Hidden;
            }

        }
    }
}
