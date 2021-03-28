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
    internal class ViewModel: INotifyPropertyChanged
    {
        private IFlightModel _model;        
        public ViewModel(IFlightModel model)
        {
            _model = model;
            model.PropertyChanged += delegate (Object sender, PropertyChangedEventArgs e)
            {
                NotifyPropertyChanged("VM_" + e.PropertyName);
            };
            this.MyModel = new PlotModel { Title = "Example 1" };
            this.MyModel.Series.Add(new FunctionSeries(Math.Cos, 0, 10, 0.1, "cos(x)"));
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
        /*
        public double Altmeter
        {
            get
            {
                return _model.Altmeter;
            }
        }
        public double Airspeed
        {
            get
            {
                return _model.airspeed;
            }
        }
        public double Coordinates
        {
            get
            {
                return _model.coordinates;
            }
        }
        public double RegisteredDirectionDeg
        {
            get
            {
                return _model.RegisteredDirectionDeg;
            }
        }
        public double RegisteredVerticalSpeed
        {
            get
            {
                return _model.RegisteredVerticalSpeed;
            }
        }
        public double RegisteredGroundSpeed
        {
            get
            {
                return _model.RegisteredGroundSpeed;
            }
        }
        public double RegisteredRoll
        {
            get
            {
                return _model.RegisteredRoll;
            }
        }
        public double RegisteredPitch
        {
            get
            {
                return _model.RegisteredPitch;
            }
        }
        public double RegisteredYaw
        {
            get
            {
                return _model.RegisteredYaw;
            }
        }
        public double RegisteredAltmeter
        {
            get
            {
                return _model.RegisteredAltmeter;
            }
        }
        public double Pitch
        {
            get
            {
                return _model.pitch;
            }
        }
        public double Roll
        {
            get
            {
                return _model.roll;
            }
        }
        public double Yaw
        {
            get
            {
                return _model.yaw;
            }
        }
        */

        public string VM_Time
        {
            get
            {
                return _model.Time;
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
        public Boolean VM_Play
        {
            get
            {
                return _model.Play;
            }
            set
            {
                _model.Play = value;
            }
        }

        public string VM_PlaySpeed
        {
            get
            {
                return _model.PlaySpeed;
            }
            set
            {
                _model.PlaySpeed = value;
            }

        }

        public int VM_CurrentLine
        {
            get
            {
                return _model.CurrentLine;
            }
            set
            {
                _model.CurrentLine = value;
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

        public float VM_LineRatio
        {
            get
            {
                return _model.LineRatio;
            }
            set
            {
                _model.LineRatio = value;
            }
        }

        public int VM_ProgressDirection
        {
            get
            {
                return _model.ProgressDirection;
            }
            set
            {
                _model.ProgressDirection = value;
            }
        }
        //maiky
        public ChartValues<float> VM_Atributes_atIndex
        {
            get
            {
                return _model.Atributes_atIndex;
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
        //plot
        public PlotModel MyModel { get; set; }
        //plot
    }
}
