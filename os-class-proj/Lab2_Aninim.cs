using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO.Pipes;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;

namespace os_class_proj
{
    internal class Lab2_Aninim
    {
        class Server_Anonim
        {
            public static AnonymousPipeServerStream pipeServerW = new(PipeDirection.Out, HandleInheritability.Inheritable);
            public static AnonymousPipeServerStream pipeServerR = new(PipeDirection.In, HandleInheritability.Inheritable);

            static public void Test()
            {
                Console.WriteLine("Сервер: дескрипторы каналов - {0}, {1}\n", 
                    pipeServerW.GetClientHandleAsString(), pipeServerR.GetClientHandleAsString());

                Console.WriteLine("Свойства канала PipeDirection.Out:" + "\nCanRead = {0},\tCanWrite = {1},\tIsAsync = {2},\tIsConnected = {3}," +
                    "\tTransmissionMode = {4}\n", pipeServerW.CanRead, pipeServerW.CanWrite, pipeServerW.IsAsync, pipeServerW.IsConnected, pipeServerW.TransmissionMode);

                Console.WriteLine("Свойства канала PipeDirection.In:" + "\nCanRead = {0},\tCanWrite = {1},\tIsAsync = {2},\tIsConnected = {3}," +
                   "\tTransmissionMode = {4}\n", pipeServerR.CanRead, pipeServerR.CanWrite, pipeServerR.IsAsync, pipeServerR.IsConnected, pipeServerR.TransmissionMode);

                Console.WriteLine("Сервер: рабочая директория приложения - {0}\n\nИсполняемый файл - {1}\n",
                    Directory.GetCurrentDirectory(), Environment.ProcessPath);

                Process p_client = new();
                p_client.StartInfo.Arguments = pipeServerW.GetClientHandleAsString() + " " + pipeServerR.GetClientHandleAsString();
                p_client.StartInfo.FileName = Environment.ProcessPath;
                p_client.Start();

                StreamWriter sw = new(pipeServerW);
                sw.AutoFlush = true;

                StreamReader sr = new(pipeServerR);

                var pos = sr.ReadLine().Split(' ');

                var color = sr.ReadLine();

                var msg = sr.ReadLine();
                Console.Clear();
                Console.SetCursorPosition(int.Parse(pos[0]), int.Parse(pos[1]));
                Console.ForegroundColor = (ConsoleColor)Enum.Parse(typeof(ConsoleColor), color);
                Console.WriteLine("{0}", msg);
                Console.ForegroundColor = ConsoleColor.White;

                var stopMsg = sr.ReadLine();
                Console.WriteLine("\n{0}", stopMsg);
            }
        }

        class Client_Anonim 
        {
            static public void Test(string[] args)
            {
                Console.WriteLine("Запуск клиента: args = {0}, {1}\n", args[0], args[1]);

                try
                {
                    PipeStream pipeClientR = new AnonymousPipeClientStream(PipeDirection.In, args[0]);
                    StreamReader sr = new(pipeClientR);

                    PipeStream pipeClientW = new AnonymousPipeClientStream(PipeDirection.Out, args[1]);
                    StreamWriter sw = new(pipeClientW);
                    sw.AutoFlush = true;

#pragma warning disable CA1416
                    Console.Write("Клиент: Введите начальную позицию текста (x y): ");
                    sw.WriteLine(Console.ReadLine());
                    pipeClientW.WaitForPipeDrain();

                    Console.Write("Клиент: Введите цвет текста: ");
                    sw.WriteLine(Console.ReadLine());
                    pipeClientW.WaitForPipeDrain();

                    Console.Write("Клиент: Введите текстовую строку: ");
                    sw.WriteLine(Console.ReadLine());
                    pipeClientW.WaitForPipeDrain();

                    sw.WriteLine("Прекратить взаимодействие");
                    pipeClientW.WaitForPipeDrain();
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"{ex.Message}");
                }
            }
        }

       public class Anonims1 
       { 
            public static void RunServer(string[] args)
            {
                if (args.Length == 0)
                {
                    Server_Anonim.Test();
                }
                else
                {
                    Client_Anonim.Test(args);
                }
            }
       }

    }
}
