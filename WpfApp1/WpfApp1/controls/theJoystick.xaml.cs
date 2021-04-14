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
    /// Joystick control class is being invoked by the Data from the model.
    /// this class is the only class not communicating with the model such that the information
    /// coming from the model affects the joystick position
    /// </summary>
    public partial class theJoystick : UserControl
    {
        public theJoystick()
        {
                InitializeComponent();
        }
    }
}
