using sql2fsbase;
using sql2fsbase.Adapters;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsmProc
{
    class Program
    {
        static void Main(string[] args)
        {
            Common.OutProc = Console.WriteLine;

            AdapterManager.SqlErrorViewInstance = new SqlErrorView();
            AdapterManager.TableRowComparerInstance = new TableRowComparer();

            if (args.Length < 2)
            {
                PrintHelp();
                return;
            }

            String projectName = args[0];
            String command = args[1];

            DirectoryInfo dir = new DirectoryInfo(Common.RootDir.FullName + "\\" + projectName);
            if (!dir.Exists)
            {
                Console.WriteLine("Project not found {0}", projectName);
                return;
            }

            ProjectDirectory projectDir = new ProjectDirectory(dir);

            if (command == "deploy")
            {
                projectDir.Restore(true);
            }
            else if (command == "pull-deploy")
            {
                projectDir.Pull(false);
                projectDir.Restore(true);
            }
            else if (command == "dump")
            {
                projectDir.Dump(true);
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Using: 'PsmProc <project> <command>'");
            Console.WriteLine("<project> = name of folder with project sources");
            Console.WriteLine("<command> = 'deploy'");
        }
    }
}
