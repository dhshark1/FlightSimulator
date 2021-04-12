using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
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
using OxyPlot.Axes;

namespace WpfApp1.controls
{
    public partial class oxyplot : UserControl
    {
        internal VM_Plot vm;
        int x = 1;
        OxyPlot.Wpf.LineAnnotation la = new OxyPlot.Wpf.LineAnnotation();
        OxyPlot.Wpf.Annotation temp;
        public oxyplot()
        {
            InitializeComponent();
            
        }
        // 
        public void listeningFunc()
        {
           
           
            x *= -1;
            la.Slope = 2 * x;
            la.Intercept = 2;
            la.Visibility = System.Windows.Visibility.Visible;
            
            temp =vm.VM_Investigated_Annotation;
            temp.YAxisKey = this.left_axis.Key;
            temp.XAxisKey = this.bottom_axis.Key;
            //this.fourthGraph.Annotations.Clear();
            if (this.fourthGraph.Annotations.Count > 0)
                {
                    this.fourthGraph.Annotations.RemoveAt(0);
                    this.fourthGraph.Annotations.Add(temp);
                    //this.fourthGraph.Visibility = System.Windows.Visibility.Visible;
                }
                else
                {
               
                Dispatcher.Invoke(() =>
                {
                    this.fourthGraph.Annotations.Add(temp);
                });
                
                }

               
        } 
    }
}
