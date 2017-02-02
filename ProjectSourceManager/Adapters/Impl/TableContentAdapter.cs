using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;

namespace ProjectSourceManager.Adapters.Impl
{
    public class TableContentAdapter : AdapterBaseSQL
    {
        public TableContentAdapter(ProjectDirectory project) : base(project)
        {
        }

        public override void AddItem(string name)
        {
            Items.Add(new TableContentItem(this, name, Project, Connection));
        }

        public override String Postfix { get { return ".xml"; } }
        public override String Prefix { get { return "Content"; } }

        public override void LoadFromRemote()
        {
            String tableList = null;

            byte[] fileData = Project.LoadFile(Prefix, "tables.conf");

            if (fileData == null)
                return;

            tableList = Encoding.GetEncoding(1251).GetString(fileData);

            foreach (String line in tableList.Split('\n'))
            {
                AddItem(line.Replace("\r", ""));
            }
        }

        public override void LoadFromLocal()
        {
            foreach (var item in Items)
            {
                item.IsExistsLocal = true;
            }
        }
    }
}