using System;
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

using Microsoft.Win32; // FileDialog 

namespace WpfApp1
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
       private ViewModel vm;
        public MainWindow()
        {
            InitializeComponent();
            
            vm = new ViewModel(new MyFlightModel());
            //this.Slider.DataContext = this;
            this.DataContext = vm;
        }
        /*
         * This method opens the path to the CSV file using a dialog box
         */
        private void OpenFileDialog_Click(object sender, RoutedEventArgs e)
        {   
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
        }

        private void Button_Click(object sender, RoutedEventArgs e)
        {
            vm.disconnect();
            vm.VM_CsvPath = FilePathBox.Text;
            vm.start();
        }

        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            vm.VM_Play = true;
        }

        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            vm.VM_Play = false;
        }

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            Console.WriteLine("Hello World");
        }

        private void BackwardsButton_Click(object sender, RoutedEventArgs e)
        {
            vm.VM_PlaySpeed = "2";
            // TBC
        }

    }
}
