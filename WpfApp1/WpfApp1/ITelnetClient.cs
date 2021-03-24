using System;
using System.Collections.Generic;
using System.Text;

namespace WpfApp1
{
    interface ITelnetClient
    {
        void connect(string ip, int port);
        void write(string command);
        string read(); // blocking call
        void disconnect();
    }
}
