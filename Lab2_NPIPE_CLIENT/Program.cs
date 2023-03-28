using System.IO.Pipes;
using System.Text;

namespace Lab2_NPIPE_CLIENT
{
    internal class Program
    {
        static void Main(string[] args)
        {
            NamedPipeClientStream client = new(".", "MyPipe", PipeDirection.InOut, PipeOptions.None);
            Console.WriteLine("Клиент стартовал, ожидаем подключения к серверу...");
            client.Connect();
            Console.WriteLine("Есть контакт!");
            while (true) 
            {
                Console.Write("Введите сообщение: ");
                string msg = Console.ReadLine();
                byte[] buffer = Encoding.Unicode.GetBytes(msg);
                client.Write(buffer);

                if (msg.ToLower() == "end" || msg.ToLower() == "конец")
                {
                    Console.WriteLine("Конец соединения");
                    client.Close();
                    break;
                }

                buffer = new byte[4096];
                int L = client.Read(buffer, 0, 4096);
                Console.WriteLine(Encoding.Unicode.GetString(buffer, 0, L));
            }
        }
    }
}
