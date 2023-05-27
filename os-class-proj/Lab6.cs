using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Principal;
using System.DirectoryServices;

namespace os_class_proj
{
    internal class Lab6
    {

        private static void UserInfo()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("Информация о текущем пользователе\n");
            Console.ForegroundColor = ConsoleColor.White;

            WindowsIdentity currentUser = WindowsIdentity.GetCurrent();
            Console.WriteLine("Имя пользователя: " + currentUser.Name);
            Console.WriteLine("SID пользователя: " + currentUser.User.Value);
            Console.WriteLine("Прошел ли пользователь проверку подлинности в Windows: " + currentUser.IsAuthenticated);
            Console.WriteLine("Определена ли в системе учетная запись пользователя как учетная запись Guest: " + currentUser.IsGuest);
            Console.WriteLine("Определена ли в системе учетная запись пользователя как учетная запись System: " + currentUser.IsSystem);
            Console.WriteLine("Определена ли в системе учетная запись пользователя как анонимная: " + currentUser.IsAnonymous);

            Console.WriteLine("\n");
        }

        private static void UsersSID()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("SIDы всех зарегистрированных пользователей\n");
            Console.ForegroundColor = ConsoleColor.White;

            DirectoryEntry localMachine = new("WinNT://" + Environment.MachineName);
            foreach (DirectoryEntry item in localMachine.Children)
            {
                if (item.SchemaClassName == "User")
                {
                    SecurityIdentifier sid = new((byte[])item.Properties["objectSid"].Value, 0);
                    Console.WriteLine("SID пользователя " + item.Name + ": " + sid.Value);
                }
            }

            Console.WriteLine("\n");
        }

        private static void GroupsSID()
        {
            Console.ForegroundColor = ConsoleColor.Blue;
            Console.WriteLine("SIDы всех зарегистрированных групп\n");
            Console.ForegroundColor = ConsoleColor.White;

            DirectoryEntry localMachine = new("WinNT://" + Environment.MachineName);
            foreach (DirectoryEntry item in localMachine.Children)
            {
                if (item.SchemaClassName == "Group")
                {
                    SecurityIdentifier sid = new((byte[])item.Properties["objectSid"].Value, 0);
                    Console.WriteLine("SID группы " + item.Name + ": " + sid.Value);
                }
            }

            Console.WriteLine();
        }

        public static void Test()
        {
            UserInfo();
            UsersSID();
            GroupsSID();
        }
    }
}
