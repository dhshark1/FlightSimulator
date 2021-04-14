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
    /// a class to control the video expirince contorl.
    /// using the standrd keys to manipulate time and speed
    /// </summary>
    public partial class VideoControl : UserControl
    {
        internal VM_VideoControl vm;
        // aside fomr VideoControl view model we require a filedialog inorder to assert that the path csv is not empty.
        internal VM_FileDialog vm_FD;
        public VideoControl()
        {
            InitializeComponent();
        }
        // a play button to start the video render.
        private void PlayButton_Click(object sender, RoutedEventArgs e)
        {
            if (vm_FD.VM_CsvPath != "")
            {
                vm.VM_ProgressDirection = 1;
                vm.VM_Play = true;
                vm.VM_PlaySpeed = "1";
            }
        }
        // a pause button to stop the video at a current frame
        private void PauseButton_Click(object sender, RoutedEventArgs e)
        {
            vm.VM_Play = false;
        }
        // a stop button to reset the slider to zero and return back to square one.

        private void StopButton_Click(object sender, RoutedEventArgs e)
        {
            vm.VM_Play = false;
            vm.VM_CurrentLine = 0;
            vm.VM_PlaySpeed = "1";
        }

        // fast forward button to accelerate the speed x2.
        private void FastForwardButton_Click(object sender, RoutedEventArgs e)
        {
            if (vm.VM_Play)
            {
                vm.VM_ProgressDirection = 1;
                vm.VM_PlaySpeed = "2";
            }
        }
        // move backwards button to accelerate the speed x2 in the oposite direction.
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
