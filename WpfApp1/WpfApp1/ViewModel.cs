using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;

using LiveCharts;//chart
using LiveCharts.Defaults;
using LiveCharts.Wpf;
namespace WpfApp1
{
    internal class ViewModel: INotifyPropertyChanged
    {
        private IFlightModel _model;        
        public ViewModel(IFlightModel model)
        {
            _model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("vm_" + e.PropertyName);
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



        //chart

        //public List<float> atributes = new List<float>();
        public ChartValues<float> atributes = new ChartValues<float> { 4, 6, 5, 2, 4 };
        
        public ChartValues<float> vm_Atributes
        {
            get
            {
                atributes.Add(20);
                return atributes;
            }
            
        }
        //chart
        public float vm_Aileron
        {
            get
            {
                float temp = _model.Aileron * 100;
                if (temp > 35)
                    return 35;
                if (temp <-35)
                    return -35;
                return _model.Aileron*100;
            }
            /*set
            {
                _model.Aileron = value;
            }*/
        }
        public float vm_Throttle0
        {
            get
            {
                return _model.Throttle0;
            }
        }
        public float vm_Rudder
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
        public float vm_Elevator
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
        public float vm_Altmeter
        {
            get
            {
                return _model.Altmeter;
            }
        }
        public float vm_Airspeed
        {
            get
            {
                return _model.Airspeed;
            }
        }
        public float vm_Coordinates
        {
            get
            {
                return _model.Coordinates;
            }
        }
        public float vm_RegisteredDirectionDeg
        {
            get
            {
                return _model.RegisteredDirectionDeg;
            }
        }
        public float vm_RegisteredVerticalSpeed
        {
            get
            {
                return _model.RegisteredVerticalSpeed;
            }
        }
        public float vm_RegisteredGroundSpeed
        {
            get
            {
                return _model.RegisteredGroundSpeed;
            }
        }
        public float vm_RegisteredRoll
        {
            get
            {
                return _model.RegisteredRoll;
            }
        }
        public float vm_RegisteredPitch
        {
            get
            {
                return _model.RegisteredPitch;
            }
        }
        public float vm_RegisteredYaw
        {
            get
            {
                return _model.RegisteredYaw;
            }
        }
        public float vm_RegisteredAltmeter
        {
            get
            {
                return _model.RegisteredAltmeter;
            }
        }
        public float vm_Pitch
        {
            get
            {
                return _model.Pitch;
            }
        }
        public float vm_Roll
        {
            get
            {
                return _model.Roll;
            }
        }
        public float vm_Yaw
        {
            get
            {
                return _model.Yaw;
            }
        }
        public float vm_Registered_heading_degrees
        {
            get
            {
                return _model.Registered_heading_degrees;
            }
        }
        public string vm_CsvPath
        {
            set
            {
                _model.CsvPath = value;
            }
        }
    }
}
