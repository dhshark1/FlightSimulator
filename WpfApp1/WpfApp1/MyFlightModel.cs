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
        private volatile Boolean stop;

        public void connect(string ip, int port)
        {
            stop = false;
            tc = new MyTelnetClient();
            tc.connect(ip, port);
        }

        public void disconnect()
        {
            throw new NotImplementedException();
        }

        public void start()
        {
            new Thread(delegate() {
                while (!stop)
                {
                    using (var reader = new StreamReader(@"C:\Users\maiky\Source\Repos\FlightSimulator\WpfApp1\WpfApp1\reg_flight.csv"))
                    {
                        
                        while (!reader.EndOfStream)
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
