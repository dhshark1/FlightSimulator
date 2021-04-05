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
            
                //this.listbox.SelectionMode = SelectionMode.Multiple;
                Button btn;
            //this.listbox.SelectAll();
           /* //int count = this.listbox
            System.Object[] ItemObject = new System.Object[vm.VM_XmlNameList.Count];
            int i = 0;
            foreach (string name in vm.VM_XmlNameList)
            {
                this.listbox.Items.Add(name);
                //ItemObject[i++] = name;
            }*/
            
                //this.listbox.listbox.AddRange(ItemObject);
            
            foreach (string name in vm.VM_XmlNameList)
                //foreach (ListBoxItem btn in vm.VM_ListBoxxmlNameList)
                //for(;i<count;++i)
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
