using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Windows.Controls;

namespace WpfApp1.controls
{
    class VM_Attributes : INotifyPropertyChanged
    {
        private IFlightModel _model;
        public VM_Attributes(IFlightModel model)
        {
            _model = model;
            _model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
        }
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        public List<string> VM_XmlNameList
        {
            get
            {
                return _model.XmlNameList;
            }
            set
            {
                _model.XmlNameList = value;
            }
        }
        public string VM_Current_attribute
        {
            get
            {
                return _model.Current_attribute;
            }
            set
            {
                _model.Current_attribute = value;
            }
        }

      /*  public List<ListBoxItem> VM_ListBoxxmlNameList
        {
            get
            {
                return _model.ListBoxxmlNameList;
            }
            set
            {
                _model.ListBoxxmlNameList = value;
            }
        }*/





    }
}
