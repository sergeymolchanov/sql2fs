using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sql2fsbase.Adapters.Impl;

namespace sql2fsbase.Adapters
{
    public class AdapterManager
    {
        public ProjectDirectory Project { get; private set; }
        public List<AdapterBase> Adapters { get; private set; }

        private ITools Tools { get; set; }

        public AdapterManager(ProjectDirectory project)
        {
            Project = project;
            Adapters = new List<AdapterBase>();

            if (Project.Settings.ConnectionString != null && Project.Settings.ConnectionString.Length > 5)
            {
                Adapters.Add(new TableContentAdapter(project));
                Adapters.Add(new DDLAdapter(project));
                Adapters.Add(new StoredProcAdapter(project));
            }

            if (Project.Settings.ReportServerURL != null && Project.Settings.ReportServerURL.Length > 5)
            {
                Adapters.Add(new ReportDatasourceAdapter(project));
                Adapters.Add(new ReportAdapter(project));
            }

            if (Project.Settings.VorlagenDir != null && Project.Settings.VorlagenDir.Length > 2)
            {
                Adapters.Add(new VorlagenAdapter(project));
            }

            foreach (var adapter in Adapters)
            {
                adapter.LoadFromRemote();

                foreach (var item in adapter.Items)
                {
                    item.IsExistsRemote = true;
                }

                adapter.LoadFromLocal();
            }
        }

        public void Merge()
        {
            // В первую очередь проверяем соответствие репозитария БД

            Common.MergeStyle mergeStyle;
            
            bool needLockRepo = false;

            if (Project.Settings.ConnectionString == null || Project.Settings.ConnectionString.Length < 2)
                throw new MergeException("Не настроено подключение к БД");

            SqlConnection connection = new SqlConnection(Project.Settings.ConnectionString);
            connection.Open();
            new SqlCommand("set dateformat 'mdy'", connection).ExecuteNonQuery();
            new SqlCommand("SET LANGUAGE Russian", connection).ExecuteNonQuery();

            String DBKey = (String)(new SqlCommand(SQL_GetDbKey, connection).ExecuteScalar());
            String DBInfo = (String)(new SqlCommand(SQL_GetDbInfo, connection).ExecuteScalar());
            byte[] storedData = Project.LoadFile("", "dbInfo.md5");
            byte[] dbData = Common.DefaultEncoding.GetBytes(DBKey + "/" + DBInfo);

            if (Common.ByteArrayEqual(storedData, dbData))
            {
                mergeStyle = Common.MergeStyle.Normal;
            }
            else if (DBKey.Length == 0 && storedData == null)
            {
                // Ничего не привязано, начальная заливка

                mergeStyle = Project.Tools.AskHowToMerge();

                needLockRepo = true;
            }
            else if (DBKey.Length == 0)
            {
                throw new MergeException("Репозитарий уже привязан к другой БД");
            }
            else 
            {
                throw new MergeException("Данная БД уже привязана к другому репозитарию");
            }

            // Синхронизируем

            LongOperationState.Timer2Pos = 0;
            LongOperationState.Timer2Max = Adapters.Count;

            foreach (var item in Adapters)
            {
                LongOperationState.Timer2Text = "Загрузка " + item.Prefix;
                item.Merge(mergeStyle);
                LongOperationState.Timer2Pos++;
            }

            LongOperationState.Timer1Text = "Завершено";
            LongOperationState.Timer2Text = "";
            LongOperationState.Timer1Pos = LongOperationState.Timer1Max;
            LongOperationState.Timer2Pos = LongOperationState.Timer2Max;
            
            if (needLockRepo) 
            {
                new SqlCommand(SQL_GenDbKey, connection).ExecuteNonQuery();
                DBKey = (String)(new SqlCommand(SQL_GetDbKey, connection).ExecuteScalar());
                dbData = Common.DefaultEncoding.GetBytes(DBKey + "/" + DBInfo);

                Project.StoreFile("", "dbInfo.md5", dbData);
            }
        }

        public class MergeException : Exception
        {
            public MergeException(String message) : base(message)
            {
            }
        }
        
        private readonly String SQL_CreatePsmTable = @"
if not exists (select 1 from sys.all_objects where name = 'psm_data')
	create table psm_data(ukey uniqueidentifier)";

        private readonly String SQL_GetDbInfo = @"
select name + '**' + CONVERT(varchar,create_date,113) dbinfo
  from sys.databases d
 where d.name = db_name()";

        private readonly String SQL_GetDbKey = @"select isnull(max(cast(ukey as nvarchar(100))), '') ukey from psm_data";

        private readonly String SQL_GenDbKey = @"insert into psm_data(ukey) values (NEWID())";
    }
}
