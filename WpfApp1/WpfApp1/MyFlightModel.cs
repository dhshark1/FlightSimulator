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
        private volatile int numOfLines;
        private volatile string playSpeed;

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
                NotifyPropertyChanged("LineRatio");
            }
        }
        public MyFlightModel()
        {
            tc = new MyTelnetClient();
            PlaySpeed = "1";
            CurrentLine = 0;
            NumOfLines = 1;
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

                while (CurrentLine<numOfLines && !stop)
                        {
                            if ((PlaySpeed!="") && Play && (float.Parse(PlaySpeed)>0))
                            {
                                //var line = reader.ReadLine();
                                tc.write(result[CurrentLine]);
                                ++CurrentLine;
                                //CurrentLine = i;
                                Thread.Sleep(Convert.ToInt32(100 * (1/float.Parse(PlaySpeed))));
                            }
                        }
            }).Start();
        }
    }
}

//TOM AND RON
