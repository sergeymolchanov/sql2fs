using System;
using System.Data.SqlClient;

namespace ProjectSourceManager.Adapters.Impl
{
    public class StoredProcAdapter : AdapterBaseSQL
    {
        private const String QueryString = @"SELECT type, name, modify_date
  FROM SYS.objects 
 where type in ('FN', 'IF', 'P', 'TF', 'TR', 'V')
 order by type, name";

        public StoredProcAdapter(ProjectDirectory project) : base(project)
        {
        }
        
        public override void AddItem(string name)
        {
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
                    StoredProcItem i = new StoredProcItem(this, name, Project, Connection);
                    i.RemoteModifyDate = dr.GetDateTime(2);
                    Items.Add(i);                    
                }
            }
        }

        public override String Prefix { get { return "DatabaseProg"; } }
        public override String Postfix { get { return ".sql"; } }
    }
}