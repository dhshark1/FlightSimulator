using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WpfApp1.controls
{
    class VM_AnomalyReport : INotifyPropertyChanged
    {
        private IFlightModel _model;
        
        public VM_AnomalyReport(IFlightModel model)
        {
            _model = model;
            _model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                if(e.PropertyName.Equals("AnomalyReportList"))
                    NotifyPropertyChanged("VM_" + e.PropertyName);
                else
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
        public List<string> VM_AnomalyReportList
        {
            get
            {
                return new List<string>(_model.AnomalyReportList);
            }
            set
            {
               _model.AnomalyReportList = value;
            }
        }
        public string VM_InvestigatedAnomaly
        {
            get { return _model.InvestigatedAnomaly; }
            set
            {
                _model.InvestigatedAnomaly = value;
            }
            
        }
        public string VM_DllFullPath
        {
            get { return _model.DllFullPath; }
            set
            {
                _model.DllFullPath = value;
            }
        }
    }
}
