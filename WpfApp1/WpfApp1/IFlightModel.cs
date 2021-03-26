﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using LiveCharts;//chart
using LiveCharts.Defaults;
using LiveCharts.Wpf;

namespace WpfApp1
{
    internal interface IFlightModel : INotifyPropertyChanged
    {

        // connection to the robot
        void connect(string ip, int port);
        void disconnect();
        void start();
        public string CsvPath
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
        public ChartValues<float> Atributes_atIndex
        {
            get;
        }
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
    }
}
