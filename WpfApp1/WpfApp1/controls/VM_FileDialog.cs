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
            //add property for the viewModel to observe
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
                //notify is property has changed
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        
        /*
        properties
        */
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
        // the start method, that is invoked when connect is successful
        //xml and csv paths are sent to the appropriate viewmodel
        //we then call model's start to start the simulation
        public bool start(string xmlP, string csvP)
        {
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
