using System;
using System.Data.SqlClient;

namespace ProjectSourceManager.Adapters.Impl
{
    public class StoredProcAdapter : AdapterBase
    {
        private const String QueryString = @"SELECT type, name 
  FROM SYS.objects 
 where type in ('FN', 'IF', 'P', 'TF', 'TR', 'V')
 order by type, name";

        private SqlConnection _connection;
        private SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(Project.Settings.ConnectionString);
                    _connection.Open();
                }
                return _connection;
            }
        }

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
                    AddItem(dr.GetString(0).Trim() + "." + dr.GetString(1));
                }
            }
        }

        public override String Prefix { get { return "DatabaseProg"; } }
        public override String Postfix { get { return ".sql"; } }
    }
}