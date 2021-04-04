using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;

namespace WpfApp1.controls
{
    class VM_FileDialog : INotifyPropertyChanged
    {
        private IFlightModel _model;
        public VM_FileDialog(IFlightModel model)
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
        public string VM_CsvPath
        {
            get
            {
                return _model.CsvPath;
            }
            set
            {
                _model.CsvPath = value;
            }
        }
        public string VM_XmlPath
        {
            get
            {
                return _model.XmlPath;
            }
            set
            {
                _model.XmlPath = value;
            }
        }
        public void start()
        {
            _model.connect("127.0.0.1", 5400);
            _model.connect("127.0.0.1", 5402);
            _model.start();
        }
        public void disconnect()
        {
            _model.disconnect();
        }
    }
}
