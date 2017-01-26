using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSourceManager.Adapters.Impl
{
    public class VorlagenAdapter : AdapterBase
    {
        public VorlagenAdapter(ProjectDirectory project) : base(project)
        {
        }

        public override void AddItem(string name)
        {
            Items.Add(new VorlagenItem(this, name, Project));
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
                AddItem(file.Name);
            }
        }
    }
}
