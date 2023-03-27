using System.Net.Sockets;
using System.Net;
using System.Text;

using Socket sockLstn = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
string hostSrv = "localhost";
int portSrv = 4000;


try
{
    IPHostEntry ipHost = Dns.GetHostEntry(hostSrv);
    IPAddress ipAddr = ipHost.AddressList[1];
    IPEndPoint localEndPoint = new(ipAddr, portSrv);
    Console.WriteLine(ipHost.HostName + "\n" + ipAddr.ToString());

    sockLstn.Bind(localEndPoint);
    sockLstn.Listen(10);
    if (sockLstn.IsBound)
        Console.WriteLine("  Сервер зпущен: ожидание клиента ...");
    else
    {
        Console.WriteLine("  Сервер: не могу выполнить Bind().");
        return;
    }

    while (true)
    {
        var sockCon = await sockLstn.AcceptAsync();
        Console.WriteLine("  Сервер: клиент {0} подключен.", sockCon.RemoteEndPoint.ToString());
        _ = Task.Run(async () => await ProcessClientAsync(sockCon));
    }
}
catch (SocketException e_)
{ 
    Console.WriteLine("SocketException: " + e_.Message); 
}

async Task ProcessClientAsync(Socket sockCon)
{
    string msg;
    byte[] b;
    int L;

    while (true)
    {
        b = new byte[128];
        L = await sockCon.ReceiveAsync(b);
        msg = Encoding.Unicode.GetString(b, 0, L);
        Console.WriteLine("  Сервер: получено: {0} ({1} байт) от {2}", msg, L, sockCon.RemoteEndPoint.ToString());
        if (msg.ToLower() == "end" || msg.ToLower() == "конец")
        {
            sockCon.Shutdown(SocketShutdown.Both);
            sockCon.Close();
            Console.WriteLine(" Сервер: закрытие соединения c {0}.", sockCon.RemoteEndPoint.ToString());
            break;
        }
        msg = " Получено от клиента " + L.ToString() + " байт";
        b = Encoding.Unicode.GetBytes(msg);
        _ = await sockCon.SendAsync(b);
    }
}


