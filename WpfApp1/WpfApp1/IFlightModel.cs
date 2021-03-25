using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

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
    }
}
