using System;
using System.Collections.ObjectModel;
using System.IO;
using System.Management.Automation;
using System.Management.Automation.Runspaces;
using System.Text;

namespace AwesomerShell
{
    class Program
    {
        static void executeCommand(PowerShell ps, String cmd)
        {
            try
            {
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
            catch (Exception e)
            {
                Console.WriteLine("\n[!] "+e.Message);
            }
        }
        static void Main(string[] args)
        {
            if(args.Length>=1 && (args[0].Equals("-h") || args[0].Equals("--help") || args[0].Equals("/?")) )
            {
                Console.WriteLine("\nUsage: AwesomerShell.exe [base64_command]\n");
                Console.WriteLine("Options:\nbase64_command: This is not a compulsory argument. It can be used to submit Base64 encoded commands which are not too complex.\n");
                System.Environment.Exit(0);
            }
            String cmd = "";
            Runspace rs = RunspaceFactory.CreateRunspace();
            rs.Open();
            PowerShell ps = PowerShell.Create();
            ps.Runspace = rs;
            if (args.Length>=1)
            {
                Console.WriteLine("\n[*] Decoding Base64");
                byte[] data = Convert.FromBase64String(args[0]);
                cmd = Encoding.UTF8.GetString(data);
                Console.WriteLine("[*] Executing command");
                executeCommand(ps, cmd);
                Console.WriteLine("[*] Execution Complete");
                cmd = "";
            }
            while (cmd!="exit")
            {
                try
                {
                    Console.SetIn(new StreamReader(Console.OpenStandardInput(8192)));
                    Console.Write("\naps> ");
                    cmd = Console.ReadLine();
                    executeCommand(ps, cmd);
                }
                catch(Exception e)
                {
                    Console.WriteLine("\n[!] " + e.Message);
                }
            }
            rs.Close();
        }
    }
}
