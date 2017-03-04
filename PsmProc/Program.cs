using sql2fsbase;
using sql2fsbase.Adapters;
using sql2fsbase.Adapters.Impl;
using sql2fsbase.Exceptions;
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
        static int Main(string[] args)
        {
            Common.OutProc = Console.WriteLine;

            AdapterManager.SqlErrorViewInstance = new SqlErrorView();
            AdapterManager.TableRowComparerInstance = new TableRowComparer();

            DirectoryInfo dir = null;
            if (args.Length > 1)
            {
                dir = new DirectoryInfo(args[1]);
                if (!dir.Exists)
                {
                    Console.WriteLine("Project not found: {0}", args[1]);
                    return 1;
                }
            }
            else
            {
                dir = new DirectoryInfo(Common.RootDir.FullName);
            }

            ProjectDirectory projectDir = new ProjectDirectory(dir);
            projectDir.CheckGitHooks();

            String command = args.Length > 0 ? args[0].ToLower() : "merge";

            if (command == "merge")
            {
                try
                {
                    projectDir.Merge(false);
                }
                catch (sql2fsbase.Exceptions.ObjectChangedException ex)
                {
                    Console.WriteLine("Object '{0}' is changed both side. Need manual merge.", ex.Item.Name);
                    return 1;
                }
                catch (SyncErrorsException ex)
                {
                    Console.WriteLine("Can't merge, error sql in file 'errors.sql'");

                    using (TextWriter sw = new StreamWriter("errors.sql"))
                    {
                        foreach (AdapterBaseSQL.AdapterSqlException err in ex.ErrorList)
                        {
                            sw.WriteLine(err.Sql);
                            sw.WriteLine();
                            sw.WriteLine();
                            sw.WriteLine("===========================================");
                        }
                    }
                    return 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    return 1;
                }
                return 0;
            }
            else if (command == "check")
            {
                try
                {
                    projectDir.Merge(true);
                }
                catch (sql2fsbase.Exceptions.ObjectChangedException ex)
                {
                    Console.WriteLine("Object '{0}' is changed. Need merge.", ex.Item.Name);
                    return 1;
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex.Message);
                    Console.WriteLine(ex.StackTrace);
                    return 1;
                }
                return 0;
            }
            else
            {
                PrintHelp();
                return 1;
            }
        }

        static void PrintHelp()
        {
            Console.WriteLine("Using: 'PsmProc <command> [<project path>]'");
            Console.WriteLine("<project> = name of folder with project sources");
            Console.WriteLine("<command> = 'merge'");
        }
    }
}
