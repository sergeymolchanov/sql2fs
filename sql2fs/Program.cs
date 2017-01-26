using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql2fs
{
    class Program
    {
        static void Main(string[] args)
        {
            string action = (args.Length > 0 ? args[0] : @"restore");
            string rootPath = (args.Length > 1 ? args[1] : @"C:\test");
            string connStr;
            using (StreamReader sr = new StreamReader(rootPath + @"\.connectstring.txt"))
            {
                connStr = sr.ReadToEnd();
            }

            SqlConnection con = new SqlConnection(connStr);
            DBTableContentSource tcs = new DBTableContentSource(con);

            DirectoryInfo dir = new DirectoryInfo(rootPath + @"\content");

            Console.WriteLine("do {0}", action);

            foreach (FileInfo file in dir.GetFiles("*.txt"))
            {
                bool isNew = file.Length == 0;
                String[] fileFileds = null;
                if (!isNew)
                {
                    using (StreamReader sr = new StreamReader(file.FullName))
                    {
                        fileFileds = sr.ReadLine().Split(';');
                    }
                }
                
                String data;
                String tableName = file.Name.Substring(0, file.Name.Length - 4);
                if (isNew || action == "dump")
                {
                    data = tcs.Dump(tableName, fileFileds);
                    using (StreamWriter w = new StreamWriter(file.FullName))
                    {
                        w.Write(data);
                        w.Flush();
                    }
                }

                if (action == "restore")
                {
                    using (StreamReader sr = new StreamReader(file.FullName))
                    {
                        data = sr.ReadToEnd();
                    }
                    tcs.Restore(tableName, data, fileFileds);
                }
            }
            
            Console.WriteLine("exit 0");
        }
    }
}