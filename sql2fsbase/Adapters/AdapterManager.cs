using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sql2fsbase.Adapters.Impl;
using sql2fsbase.Adapters.Impl.DBContent;

namespace sql2fsbase.Adapters
{
    public class AdapterManager
    {
        public ProjectDirectory Project { get; private set; }
        public List<AdapterBase> Adapters { get; private set; }

        public static ITableRowComparer TableRowComparerInstance { get; set; }
        public static ISqlErrorView SqlErrorViewInstance { get; set; }

        public AdapterManager(ProjectDirectory project)
        {
            if (TableRowComparerInstance == null)
                throw new NullReferenceException();
            if (SqlErrorViewInstance == null)
                throw new NullReferenceException();

            Project = project;
            Adapters = new List<AdapterBase>();

            if (Project.Settings.ReportServerURL.Length > 5)
                Adapters.Add(new ReportAdapter(project));

            if (Project.Settings.ConnectionString.Length > 5)
            {
                Adapters.Add(new DDLAdapter(project, SqlErrorViewInstance));
                Adapters.Add(new TableContentAdapter(project, SqlErrorViewInstance, TableRowComparerInstance));
                Adapters.Add(new StoredProcAdapter(project, SqlErrorViewInstance));
            }

            if (Project.Settings.VorlagenDir.Length > 2)
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

        public void Dump(bool force)
        {
            LongOperationState.Timer2Pos = 0;
            LongOperationState.Timer2Max = Adapters.Count;

            foreach (var item in Adapters)
            {
                LongOperationState.Timer2Text = "Загрузка " + item.Prefix;
                item.Dump(force);
                LongOperationState.Timer2Pos++;
            }
        }

        public void Restore(bool force)
        {
            LongOperationState.Timer2Pos = 0;
            LongOperationState.Timer2Max = Adapters.Count * 2;

            foreach (var item in Adapters)
            {
                LongOperationState.Timer2Text = "Проверка " + item.Prefix;
                if (!force)
                {
                    item.Check();
                }
                LongOperationState.Timer2Pos++;
            }

            foreach (var item in Adapters)
            {
                LongOperationState.Timer2Text = "Обновление " + item.Prefix;
                item.Restore(force);
                LongOperationState.Timer2Pos++;
            }
        }
    }
}
