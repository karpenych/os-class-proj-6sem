using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;

namespace os_class_proj
{
    internal class Lab_3
    {
        static string name = "";

        public void StartLab3()
        {
            name = "Context_LibA";
            var context = new System.Runtime.Loader.AssemblyLoadContext(name, true);
            context.Unloading += Context_Unloading;
            Assembly assembly = context.LoadFromAssemblyPath(@"C:\Users\DmitriiKarp\Desktop\MGU\6_semestr\OS(ekz)\os-class-proj\Lab3_LibA\bin\Debug\net7.0\Lab3_LibA.dll");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Загружена сборка: " + assembly.FullName);
            Console.ForegroundColor = ConsoleColor.White;

            var type = assembly.GetType("Lab3_LibA.SysDiagnostic");
            Console.WriteLine("Рабочий класс: " + type.FullName);
            var instance = Activator.CreateInstance(type);

            MethodInfo[] mm = type.GetMethods();
            foreach (var m in mm)
            {
                if(m.Name != "GetType" && m.Name != "ToString" && m.Name != "Equals" && m.Name != "GetHashCode")
                    m.Invoke(instance, null);
            }

            context.Unload();

            name = "Context_LibB";
            context = new System.Runtime.Loader.AssemblyLoadContext(name, true);
            context.Unloading += Context_Unloading;
            assembly = context.LoadFromAssemblyPath(@"C:\Users\DmitriiKarp\Desktop\MGU\6_semestr\OS(ekz)\os-class-proj\Lab3_LibB\bin\Debug\net7.0\Lab3_LibB.dll");
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Загружена сборка: " + assembly.FullName);
            Console.ForegroundColor = ConsoleColor.White;

            type = assembly.GetType("Lab3_LibB.WinMenIns");
            Console.WriteLine("Рабочий класс: " + type.FullName);
            instance = Activator.CreateInstance(type);

            mm = type.GetMethods();
            foreach (var m in mm)
            {
                if (m.Name != "GetType" && m.Name != "ToString" && m.Name != "Equals" && m.Name != "GetHashCode")
                    m.Invoke(instance, null);
            }

            context.Unload();
        }

        void Context_Unloading(System.Runtime.Loader.AssemblyLoadContext obj)
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.WriteLine("Контекст {0} выгружен со всеми сборками\n", name);
            Console.ForegroundColor = ConsoleColor.White;
        }
    }
}
