using System;
using System.Data.SqlClient;
using System.IO;

namespace sql2fsbase.Adapters.Impl
{
    public class StoredProcAdapter : AdapterBaseSQL
    {
        private const String QueryString = @"SELECT type, name, modify_date
  FROM SYS.objects 
 where type in ('FN', 'IF', 'P', 'TF', 'TR', 'V')
 order by type, name";

        public StoredProcAdapter(ProjectDirectory project)
            : base(project)
        {
        }
        
        public override void AddItem(string name)
        {
            if (!IsExcluded(name))
                Items.Add(new StoredProcItem(this, name, Project, Connection));
        }

        public override void LoadFromRemote()
        {
            SqlCommand cmd = new SqlCommand(QueryString, Connection);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    String name = dr.GetString(0).Trim() + "." + dr.GetString(1);
                    if (!IsExcluded(name))
                    {
                        StoredProcItem i = new StoredProcItem(this, name, Project, Connection);
                        i.RemoteModifyDate = dr.GetDateTime(2);
                        Items.Add(i);
                    }
                }
            }
        }

        public override String Prefix { get { return "DatabaseProg"; } }
        public override String Postfix { get { return ".sql"; } }

        public override void OnAfterSync()
        {
            runScripts("AfterSyncObjects");
        }

        public override void OnBeforeSync()
        {
            runScripts("BeforeSyncObjects");
        }

        private void runScripts(String type)
        {
            DirectoryInfo[] databaseRunDirs = Project.Dir.GetDirectories("DatabaseRun");

            if (databaseRunDirs.Length == 0)
                return;

            DirectoryInfo[] scriptDir = databaseRunDirs[0].GetDirectories(type);

            if (scriptDir.Length == 0)
                return;

            foreach (FileInfo fi in scriptDir[0].GetFiles("*.sql"))
            {
                using (StreamReader r = fi.OpenText())
                {
                    new SqlCommand(r.ReadToEnd(), Connection).ExecuteNonQuery();
                }
            }
        }
    }
}