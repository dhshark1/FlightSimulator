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
    /// Interaction logic for VideoControl.xaml
    /// </summary>
    public partial class VideoControl : UserControl
    {
        internal VM_VideoControl vm;
        internal VM_FileDialog vm_FD;
        public VideoControl()
        {
            InitializeComponent();
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
