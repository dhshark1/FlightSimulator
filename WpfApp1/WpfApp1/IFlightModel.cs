using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using LiveCharts;//chart
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using OxyPlot;
using OxyPlot.Series;
using System.Windows.Controls;
namespace WpfApp1
{
    internal interface IFlightModel : INotifyPropertyChanged
    {

        // connection to the robot
        void connect(string ip, int port);
        void disconnect();
        void start();

        //Plot
        public string PlotTitle
        {
            get; set;
        }
        public IList<DataPoint> PlotPoints
        {
            get;
        }
        public string PlotTitle_correlated
        {
            get;
            set;
        }
        public IList<DataPoint> PlotPoints_correlated
        {
            get;
        }
        //
        public string Current_attribute
        {
            get;
            set;
        }

        public string CsvPath
        {
            get;
            set;
        }
        public string XmlPath
        {
            get;
            set;
        }
        /*public List<ListBoxItem> ListBoxxmlNameList
        {
            get;
            set;
        }*/
        public List<string> XmlNameList
        {
            get;
            set;
        }
        public Boolean Play
        {
            get;
            set;
        }
        public string PlaySpeed
        {
            get;
            set;
        }
        public string Time
        {
            get;
            set;
        }
        public int CurrentLine
        {
            get;
            set;
        }
        public int NumOfLines
        {
            get;
            set;
        }
        public float LineRatio
        {
            get;
            set;
        }
        public int ProgressDirection
        {
            get;
            set;
        }
        /*public ChartValues<float> Atributes_atIndex
        {
            get;
        }*/
        public short Atributes_index
        {
            get;
            set;
        }
        public float Aileron
        {
            get;
            set;
        }
        public float Throttle0
        {
            get;
            set;
        }
        public float Rudder
        {
            get;
            set;
        }
        public float Elevator
        {
            get;
            set;
        }
        public float Altmeter
        {
            set;
            get;
        }
        public float Airspeed
        {
            set;
            get;
        }
        /*public float RegisteredAltmeter
        {
            set; get;
        }*/
        public float Pitch
        {
            set; get;
        }
        public float Roll
        {
            set; get;
        }
        public float Yaw
        {
            set; get;
        }
        public float Registered_heading_degrees
        {
            set; get;
        }
        public float SlopeLineAnnotation
        {
            get;
            set;
        }
        public float InterceptLineAnnotation
        {
            get;
            set;
        }
        public List<DataPoint> RegressionPoints
        {
            get;
            set;
        }
        public List<string> AnomalyReportList
        {
            get;
            set;
        }
        public string InvestigatedAnomaly
        {
            get;
            set;
        }
        public List<DataPoint> AnomalyReportRegressionList
        {
            get;
            set;
        }
        public List<DataPoint> RegressionPoints_last_30
        {
            get;
            set;
        }
        public Object AlgorithmSelect
        {
            get;
            set;
        }
    }
}
