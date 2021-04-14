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
    /// <summary>
    /// The oxyplot is a class to handle all the drawing and data presentation within the graphs
    /// we are using an abstract value "Annotation" to support all types of algorithm drawing 
    /// that the library enables.
    /// </summary>
    public partial class oxyplot : UserControl
    {
        internal VM_Plot vm;
        int x = 1;
        // a line annotation for the line regression
        OxyPlot.Wpf.LineAnnotation la = new OxyPlot.Wpf.LineAnnotation();
        // an Ellipse for the Welzel algortihm.
        OxyPlot.Wpf.EllipseAnnotation pa = new OxyPlot.Wpf.EllipseAnnotation();
        // an abstract object to plot the relevant figure for the inserted algorithm.
        OxyPlot.Wpf.Annotation temp;
        public oxyplot()
        {
            InitializeComponent();
            
        }
        
        /// <summary>
        /// an implementation for the observer design pattern
        /// once the investigated anomaly is changed the graph is being notified and 
        /// plots the relevant figure according to temp
        /// </summary>
        public void listeningFunc()
        {
           
            temp = vm.VM_Investigated_Annotation;
            
            if (this.fourthGraph.Annotations.Count > 0)
            {
                this.fourthGraph.Annotations.RemoveAt(0);
                this.fourthGraph.Annotations.Add(temp);
                //this.fourthGraph.Visibility = System.Windows.Visibility.Visible;
            }
            else
            {
            // using dispatcher as the owner of the oxyplot is window.
            Dispatcher.Invoke(() =>
            {
                this.fourthGraph.Annotations.Add(temp);
            });
                
            }

               
        } 
    }
}
