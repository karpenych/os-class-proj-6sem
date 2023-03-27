using System.Net.Sockets;
using System.Text;


using  Socket sockCon = new(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
string hostSrv = "localhost";
int portSrv = 4000;


await sockCon.ConnectAsync(hostSrv, portSrv);
if (sockCon.Connected == true)
{
    Console.WriteLine(" Соединение с {0} установлено.", hostSrv);
}
else
{
    Console.WriteLine(" Не могу установить соединение с {0}.\n", hostSrv);
    return;
}

string msg;
int L;

while (true)
{
    Console.WriteLine(" Ввод сообщения : ");
    msg = Console.ReadLine();
    byte[] b = Encoding.Unicode.GetBytes(msg);

    await sockCon.SendAsync(b);
    if (msg.ToLower() == "end" || msg.ToLower() == "конец")
    {
        sockCon.Shutdown(SocketShutdown.Both);
        sockCon.Close();
        Console.WriteLine(" Клиент:  закрытие соединения.");
        break;
    }
    b = new byte[128];
    L = await sockCon.ReceiveAsync(b);
    msg = Encoding.Unicode.GetString(b, 0, L);
    Console.WriteLine(" Ответ сервера : {0} ({1} байт)", msg, L);
}

