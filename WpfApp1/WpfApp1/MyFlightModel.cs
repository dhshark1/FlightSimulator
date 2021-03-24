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
        private volatile Boolean stop, play = true;
        private string csvPath;
        public MyFlightModel()
        {
            tc = new MyTelnetClient();
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

        public void start()
        {
            new Thread(delegate() {
                
                    using (var reader = new StreamReader(csvPath))
                    {
                        while (!reader.EndOfStream && !stop)
                        {
                            if (play)
                            {
                                var line = reader.ReadLine();
                                tc.write(line);
                                Thread.Sleep(100);
                            }
                        }
                    }
            }).Start();
        }
    }
}
