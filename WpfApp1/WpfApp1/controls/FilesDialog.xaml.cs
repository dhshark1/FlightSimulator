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
    /// Interaction logic for FilesDialog.xaml
    /// </summary>
    public partial class FilesDialog : UserControl
    {
        internal VM_FileDialog vm;
        public FilesDialog()
        {
            InitializeComponent();
           // this.DataContext = vm;
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

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.disconnect();
            vm.VM_XmlPath = FileXmlPathBox.Text;
            vm.VM_CsvPath = FilePathBox.Text;
            vm.start();
        }
    }
}
