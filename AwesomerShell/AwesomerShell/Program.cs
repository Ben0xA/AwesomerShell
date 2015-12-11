using System;
using System.Collections.ObjectModel;
using System.Management.Automation;
using System.Management.Automation.Runspaces;

namespace AwesomerShell
{
    class Program
    {
        static void Main(string[] args)
        {
            //Console.Write("aps> ");
            //String cmd = Console.ReadLine();

            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.Open();

            PowerShell ps = PowerShell.Create();
            ps.Runspace = rs;
            String cmd = "";
            while (cmd!="exit")
            {
                try
                {
                    Console.Write("\naps> ");
                    cmd = Console.ReadLine();
                    ps.AddScript(cmd);

                    Collection<PSObject> output = ps.Invoke();
                    if (output != null)
                    {
                        foreach (PSObject rtnItem in output)
                        {
                            Console.WriteLine(rtnItem.ToString());
                        }
                    }
                }
                catch(Exception e)
                {
                    Console.WriteLine(e.Message);
                }
            }
            rs.Close();
        }
    }
}
