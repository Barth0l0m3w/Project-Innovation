using System;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Collections.Generic;
using System.Linq;

namespace server;

public class TCPServer
{
    public static void Main(string[] args)
    {
        TCPServer tcpGameServer = new TCPServer();
        tcpGameServer.run();
    }

    private TCPServer()
    {
    }

    private void run()
    {
        TcpListener listener = new TcpListener(IPAddress.Any, 55558);
        listener.Start(50);

        while (true)
        {
            //check for new members	
            if (listener.Pending())
            {
            }


            Thread.Sleep(100);
        }
    }
}