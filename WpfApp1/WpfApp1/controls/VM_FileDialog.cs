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
            //add model's property to be observed by viewmodel
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
                //send notification if property changed
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        //properties
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
        public bool start(string xmlP, string csvP)
        {
            //if connect is successful then bind input xml and csv file 
            //and call model's start to begin simulation
            if (_model.connect("127.0.0.1", 5400) == 1)
            {
                // _model.connect("127.0.0.1", 5402);
                VM_XmlPath = xmlP;
                VM_CsvPath = csvP;
                _model.start();
                return true;
            }
            return false;
        }
        public void disconnect()
        {
            _model.disconnect();
        }
    }
}
