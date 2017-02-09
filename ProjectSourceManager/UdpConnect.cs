using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Sockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ProjectSourceManager
{
    public class UdpConnect
    {
        public static void SendFile(String _file, String addr, int lag)
        {
            UdpClient server = null;
            const int dataPacketSize = 1024;

            server = new UdpClient(addr, 9040);

            using (StreamReader sr = new StreamReader(_file))
            {
                char[] chars = new char[dataPacketSize];
                byte[] bytes = new byte[dataPacketSize];

                while(true)
                {
                    int bytesCount = sr.Read(chars, 0, dataPacketSize);
                    
                    for (int i = 0; i < bytesCount; i++)
                        bytes[i] = (byte)chars[i];

                    server.Send(bytes, bytesCount);

                    if (lag > 0)
                        Thread.Sleep(lag);

                    if (bytesCount < dataPacketSize)
                        break;
                }                
            }
        }
    }
}
