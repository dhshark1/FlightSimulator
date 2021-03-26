using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;

namespace WpfApp1
{
    class MyTelnetClient : ITelnetClient
    {
        Boolean is_connected = false;
        private TcpClient tcpClient;
        private NetworkStream netStream;
        private int buffer_size = 65536;

        private IPAddress[] IPAddresses;
        public void connect(string ip, int port)
        {
            is_connected = true;
            tcpClient = new TcpClient(AddressFamily.InterNetwork);
            IPAddresses = Dns.GetHostAddresses(ip);
       
            Console.WriteLine("Establishing connection to {0}", ip);
            tcpClient.Connect(IPAddresses, port);
            Console.WriteLine("Connection established");
            netStream = tcpClient.GetStream();
            //0.000000,0.000000,0.000000,0.000000,0.000000,0.000000,0.200000,0.000000,0,0,0,0,0,0,63.9918356506,-22.6054262395,146.217300,0.092148,5.135286,180.009918,-19.891403,2.821265,0.009063,0.000768,7.823901,45.613930,-218.024002,3.615943,9.559453,0.752271,0.013880,-219.999863,-220.000000,146.217300,0.002585,0.033478,272.124023,232.285141,-0.016821,-0.533973,-1671.869995,932.458130
            // Uses the GetStream public method to return the NetworkStream.

        }

        public void disconnect()
        {
            if (is_connected)
            {
                is_connected = false;
                tcpClient.Close();
            }
                
        }

        /* public string read()
         {
             throw new NotImplementedException();
         }*/
        public string read()
        {
            string response = null;
            if (netStream.CanWrite)
            {
                byte[] readBytes = new byte[buffer_size];
                int bytesRead = netStream.Read(readBytes, 0, buffer_size);
                response = Encoding.ASCII.GetString(readBytes, 0, bytesRead);
            }
            return response;
        }

        public void write(string command)
        {
                if (netStream.CanWrite)
                {
                    Byte[] sendBytes = Encoding.ASCII.GetBytes(command + "\r\n");
                    netStream.Write(sendBytes, 0, sendBytes.Length);
                    netStream.Flush();
                }
        }
    }
}
