using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace WpfApp1 
{
    class VM_Plot : INotifyPropertyChanged
    {
        private IFlightModel _model;
        public VM_Plot(IFlightModel model)
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
    }
}
