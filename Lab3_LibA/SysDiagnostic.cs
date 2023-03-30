using System.Diagnostics;
using System.Diagnostics.Metrics;

namespace Lab3_LibA
{
    public class SysDiagnostic
    {
        public void ShowCurrentProccInfo()
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Вызвана функция ShowCurrentProccInfo из модуля Lab3_LibA.");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Информация о текущем процессе:");
            Console.ForegroundColor = ConsoleColor.White;

            Process currentProcess = Process.GetCurrentProcess();

            Console.WriteLine("ID: " + currentProcess.Id);
            Console.WriteLine("Имя: " + currentProcess.ProcessName);
            Console.WriteLine("Время запуска: " + currentProcess.StartTime);
            Console.WriteLine("Выделенная память: " + currentProcess.PagedMemorySize64 + " байт");
            Console.WriteLine("Выделенная виртуальная память: " + currentProcess.VirtualMemorySize64 + " байт\n\n");
        } 

        public void ShowAllModulesInfo() 
        {
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Вызвана функция ShowAllModulesInfo из модуля Lab3_LibA.");

            Console.ForegroundColor = ConsoleColor.DarkYellow;
            Console.WriteLine("Информация о всех модулях Visual Studio:");

            uint ctr = 1;

            Process currentProcess = Process.GetCurrentProcess();
            foreach (ProcessModule module in currentProcess.Modules)
            {
                Console.ForegroundColor = ConsoleColor.Blue;
                Console.WriteLine(ctr + ")");
                Console.ForegroundColor = ConsoleColor.White;
                Console.WriteLine("Название: " + module.ModuleName);
                Console.WriteLine("Полный путь: " + module.FileName);
                Console.WriteLine("Необходимая для загрузки память: " + module.ModuleMemorySize + " байт");
                Console.WriteLine("Адрес модуля в памяти: " + module.BaseAddress);
                Console.WriteLine();
                ctr++;
            }
            Console.WriteLine();
        }
    }
}
