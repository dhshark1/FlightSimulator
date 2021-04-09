using System;
using System.ComponentModel;
using System.Collections.Generic;
using System.Text;
using OxyPlot;
using OxyPlot.Series;

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
        public List<string> AnomalyReportList
        {
            get
            {
                return _model.AnomalyReportList;
            }
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
        public string VM_PlotTitle
        {
            get
            {
                return _model.PlotTitle;
            }
            set
            {

            }
        }

        public IList<DataPoint> VM_PlotPoints
        {
            get
            {
                return new List<DataPoint>(_model.PlotPoints);
            }

        }
        public string VM_PlotTitle_correlated
        {
            get
            {
                return _model.PlotTitle_correlated;

            }
            set
            {
                _model.PlotTitle_correlated = value;
            }
        }
        public IList<DataPoint> VM_PlotPoints_correlated
        {
            get
            {
                return new List<DataPoint>(_model.PlotPoints_correlated);

            }
        }
        public float VM_SlopeLineAnnotation
        {
            get
            {
                return _model.SlopeLineAnnotation;
            }
            set
            {
                _model.SlopeLineAnnotation = value;
            }
        }
        public float VM_InterceptLineAnnotation
        {
            get
            {
                return _model.SlopeLineAnnotation;
            }
            set
            {
                _model.SlopeLineAnnotation = value;
            }
        }

        public List<DataPoint> VM_RegressionPoints
        {
            get
            {
                return new List<DataPoint>(_model.RegressionPoints);
            }
            set
            {
                _model.RegressionPoints = value;
            }
        }
    }
}
