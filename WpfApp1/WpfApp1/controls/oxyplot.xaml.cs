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
    /// Interaction logic for oxyplot.xaml
    /// </summary>
    public partial class oxyplot : UserControl
    {
        //internal VM2 vm2;
        public oxyplot()
        {
            InitializeComponent();
            //this.DataContext = vm2;
            /*Button b2 = new Button();
            b2.Content = "b2";
            b2.Name = "b332";
            b2.Click += new RoutedEventHandler(sendMe);*/
            //b2.ActualHeight = b1.ActualHeight;
            //b
            //stack.Children.Add(b2);
        }

        /*private void Button_Click(object sender, RoutedEventArgs e)
        {
            this.b1.Content = "nezer";
        }
        private void sendMe(object sender, RoutedEventArgs e)
        {
            this.b1.Content = ((Button)sender).Name;
        }*/
    }
}
