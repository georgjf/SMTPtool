using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net.Sockets;
using System.Net;
using System.Threading;
using System.Diagnostics;

namespace SMTPtool
{
    class SMTPServer
    {
        List<SMTPconnection> connections = new List<SMTPconnection>();

        public void Run()
        {
            IPEndPoint endPoint = new IPEndPoint(IPAddress.Any, 25);
            TcpListener listener = new TcpListener(endPoint);
            listener.Start();

            Debug.WriteLine("SMTP Server running");
            Debug.WriteLine("Accepting connections on 127.0.0.1:25");


            while (true)
            {
                TcpClient client = listener.AcceptTcpClient();
                SMTPconnection connection = new SMTPconnection();
                connections.Add(connection);
                connection.Init(client);
                Thread thread = new System.Threading.Thread(new ThreadStart(connection.Run));
                thread.IsBackground = true;
                thread.Start();
            }
        
        }
    }
}
