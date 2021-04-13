using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
using LiveCharts;//chart
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using OxyPlot;
using OxyPlot.Series;
namespace WpfApp1

{
    internal class ViewModel : INotifyPropertyChanged
    {
        private IFlightModel _model;
        public ViewModel(IFlightModel model)
        {
            _model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };

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
        
        public event PropertyChangedEventHandler PropertyChanged;
        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }


       






        public int VM_NumOfLines
        {
            get
            {
                return _model.NumOfLines;
            }
            set
            {
                _model.NumOfLines = value;
            }
        }



        public short VM_Atributes_index
        {
            get
            {
                return _model.Atributes_index;
            }
            set
            {
                _model.Atributes_index = value;
            }
        }
        //
        public float VM_Aileron
        {
            get
            {
                float temp = _model.Aileron * 100;
                if (temp > 35)
                    return 35;
                if (temp < -35)
                    return -35;
                return _model.Aileron * 100;
            }
            /*set
            {
                _model.Aileron = value;
            }*/
        }
        public float VM_Throttle0
        {
            get
            {
                return _model.Throttle0;
            }
        }
        public float VM_Rudder
        {
            get
            {
                return _model.Rudder;
            }
            /*set
            {
                _model.Aileron = value;
            }*/
        }
        public float VM_Elevator
        {
            get
            {
                float temp = _model.Elevator * 100;
                if (temp > 35)
                    return 35;
                if (temp < -35)
                    return -35;
                return _model.Elevator * 100;
            }
        }
        public float VM_Altmeter
        {
            get
            {
                return _model.Altmeter;
            }
        }
        public float VM_Airspeed
        {
            get
            {
                return _model.Airspeed;
            }
        }
        public float VM_Pitch
        {
            get
            {
                return _model.Pitch;
            }
        }
        public float VM_Roll
        {
            get
            {
                return _model.Roll;
            }
        }
        public float VM_Yaw
        {
            get
            {
                return _model.Yaw;
            }
        }
        public float VM_Registered_heading_degrees
        {
            get
            {
                return _model.Registered_heading_degrees;
            }
        }
    }
}
