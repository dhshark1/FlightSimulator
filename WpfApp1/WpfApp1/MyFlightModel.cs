using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading;
using System.IO;

namespace WpfApp1
{
    class MyFlightModel : IFlightModel
    {
        public event PropertyChangedEventHandler PropertyChanged;
        private MyTelnetClient tc;
        private volatile Boolean stop, play = false;
        private volatile string csvPath;
        private volatile int currentLine;
        private volatile string playSpeed;


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
            PlaySpeed = "1";

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

        public void connect(string ip, int port)
        {
            stop = false;
            tc.connect(ip, port);
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
        public void start()
        {
            new Thread(delegate() {

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

                while (i<resultSize && !stop)
                        {
                            if ((PlaySpeed!="") && Play && (float.Parse(PlaySpeed)>0))
                            {
                                //var line = reader.ReadLine();
                                tc.write(result[i]);
                                ++i;
                                CurrentLine = i;
                                Thread.Sleep(Convert.ToInt32(100 * (1/float.Parse(PlaySpeed))));
                            }
                        }
            }).Start();
        }
    }
}

//TOM AND RON
