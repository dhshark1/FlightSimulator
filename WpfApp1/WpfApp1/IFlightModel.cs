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
    }
}
