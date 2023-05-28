using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Security.AccessControl;
using System.Security.Principal;
using System.Text;
using System.Threading.Tasks;

namespace os_class_proj
{
    class Lab7
    {
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
