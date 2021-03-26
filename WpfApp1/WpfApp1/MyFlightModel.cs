using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.IO;
using LiveCharts;//chart
using LiveCharts.Defaults;
using LiveCharts.Wpf;
using System.Text.RegularExpressions;

namespace WpfApp1
{
    class MyFlightModel : IFlightModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private MyTelnetClient tc;
        private volatile Boolean stop, play = false;
        private volatile string csvPath;
        private volatile int currentLine;
        private volatile int numOfLines;
        private volatile string playSpeed;
        private volatile int progressDirection;

        private MyTelnetClient tc_reader;
        private string[] get_msgs = new string[6] { "get /instrumentation/altimeter/indicated-altitude-ft", "get /velocities/airspeed-kt[0]", "get /orientation/heading-deg", "get /orientation/roll-deg", "get /orientation/pitch-deg", "get /orientation/side-slip-deg" };
        private volatile float altmeter = 0, airspeed = 0, registeredHeading_degrees = 0;
        private volatile float pitch = 0, roll = 0, yaw = 0;
        private volatile float aileron = 0, throttle0 = 0, rudder = 0, elevator = 0;
        private short atributes_index=0;

        //public volatile List<float>[] atributes = new List<float>[42];
        public volatile ChartValues<float>[] atributes = new ChartValues<float>[42];

        public ChartValues<float> Atributes_atIndex
        {
            get
            {
                return atributes[atributes_index];
            }

        }

        public float Altmeter
        {
            get
            {
                return altmeter;

            }
            set
            {
                altmeter = value;
                NotifyPropertyChanged("Altmeter");
            }
        }
        public float Airspeed
        {
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
        public float Pitch
        {
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
        public float Roll
        {
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
        public float Yaw
        {
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
        public short Atributes_index
        {
            get
            {
                return atributes_index;
            }
            set
            {
                atributes_index = value;
                NotifyPropertyChanged("Atributes_index");
                NotifyPropertyChanged("Atributes_atIndex");
            }
        }

        public int ProgressDirection
        {
            get
            {
                return progressDirection;
            }
            set
            {
                progressDirection = value;
                NotifyPropertyChanged("ProgressDirection");
            }
        }
        public int NumOfLines
        {
            get
            {
                return numOfLines;
            }
            set
            {
                numOfLines = value;
                NotifyPropertyChanged("NumOfLines");
            }
        }


        public void NotifyPropertyChanged(string propName)
        {
            if (this.PropertyChanged != null)
            {
                this.PropertyChanged(this, new PropertyChangedEventArgs(propName));
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
                playSpeed = value;
                NotifyPropertyChanged("PlaySpeed");
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
                NotifyPropertyChanged("LineRatio");
            }
        }
        public MyFlightModel()
        {
            tc = new MyTelnetClient();
            tc_reader = new MyTelnetClient();
            PlaySpeed = "1";
            CurrentLine = 0;
            NumOfLines = 1;
            ProgressDirection = 1;
            CsvPath = "";
            atributes[0] = new ChartValues<float>();
            atributes[0].Add(0);
        }
        public Boolean Play
        {
            get
            {
                return play;
            }
            set
            {
                play = value;
                NotifyPropertyChanged("Play");
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
                NotifyPropertyChanged("CsvPath");
            }
        }

        public void connect(string ip, int port)
        {
            stop = false;
            if (port == 5400)
                tc.connect(ip, port);
            if (port == 5402)
                tc_reader.connect(ip, port);
        }

        public void disconnect()
        {
            stop = true;
            tc.disconnect();
        }
        public string Time 
        {
            get
            {
                int x = (CurrentLine/10) % 60;
                int y = (CurrentLine/10) / 60;
                return (y.ToString() + ":" + x.ToString());
            }
            set
            {
            }
        }
        public float LineRatio
        {
            get
            {
                return ((float) CurrentLine) / NumOfLines;
            }
            set
            {
                CurrentLine = Convert.ToInt32(value * ((float) NumOfLines));
                NotifyPropertyChanged("CurrentLine");
                NotifyPropertyChanged("LineRatio");
            }
        }

        //
        private void getAndSaveFG_attribute()
        {
            string input, input_digits_and_dot = "";
            int first, second, i = 0;
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
                        first = input.IndexOf('\'', 0) + 1;
                        second = input.IndexOf('\'', first + 1) - 1;
                        if (first != -1 && second != -1)
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
        //

        public void start()
        {
            new Thread(delegate() {

                var list = new List<string>();
                using (var reader = new StreamReader(csvPath))
                {
                    //Reading the file into a list then reading it to an array
                    
                    string tmpLine;
                    while ((tmpLine = reader.ReadLine()) != null)
                    {
                        list.Add(tmpLine);
                    }
                    numOfLines = list.Count;
                }
                string[] result = list.ToArray();

                //
                for (int j = 0; j < 42; j++)
                    atributes[j] = new ChartValues<float>();
                for (int k = 0; k < numOfLines; k++)
                {
                    line_to_atributes_arr(result[k], k);
                }
                //

                while (CurrentLine<numOfLines && !stop)
                        {
                    if ((CurrentLine >= 0) && (PlaySpeed != "") && Play && (float.Parse(PlaySpeed) > 0))
                    {
                        //var line = reader.ReadLine();
                        tc.write(result[CurrentLine]);
                        if ((ProgressDirection==1) || (CurrentLine > 0))
                        {
                            CurrentLine += ProgressDirection;
                        }
                        Thread.Sleep(Convert.ToInt32(100 * (1 / float.Parse(PlaySpeed))));
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                        }
            }).Start();
            new Thread(getAndSaveFG_attribute).Start();
        }
    }
}

//TOM AND RON AND MAIKY AND DANY 26.3 18:41
