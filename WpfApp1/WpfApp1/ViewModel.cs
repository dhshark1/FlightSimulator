using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel;
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
    }
}
