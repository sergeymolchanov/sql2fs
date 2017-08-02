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

        public static TableContent.ISqlErrorView SqlErrorViewInstance { get; set; }

        public AdapterManager(ProjectDirectory project)
        {
            if (SqlErrorViewInstance == null)
                throw new NullReferenceException();

            Project = project;
            Adapters = new List<AdapterBase>();

            if (Project.Settings.ConnectionString != null && Project.Settings.ConnectionString.Length > 5)
            {
                Adapters.Add(new TableContentAdapter(project, SqlErrorViewInstance));
                Adapters.Add(new DDLAdapter(project, SqlErrorViewInstance));
                Adapters.Add(new StoredProcAdapter(project, SqlErrorViewInstance));
            }

            if (Project.Settings.ReportServerURL != null && Project.Settings.ReportServerURL.Length > 5)
                Adapters.Add(new ReportAdapter(project));

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
            LongOperationState.Timer2Pos = 0;
            LongOperationState.Timer2Max = Adapters.Count;

            foreach (var item in Adapters)
            {
                LongOperationState.Timer2Text = "Загрузка " + item.Prefix;
                item.Merge();
                LongOperationState.Timer2Pos++;
            }

            LongOperationState.Timer1Text = "Завершено";
            LongOperationState.Timer2Text = "";
            LongOperationState.Timer1Pos = LongOperationState.Timer1Max;
            LongOperationState.Timer2Pos = LongOperationState.Timer2Max;
        }
    }
}
