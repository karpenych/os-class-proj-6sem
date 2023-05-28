using System.IO.Pipes;
using System;
using System.Text;
using System.Security.AccessControl;
using System.Security.Principal;

namespace Lab2_NPIPE_SERVER
{
    internal class Program
    {
        static int cl_num = 0;

        static void Main(string[] args)
        {
            new Thread(ServerThread).Start();
        }

        static void ServerThread()
        {
            cl_num++;

            try
            {
                NamedPipeServerStream server = new("MyPipe", PipeDirection.InOut, 3, PipeTransmissionMode.Byte, PipeOptions.None, 4096, 4096);
                
                GetRights(server);

                Console.WriteLine("\n\n\nОжидание клиента № {0}. . .", cl_num);

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

        static public void GetRights(NamedPipeServerStream obj)
        {
            PipeSecurity sec_desc = obj.GetAccessControl();
            AuthorizationRuleCollection rules = sec_desc.GetAccessRules(true, true, typeof(SecurityIdentifier));

            foreach (AuthorizationRule rule in rules)
            {
                var pipeRule = rule as PipeAccessRule;
                Console.WriteLine("Access type: {0}\nRights: {1}\nIdentity: {2}",
                                  pipeRule.AccessControlType,
                                  pipeRule.PipeAccessRights,
                                  pipeRule.IdentityReference.Value);

                PipeAccessRights pr1 = PipeAccessRights.Write & pipeRule.PipeAccessRights,
                pr2 = PipeAccessRights.Delete & pipeRule.PipeAccessRights,
                pr3 = PipeAccessRights.CreateNewInstance & pipeRule.PipeAccessRights,
                pr4 = PipeAccessRights.Read & pipeRule.PipeAccessRights,
                pr5 = PipeAccessRights.FullControl & pipeRule.PipeAccessRights;

                Console.WriteLine("{0} | {1} | {2} | {3} | {4} \n", pr1, pr2, pr3, pr4, pr5);
            }

            Console.WriteLine("Sid Group: " + sec_desc.GetGroup(typeof(SecurityIdentifier)).Value);
        }


    }
}