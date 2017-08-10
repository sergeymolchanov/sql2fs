using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql2fsbase.Adapters.Impl
{
    public class VorlagenAdapter : AdapterBase
    {
        public VorlagenAdapter(ProjectDirectory project) : base(project)
        {
        }

        public override void AddItem(string name, bool isLocal)
        {
            Items.Add(new VorlagenItem(this, name, Project) { IsExistsLocal = isLocal, IsExistsRemote = !isLocal });
        }

        public override String Prefix
        {
            get { return "Vorlagen"; }
        }

        public override void LoadFromRemote()
        {
            if (Project.Settings.VorlagenDir.Length < 5)
                return;

            DirectoryInfo dir = new DirectoryInfo(Project.Settings.VorlagenDir);

            foreach (FileInfo file in dir.GetFiles())
            {
                VorlagenItem i = new VorlagenItem(this, file.Name, Project);
                i.RemoteModifyDate = file.LastWriteTime;
                Items.Add(i);
            }
        }
    }
}
