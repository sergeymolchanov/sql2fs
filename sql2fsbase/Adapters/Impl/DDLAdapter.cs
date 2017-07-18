using System;
using System.Data.SqlClient;
using System.IO;

namespace sql2fsbase.Adapters.Impl
{
    public class DDLAdapter : AdapterBaseSQL
    {
        private const String QueryString = @"select RowKey, OriginalTime, CommandText from DDL_Log where IsOriginal = 1";

        public DDLAdapter(ProjectDirectory project, TableContent.ISqlErrorView sqlErrorView)
            : base(project, sqlErrorView)
        {
            String _initSqlFile = Common.RootDir.FullName + "\\Hooks\\Database.sql";
            if (File.Exists(_initSqlFile))
            {
                String initSQL = File.ReadAllText(_initSqlFile);
                foreach(String sql in initSQL.Split(new String[] {"/*SPLIT*/"}, StringSplitOptions.RemoveEmptyEntries))
                {
                    SqlCommand cmdInit = new SqlCommand(sql, Connection);
                    cmdInit.ExecuteNonQuery();
                }
            }
        }
        
        public override void AddItem(string name)
        {
            Items.Add(new DDLItem(this, name, Project, Connection));
        }

        public override void LoadFromRemote()
        {
            SqlCommand cmd = new SqlCommand(QueryString, Connection);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    AddItem(dr.GetString(0));
                }
            }
        }

        protected override void DoSort()
        {
            Items.Sort();
        }

        public override String Prefix { get { return "DatabaseDDL"; } }
        public override String Postfix { get { return ".sql"; } }

        private const String SQL_TEMPLATE = @"
declare @proc nvarchar(max) = '{0}'
execute sp_sqlexec @proc";
    }
}