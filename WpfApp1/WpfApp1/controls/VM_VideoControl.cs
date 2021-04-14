using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace WpfApp1.controls
{
    class VM_VideoControl : INotifyPropertyChanged
    {
        private IFlightModel _model;
        public VM_VideoControl(IFlightModel model)
        {
            _model = model;
            //add property for VM to observe 
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
                //notify that a property has changed
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
            }
        }
        
        /*
        properties that are relevant for the video control
        some have a getter and a setter and some have only a getter.
        */

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

        public string VM_Time
        {
            get
            {
                return _model.Time;
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


    }
}
