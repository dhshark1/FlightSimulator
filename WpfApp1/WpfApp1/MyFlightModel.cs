using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.IO;
using System.Text.RegularExpressions;
using LiveCharts;//chart
using LiveCharts.Defaults;
using LiveCharts.Wpf;
namespace WpfApp1
{
    class MyFlightModel : IFlightModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private MyTelnetClient tc;
        private MyTelnetClient tc_reader;
        private volatile Boolean stop, Play = true;
        private string csvPath;

        private string[] get_msgs = new string[6] { "get /instrumentation/altimeter/indicated-altitude-ft", "get /velocities/airspeed-kt[0]" , "get /orientation/heading-deg" , "get /orientation/roll-deg", "get /orientation/pitch-deg", "get /orientation/side-slip-deg" };

        private volatile int currentLine;
        private volatile string playSpeed="1";//delet the equal to 1!!!!!!!!!!!!!!!!!!!!

        private volatile float altmeter = 0, airspeed=0, coordinates, registeredDirectionDeg, registeredVerticalSpeed, registeredGroundSpeed, registeredRoll;
        private volatile float registeredPitch, registeredYaw, registeredAltmeter, registeredHeading_degrees=0;
        private volatile float pitch=0, roll=0, yaw=0;
        private volatile float aileron =0, throttle0=0, rudder=0, elevator=0;

        //List<float> l_aileron = new List<float>(), l_elevator = new List<float>(), l_rudder = new List<float>(), l_flaps = new List<float>(), l_slats = new List<float>(), l_speedbrake = new List<float>(), l_throttle0 = new List<float>(), l_throttle1 = new List<float>(), l_engine_pump0 = new List<float>(), l_engine_pump1 = new List<float>();
        //List<float> l_electric_pump0 = new List<float>(), l_electric_pump1 = new List<float>(), l_external_power = new List<float>(), l_APU_generator = new List<float>(), l_latitude_deg = new List<float>(), l_speedbrake = new List<float>(), l_throttle0 = new List<float>(), l_throttle1 = new List<float>(), l_engine_pump0 = new List<float>(), l_engine_pump1 = new List<float>();
        //List<float> l_aileron = new List<float>(), l_elevator = new List<float>(), l_rudder = new List<float>(), l_flaps = new List<float>(), l_slats = new List<float>(), l_speedbrake = new List<float>(), l_throttle0 = new List<float>(), l_throttle1 = new List<float>(), l_engine_pump0 = new List<float>(), l_engine_pump1 = new List<float>();
        //List<float> l_aileron = new List<float>(), l_elevator = new List<float>(), l_rudder = new List<float>(), l_flaps = new List<float>(), l_slats = new List<float>(), l_speedbrake = new List<float>(), l_throttle0 = new List<float>(), l_throttle1 = new List<float>(), l_engine_pump0 = new List<float>(), l_engine_pump1 = new List<float>();

        public volatile List<float>[] atributes = new List<float>[42];
        public volatile ChartValues<float>[] atributes2 = new ChartValues< float>[42];
        private List<float> temp = new List<float> {1,20,31,41 };

        public List<float> Atributes
        {
            get
            {
                return temp;
            }
            
        }

        public string PlaySpeed
        {
            get
            {
                return playSpeed;
            }
            set
            {
                NotifyPropertyChanged("PlaySpeed");
                playSpeed = value;
            }
        }
        public int CurrentLine
        {
            get
            {
                return currentLine;
            }
            set
            {
                currentLine = value;
                NotifyPropertyChanged("Time");
                NotifyPropertyChanged("CurrentLine");
            }
        }
        public MyFlightModel()
        {
            tc = new MyTelnetClient();
            tc_reader = new MyTelnetClient();
        }

        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
        }
        public float Aileron
        {
            get
            {
                return aileron;

            }
            set
            {
                aileron = value;
                NotifyPropertyChanged("Aileron");
            }
        }
        public float Throttle0
        {
            get
            {
                return throttle0;

            }
            set
            {
                throttle0 = value;
                NotifyPropertyChanged("Throttle0");
            }
        }
        public float Rudder
        {
            get
            {
                return rudder;

            }
            set
            {
                rudder = value;
                NotifyPropertyChanged("Rudder");
            }
        }
        public float Elevator
        {
            get
            {
                return elevator;

            }
            set
            {
                elevator = value;
                NotifyPropertyChanged("Elevator");
            }
        }
        public string CsvPath
        {
            get
            {
                return csvPath;
            } 
            set
            {
                csvPath = value;
            }
        }

        public float Altmeter { 
            get {
                return altmeter;
                
            }
            set
            {
                altmeter = value;
                NotifyPropertyChanged("Altmeter");
            }
        }
        public float Airspeed {
            get
            {
                return airspeed;

            }
            set
            {
                airspeed = value;
                NotifyPropertyChanged("Airspeed");
            }
        }
        public float Coordinates {
            get
            {
                return coordinates;

            }
            set
            {
                coordinates = value;
            }
        }
        public float RegisteredDirectionDeg {
            get
            {
                return registeredDirectionDeg;

            }
            set
            {
                registeredDirectionDeg = value;
            }
        }
        public float RegisteredVerticalSpeed {
            get
            {
                return registeredVerticalSpeed;

            }
            set
            {
                registeredVerticalSpeed = value;
            }
        }
        public float RegisteredGroundSpeed {
            get
            {
                return registeredGroundSpeed;

            }
            set
            {
                registeredGroundSpeed = value;
            }
        }
        public float RegisteredRoll {
            get
            {
                return registeredRoll;

            }
            set
            {
                registeredRoll = value;
            }
        }
        public float RegisteredPitch {
            get
            {
                return registeredPitch;

            }
            set
            {
                registeredPitch = value;
            }
        }
        public float RegisteredYaw {
            get
            {
                return registeredYaw;

            }
            set
            {
                registeredYaw = value;
            }
        }
        public float RegisteredAltmeter {
            get
            {
                return registeredAltmeter;

            }
            set
            {
                registeredAltmeter = value;
            }
        }
        public float Registered_heading_degrees
        {
            get
            {
                return registeredHeading_degrees;
            }
            set
            {
                registeredHeading_degrees = value;
                NotifyPropertyChanged("Registered_heading_degrees");
            }
        }
        public float Pitch {
            get
            {
                return pitch;

            }
            set
            {
                pitch = value;
                NotifyPropertyChanged("Pitch");
            }
        }
        public float Roll {
            get
            {
                return roll;

            }
            set
            {
                roll = value;
                NotifyPropertyChanged("Roll");
            }
        }
        public float Yaw {
            get
            {
                return yaw;

            }
            set
            {
                yaw = value;
                NotifyPropertyChanged("Yaw");
            }
        }

        public void connect(string ip, int port)
        {
            stop = false; 
            if(port == 5400)
                tc.connect(ip, port);
            if(port == 5402)
                tc_reader.connect(ip, port);
        }

        public void disconnect()
        {
            stop = true;
            tc.disconnect();
        }
        private void getAndSaveFG_attribute()
        {
            string input , input_digits_and_dot="";
            int first, second, i=0;
            float converted_input;
            while (!stop)
            {
                if (Play)
                {
                    i = 0;
                    foreach (string msg in get_msgs)
                    {
                        tc_reader.write(msg);
                        input = tc_reader.read();
                        first = input.IndexOf('\'', 0) +1;
                        second = input.IndexOf('\'', first + 1) - 1;
                        if(first != -1 && second!= -1)
                        {
                            input_digits_and_dot = input.Substring(first, second - first + 1);
                            converted_input = float.Parse(input_digits_and_dot);
                            Console.WriteLine(converted_input);
                                switch (i)
                                {
                                    case 0:
                                        Altmeter = converted_input;
                                    break;
                                    case 1:
                                        Airspeed = converted_input;
                                    break;
                                    case 2:
                                        Registered_heading_degrees = converted_input;
                                    break;
                                    case 3:
                                        Roll = converted_input;
                                    break;
                                    case 4:
                                        Pitch = converted_input;
                                    break;
                                    case 5:
                                        Yaw = converted_input;
                                    break;
                                    default:
                                    break;
                                }
                            }
                        i++;
                    }
                    //add check if currentLine
                    Aileron = atributes[0][currentLine];
                    Rudder = atributes[2][currentLine];
                    Throttle0 = atributes[6][currentLine];
                    Elevator = atributes[1][currentLine];
                    Thread.Sleep(100);
                }
            }
        }
        public void line_to_atributes_arr(String line, int index)
        {
            string input_digits_and_dot;
            int first=0, second;
            float converted_input;

            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            String[] Fields = CSVParser.Split(line);
                
            for (int i = 0; i < 42; i++)
            {
                /*first = line.IndexOf(',', first) + 1;
                second = line.IndexOf(',', first + 1) - 1;
                input_digits_and_dot = line.Substring(first, second - first + 1);
                converted_input = float.Parse(input_digits_and_dot);*/
                atributes[i].Add(float.Parse(Fields[i]));
            }
        }
        public void start()
        {
            new Thread(delegate () {

                var list = new List<string>();
                int resultSize, i = 0;
                using (var reader = new StreamReader(csvPath))
                {
                    //Reading the file into a list then reading it to an array

                    string tmpLine;
                    while ((tmpLine = reader.ReadLine()) != null)
                    {
                        list.Add(tmpLine);
                    }
                    resultSize = list.Count;
                }
                string[] result = list.ToArray();

                for (int j = 0; j < 42; j++)
                    atributes[j] = new List<float>();
                for(int k = 0; k < resultSize; k++)
                {
                    line_to_atributes_arr(result[k], k);
                }


                while (i < resultSize && !stop)
                {
                    if ((PlaySpeed != "") && Play && (float.Parse(PlaySpeed) > 0))
                    {
                        //var line = reader.ReadLine();
                        tc.write(result[i]);
                        ++i;
                        CurrentLine = i;
                        Thread.Sleep(Convert.ToInt32(100 * (1 / float.Parse(PlaySpeed))));
                    }
                }
            }).Start();
            /*new Thread(delegate() {
                
                    using (var reader = new StreamReader(csvPath))
                    {
                        while (!reader.EndOfStream && !stop)
                        {
                            if (play)
                            {
                                var line = reader.ReadLine();
                                    tc.write(line);
                            //airspeed++;
                            //Airspeed++;
                            //dani and maiky
                            *//*airspeed++;
                            tc_reader.write("get /velocities/airspeed-kt[0]");
                            String s = tc_reader.read();
                            //Double.TryParse(s, out airspeed);
                            Airspeed = airspeed;*//*
                            //dani and maiky

                            Thread.Sleep(100);
                            }
                        }
                    }
            }).Start();*/
            //maiky
            new Thread(getAndSaveFG_attribute).Start();
            //
        }
    }
}
