using System;
using System.Collections.Generic;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;

namespace Lab3_LibB
{
    public class WinMenIns
    {
        public void ShowWin32ProcessInfo()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Вызвана функция ShowWin32ProcessInfo из модуля Lab3_LibB.");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Информация о классе Win32_Process:");
            Console.ForegroundColor = ConsoleColor.White;

#pragma warning disable CA1416
            ManagementClass procInfo = new("Win32_Process");
            Console.WriteLine(procInfo.SystemProperties.Count.ToString());
            foreach (var p in procInfo.SystemProperties)
            {
                Console.WriteLine("{0,-15} : {1,-20} : {2}", p.Name, p.Value, p.IsArray);
            }

        }

        public void ShowDevenwDllhostDasthostInfos()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Вызвана функция ShowDevenwDllhostDasthostInfos из модуля Lab3_LibB.");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Информация о devenv.exe:");
            Console.ForegroundColor = ConsoleColor.White;

            SelectQuery processQuery = new("Win32_Process", "Name = 'devenv.exe'");      
            ManagementObjectSearcher searcher = new(processQuery);
            ManagementObjectCollection processes = searcher.Get();

            foreach (ManagementObject proc in processes)
            {
                Console.WriteLine("ID: " + proc.GetPropertyValue("ProcessId"));
                Console.WriteLine("Имя: " + proc.GetPropertyValue("Name"));
                Console.WriteLine("Cтатус: " + proc.GetPropertyValue("Status"));
                Console.WriteLine("Приоритет: " + proc.GetPropertyValue("Priority"));
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Информация о dllhost.exe:");
            Console.ForegroundColor = ConsoleColor.White;

            processQuery = new("Win32_Process", "Name = 'dllhost.exe'");
            searcher = new(processQuery);
            processes = searcher.Get();

            foreach (ManagementObject proc in processes)
            {
                Console.WriteLine("ID: " + proc.GetPropertyValue("ProcessId"));
                Console.WriteLine("Имя: " + proc.GetPropertyValue("Name"));
                Console.WriteLine("Cтатус: " + proc.GetPropertyValue("Status"));
                Console.WriteLine("Приоритет: " + proc.GetPropertyValue("Priority"));
                Console.WriteLine();
            }

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Информация о dasthost.exe:");
            Console.ForegroundColor = ConsoleColor.White;

            processQuery = new("Win32_Process", "Name = 'dasthost.exe'");
            searcher = new(processQuery);
            processes = searcher.Get();

            foreach (ManagementObject proc in processes)
            {
                Console.WriteLine("ID: " + proc.GetPropertyValue("ProcessId"));
                Console.WriteLine("Имя: " + proc.GetPropertyValue("Name"));
                Console.WriteLine("Cтатус: " + proc.GetPropertyValue("Status"));
                Console.WriteLine("Приоритет: " + proc.GetPropertyValue("Priority"));
                Console.WriteLine();
            }
            Console.WriteLine();
        }
    }
}
