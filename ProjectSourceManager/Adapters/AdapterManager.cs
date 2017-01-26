using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ProjectSourceManager.Adapters.Impl;

namespace ProjectSourceManager.Adapters
{
    public class AdapterManager
    {
        public ProjectDirectory Project { get; private set; }
        public List<AdapterBase> Adapters { get; private set; }

        public AdapterManager(ProjectDirectory project)
        {
            Project = project;
            Adapters = new List<AdapterBase>();

            if (Project.Settings.ReportServerURL.Length > 5)
                Adapters.Add(new ReportAdapter(project));

            Adapters.Add(new TableContentAdapter(project));
            Adapters.Add(new VorlagenAdapter(project));
            Adapters.Add(new StoredProcAdapter(project));

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

        public void Dump()
        {
            ProgressBarForm.Instance.Timer2Pos = 0;
            ProgressBarForm.Instance.Timer2Max = Adapters.Count;

            foreach (var item in Adapters)
            {
                ProgressBarForm.Instance.Timer2Text = "Загрузка " + item.Prefix;
                item.Dump();
                ProgressBarForm.Instance.Timer2Pos++;
            }
        }

        public void Restore()
        {
            ProgressBarForm.Instance.Timer2Pos = 0;
            ProgressBarForm.Instance.Timer2Max = Adapters.Count * 2;

            foreach (var item in Adapters)
            {
                ProgressBarForm.Instance.Timer2Text = "Проверка " + item.Prefix;
                item.Check();
                ProgressBarForm.Instance.Timer2Pos++;
            }

            foreach (var item in Adapters)
            {
                ProgressBarForm.Instance.Timer2Text = "Обновление " + item.Prefix;
                item.Restore();
                ProgressBarForm.Instance.Timer2Pos++;
            }
        }
    }
}
