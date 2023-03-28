using System.IO.Pipes;
using System;
using System.Text;

namespace Lab2_NPIPE_SERVER
{
    internal class Program
    {
        static int cl_num = 0;

        static void Main(string[] args)
        {
            ServerThread();
        }

        static void ServerThread()
        {
            cl_num++;

            Console.WriteLine("Ожидание клиента № {0}. . .", cl_num);
            try
            {
                NamedPipeServerStream server = new("MyPipe", PipeDirection.InOut, 3, PipeTransmissionMode.Byte, PipeOptions.None, 4096, 4096);
                server.WaitForConnection();         
                Console.WriteLine("Клиент {0} подключен", cl_num);
                while (true) 
                {
                    byte[] buffer = new byte[4096];
                    int L = server.Read(buffer, 0, 4096);
                    string msg = Encoding.Unicode.GetString(buffer, 0, L);
                    Console.WriteLine("От клиента {0}:  '{1}' ", cl_num, msg);

                    if (msg.ToLower() == "конец" || msg.ToLower() == "еnd")
                    {
                        new Thread(ServerThread).Start();
                        break;
                    }

                    msg = $"Вы отправили: {msg} ({L}) байт";
                    buffer = new byte[4096];
                    buffer = Encoding.Unicode.GetBytes(msg);
                    server.Write(buffer);
                }
                

            }                                  
            catch (SystemException e)
            {
                Console.WriteLine(e.Message);
            }
        }


    }
}