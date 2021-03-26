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

        public List<float> Atributes
        {
            get;

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
        public string CsvPath
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
        public float Coordinates
        {
            set; get;
        }
        public float RegisteredDirectionDeg
        {
            set; get;
        }
        public float RegisteredVerticalSpeed
        {
            set; get;
        }
        public float RegisteredGroundSpeed
        {
            set; get;
        }
        public float RegisteredRoll
        {
            set; get;
        }
        public float RegisteredPitch
        {
            set; get;
        }
        public float RegisteredYaw
        {
            set; get;
        }
        public float Registered_heading_degrees
        {
            get;
            set;
        }
        public float RegisteredAltmeter
        {
            set; get;
        }
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
    }
}
