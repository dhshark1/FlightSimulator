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
    /// Interaction logic for atributes.xaml
    /// </summary>
    public partial class atributes : UserControl
    {
        internal VM_Attributes vm;
        
        public atributes()
        {
            InitializeComponent();

        }
        public void addEventHendler2Attributes()
        {
                Button btn;
            foreach (string name in vm.VM_XmlNameList)
                {
                    
                    btn = new Button();
                    btn.Content = name;
                    
                    this.stack.Children.Add(btn);
                    
                btn.Click += new RoutedEventHandler(sendme);

                }
                
                
            

        }

        private void sendme(object sender, RoutedEventArgs e)
        {
            vm.VM_Current_attribute = (string)((Button)sender).Content;
        }
        /*
        private void ListBoxItem_Selected_1(object sender, RoutedEventArgs e)
        {

        }*/
    }
}
