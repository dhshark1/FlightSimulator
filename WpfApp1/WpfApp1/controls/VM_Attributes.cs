using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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






    }
}
