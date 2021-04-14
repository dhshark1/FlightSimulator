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
    /// Attribute class holds information about the attributes in the flight such as
    /// throttle engine power etc.
    /// </summary>
    public partial class atributes : UserControl
    {
        internal VM_Attributes vm;
        
        public atributes()
        {
            InitializeComponent();

        }
        // assigning a handler for each attribute within the stack.
        public void addEventHendler2Attributes()
        {
                Button btn;
            // using an xml list (first thought to be dynamiclly loaded hence supporting a multiple XML name list
            foreach (string name in vm.VM_XmlNameList)
                {
                    
                    btn = new Button();
                    btn.Content = name;
                    
                    this.stack.Children.Add(btn);
                    
                btn.Click += new RoutedEventHandler(sendme);

                }


        }
        // the attribute selecteded to be investigated.
        private void sendme(object sender, RoutedEventArgs e)
        {
            vm.VM_Current_attribute = (string)((Button)sender).Content;
        }

    }
}
