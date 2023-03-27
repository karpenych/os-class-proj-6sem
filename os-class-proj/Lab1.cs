using System.Net.Sockets;
using System.Net;
using System.Text;
using System.Diagnostics;
using System.Net.Http;

namespace os_class_proj
{
    internal class Lab1
    {
        class Multicast
        {
            public void MultSend()
            {
                Console.WriteLine("Multicast Send");
                System.Net.Sockets.Socket s = new System.Net.Sockets.Socket
                    (System.Net.Sockets.AddressFamily.InterNetwork,
                        System.Net.Sockets.SocketType.Dgram,
                        System.Net.Sockets.ProtocolType.Udp);

                IPAddress ip = IPAddress.Parse("224.5.6.7");
                s.SetSocketOption(System.Net.Sockets.SocketOptionLevel.IP,
                                    System.Net.Sockets.SocketOptionName.AddMembership,
                                    new System.Net.Sockets.MulticastOption(ip));

                s.SetSocketOption(SocketOptionLevel.IP,
                      SocketOptionName.MulticastTimeToLive, 2);

                IPEndPoint ipep = new IPEndPoint(ip, 4567);
                s.Connect(ipep);
                byte[] b = new byte[10];

                for (int x = 0; x < b.Length; x++)
                    b[x] = (byte)(x + 65);

                s.Send(b, b.Length, System.Net.Sockets.SocketFlags.None);
                s.Close();
            }

            public void MultReceive()
            {
                Console.WriteLine("Multicast Receive");
                System.Net.Sockets.Socket s = new System.Net.Sockets.Socket
                             (System.Net.Sockets.AddressFamily.InterNetwork,
                                 System.Net.Sockets.SocketType.Dgram,
                                 System.Net.Sockets.ProtocolType.Udp);

                IPEndPoint ipep = new IPEndPoint(IPAddress.Any, 4567);
                s.Bind(ipep);
                IPAddress ip = IPAddress.Parse("224.5.6.7");
                s.SetSocketOption(System.Net.Sockets.SocketOptionLevel.IP,
                                    System.Net.Sockets.SocketOptionName.AddMembership,
                                    new System.Net.Sockets.MulticastOption(ip, IPAddress.Any));

                byte[] b = new byte[1024];
                int res = s.Receive(b);
                Console.WriteLine(res.ToString());
                string str = System.Text.Encoding.ASCII.GetString(b, 0, b.Length);
                Console.WriteLine(str.Trim());
            }

            public void Test()
            {
                Console.WriteLine("0 - вызов MultSend(), 1 - MultReceive() ");
                string? s = Console.ReadLine();
                if (s == "0") MultSend();
                else MultReceive();
            }
        }

        class UDP_
        {
            static Socket sock = new(AddressFamily.InterNetwork, SocketType.Dgram, ProtocolType.Udp);
            IPEndPoint remoteEndPoint = new(IPAddress.Any, 0);
            int portSrv = 4000;     //порт сервера
            IPAddress ipAddr;

            public void CommonPart(int port)      
            {
                IPHostEntry ipHost = Dns.GetHostEntry("localhost");
                ipHost = Dns.GetHostEntry(ipHost.HostName);
                int L = ipHost.AddressList.Length;                                                     
                ipAddr = ipHost.AddressList[L - 2];      
                IPEndPoint localEndPoint = new(ipAddr, port);
                sock.Bind(localEndPoint);            
                if (sock.IsBound) 
                    Console.WriteLine("Cокет UDP установлен ...");
            }

            public void runServer()                                
            {
                Console.WriteLine("UDP сервер");
                CommonPart(portSrv);                
                while (true)
                {
                    EndPoint remote = remoteEndPoint;
                    byte[] b = new byte[128];        
                    int n = sock.ReceiveFrom(b, ref remote);

                    string str = "";                
                    if (n > 0) str = Encoding.Unicode.GetString(b, 0, n);
                    Console.WriteLine(" Сервер : {0} ({1} байт), \n удаленный адрес и порт - {2}", str, n, remote.ToString());
                    byte[] b1 = Encoding.Unicode.GetBytes(str); 
                    sock.SendTo(b1, str.Length * 2, SocketFlags.None, remote);
                }
            }

            public void runClient(int num)                
            {
                Console.WriteLine("UDP клиент {0}", num);
                CommonPart(portSrv + num);
                IPAddress ipAddrSrv = ipAddr;
                IPEndPoint ipEndPointSrv = new(ipAddrSrv, portSrv);
                EndPoint remoteSrv = ipEndPointSrv;
                string str = "Запрос от клиента № " + num.ToString();
                byte[] b = Encoding.Unicode.GetBytes(str);
                sock.SendTo(b, str.Length * 2, SocketFlags.None, remoteSrv);
                byte[] b1 = new byte[128];
                int n = sock.ReceiveFrom(b1, ref remoteSrv);
                if (n > 0) str = Encoding.Unicode.GetString(b1, 0, n);
                Console.WriteLine("Клиент {0} получил: {1} ({2} байт)", num, str, n);
                sock.Shutdown(SocketShutdown.Both);                                                                                               
                sock.Close();             
                Console.ReadKey();
            }

            public void Test(string[] args)
            {
                UDP_ obj = new();
                if (args.Length == 0)
                {
                    Thread t = new(obj.runServer);
                    t.Start();
                    ProcessStartInfo[] info = new ProcessStartInfo[5];
                    for (int i = 0; i < 5; i++)
                    {
                        info[i] = new ProcessStartInfo();
                        var dir = System.IO.Directory.GetCurrentDirectory();
                        var appName = System.IO.Directory.GetFiles(dir)
                                                .Where(fileName => fileName.EndsWith(".exe"))
                                                .Select(fileName => fileName).ElementAt(0);

                        info[i].FileName = appName.ToString();
                        info[i].Arguments = " " + (1 + i).ToString() + " ";
                        info[i].UseShellExecute = true;
                        Process.Start(info[i]);                
                        Thread.Sleep(2000);
                    }
                }
                else 
                    obj.runClient(Convert.ToInt32(args[0]));
            }
        }


        public static void StartMulticast()
        {
            Multicast mc = new();
            mc.Test();
        }

        public static void StartUPD(string[] args) 
        {
            UDP_ upd = new();
            upd.Test(args);
        }
    }
}
