using LiveCharts;//chart
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;

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
        private volatile int Num_of_Atributes = 42;

        private MyTelnetClient tc_reader;
        private string[] get_msgs = new string[6] { "get /instrumentation/altimeter/indicated-altitude-ft", "get /velocities/airspeed-kt[0]", "get /orientation/heading-deg", "get /orientation/roll-deg", "get /orientation/pitch-deg", "get /orientation/side-slip-deg" };
        private volatile float altmeter = 0, airspeed = 0, registeredHeading_degrees = 0;
        private volatile float pitch = 0, roll = 0, yaw = 0;
        private volatile float aileron = 0, throttle0 = 0, rudder = 0, elevator = 0;
        private short atributes_index = 0;
        private volatile bool atributes_are_ready = false, start_to_read = false;

        public volatile List<float>[] atributes = new List<float>[42];
        //public volatile ChartValues<float>[] atributes = new ChartValues<float>[42];
        public volatile ChartValues<float> display_atribute = new ChartValues<float>();

        public ChartValues<float> Atributes_atIndex
        {
            get
            {
                //display_atribute = atributes[atributes_index];
                return display_atribute;
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
                if (value >= currentLine)
                    currentLine = numOfLines - 1;
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
            CurrentLine = -1;
            NumOfLines = 1;
            ProgressDirection = 1;
            CsvPath = "";
            //atributes[0] = new ChartValues<float>();
            //atributes[0].Add(0);
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
                atributes_are_ready = false;
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
                int x = (CurrentLine / 10) % 60;
                int y = (CurrentLine / 10) / 60;
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
                return ((float)CurrentLine) / NumOfLines;
            }
            set
            {
                CurrentLine = Convert.ToInt32(value * ((float)NumOfLines));
                NotifyPropertyChanged("CurrentLine");
                NotifyPropertyChanged("LineRatio");
            }
        }
        /*private void notifyHelper()
        {
            NotifyPropertyChanged("Atributes_atIndex");
        }*/
        //
        private void getAndSaveFG_attribute()
        {
            string input, input_digits_and_dot = "";
            int first, second, i = 0;
            float converted_input;
            NotifyPropertyChanged("Atributes_atIndex");
            //new Thread(display_atribute_update).Start();
            while (currentLine < numOfLines && !stop)
            {
                if (Play && start_to_read)
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
                    Thread.Sleep(1000);
                }
            }
        }
        public void line_to_atributes_arr(String line, int index)
        {
            Regex CSVParser = new Regex(",(?=(?:[^\"]*\"[^\"]*\")*(?![^\"]*\"))");
            String[] Fields = CSVParser.Split(line);

            for (int i = 0; i < Num_of_Atributes; i++)
            {
                /*first = line.IndexOf(',', first) + 1;
                second = line.IndexOf(',', first + 1) - 1;
                input_digits_and_dot = line.Substring(first, second - first + 1);
                converted_input = float.Parse(input_digits_and_dot);*/
                atributes[i].Add(float.Parse(Fields[i]));
            }
            // atributes_are_ready = true;


        }
        //VERSION WITHOU IENUMERATOR
        private void display_atribute_update()
        {
            int[] display_lines = new int[Num_of_Atributes];//After creation all items of array will have default values, which is 0
            IEnumerator<float>[] atributes_IEnumerator = new IEnumerator<float>[Num_of_Atributes];
            IEnumerator<float> iEnum;

            int local_current_line, display_lines_temp, range = 0, gap = 0;
            float[] temporalCv;
            for (int j = 0; j < Num_of_Atributes; j++)
                atributes_IEnumerator[j] = atributes[j].GetEnumerator();
            while (currentLine < numOfLines && !stop)
            {
                local_current_line = currentLine;
                iEnum = atributes_IEnumerator[atributes_index];
                display_lines_temp = display_lines[atributes_index];
                //ChartValues<float> temp = atributes[atributes_index];
                if (display_lines_temp < local_current_line - 80)
                {
                    /*for (; (display_lines_temp < local_current_line) && (iEnum.MoveNext()); display_lines[atributes_index]++)
                    {
                        display_lines_temp++;
                        display_atribute.Add(iEnum.Current);
                        
                        //Thread.Sleep(50);
                    }*/
                    /* range = local_current_line - display_lines_temp;
                     temporalCv = new float[range];

                     for (var i = 0; i < range && (iEnum.MoveNext()); i++)
                     {
                         display_lines_temp++;
                         temporalCv[i] = iEnum.Current;
                     }
                     display_lines[atributes_index] = display_lines_temp;
                     display_atribute.AddRange(temporalCv);*/
                    //display_atribute.Clear();// לשקול להחזיר אתזה!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!
                    range = local_current_line - display_lines_temp;
                    temporalCv = new float[range];

                    atributes[atributes_index].CopyTo(display_lines_temp, temporalCv, 0, range);
                    display_lines_temp += range;
                    display_lines[atributes_index] = display_lines_temp;
                    display_atribute.AddRange(temporalCv);
                }

                if (display_lines[atributes_index] > local_current_line)
                {
                    atributes_IEnumerator[atributes_index].Reset();
                    display_lines[atributes_index] = 0;
                    display_atribute = new ChartValues<float>();
                    NotifyPropertyChanged("Atributes_atIndex");
                    for (; (display_lines[atributes_index] < local_current_line) && (atributes_IEnumerator[atributes_index].MoveNext()); display_lines[atributes_index]++)
                    {
                        display_atribute.Add(atributes_IEnumerator[atributes_index].Current);
                    }
                }
                Thread.Sleep(500);
            }
        }
        /*private void display_atribute_update()
         {
             int[] display_lines=new int[Num_of_Atributes];//After creation all items of array will have default values, which is 0
             IEnumerator<float>[] atributes_IEnumerator = new IEnumerator<float>[Num_of_Atributes];
             IEnumerator<float> iEnum;
             int local_current_line, display_lines_temp,range=0,gap=0;
             float[] temporalCv;
             for (int j = 0; j < Num_of_Atributes; j++)
                 atributes_IEnumerator[j] = atributes[j].GetEnumerator();
             while (currentLine < numOfLines && !stop)
             {
                 local_current_line = currentLine;
                 iEnum = atributes_IEnumerator[atributes_index];
                 display_lines_temp = display_lines[atributes_index];
                 //ChartValues<float> temp = atributes[atributes_index];
                 if (display_lines_temp < local_current_line - 80)
                 {
                     *//*for (; (display_lines_temp < local_current_line) && (iEnum.MoveNext()); display_lines[atributes_index]++)
                     {
                         display_lines_temp++;
                         display_atribute.Add(iEnum.Current);

                         //Thread.Sleep(50);
                     }*/
        /* range = local_current_line - display_lines_temp;
         temporalCv = new float[range];

         for (var i = 0; i < range && (iEnum.MoveNext()); i++)
         {
             display_lines_temp++;
             temporalCv[i] = iEnum.Current;
         }
         display_lines[atributes_index] = display_lines_temp;
         display_atribute.AddRange(temporalCv);*//*
        range = local_current_line - display_lines_temp;
        temporalCv = new float[range];
        //atributes[j].CopyTo()
        for (var i = 0; i < range && (iEnum.MoveNext()); i++)
        {
            display_lines_temp++;
            temporalCv[i] = iEnum.Current;
        }
        display_lines[atributes_index] = display_lines_temp;
        display_atribute.AddRange(temporalCv);
    }

    if (display_lines[atributes_index] > local_current_line)
    {
        atributes_IEnumerator[atributes_index].Reset();
        display_lines[atributes_index] = 0;
        display_atribute = new ChartValues<float>();
        NotifyPropertyChanged("Atributes_atIndex");
        for (; (display_lines[atributes_index] < local_current_line) && (atributes_IEnumerator[atributes_index].MoveNext()); display_lines[atributes_index]++)
        {
            display_atribute.Add(atributes_IEnumerator[atributes_index].Current);
        }
    }
    Thread.Sleep(500);
}
}*/
        public void start()
        {

            new Thread(delegate ()
            {

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
                /* for (int j = 0; j < Num_of_Atributes; j++)
                     atributes[j] = new ChartValues<float>();*/
                for (int j = 0; j < Num_of_Atributes; j++)
                    atributes[j] = new List<float>();
                for (int k = 0; k < numOfLines; k++)
                {
                    line_to_atributes_arr(result[k], k);
                }
                atributes_are_ready = true;
                //
                // new Thread(getAndSaveFG_attribute).Start();
                while (CurrentLine < numOfLines && !stop)
                {
                    if ((CurrentLine >= 0) && (PlaySpeed != "") && Play && (float.Parse(PlaySpeed) > 0))
                    {
                        //var line = reader.ReadLine();
                        tc.write(result[CurrentLine]);
                        if ((ProgressDirection == 1) || (CurrentLine > 0))
                        {
                            CurrentLine += ProgressDirection;
                        }
                        start_to_read = true;
                        Thread.Sleep(Convert.ToInt32(100 * (1 / float.Parse(PlaySpeed))));
                    }
                    else
                    {
                        Thread.Sleep(100);
                    }
                }
            }).Start();
            while (!atributes_are_ready)
            {

            }
            new Thread(getAndSaveFG_attribute).Start();
            new Thread(display_atribute_update).Start();

        }
    }
}

//TOM AND RON AND MAIKY AND DANY 26.3 18:41
