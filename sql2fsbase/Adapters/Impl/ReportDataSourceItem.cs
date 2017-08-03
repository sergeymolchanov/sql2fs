using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql2fsbase.Adapters.Impl
{
    public class ReportDataSourceItem : AdaptedItem
    {
        public ReportDataSourceItem(AdapterBase adapter, String name, ProjectDirectory project, ReportingService2010 service)
            : base(adapter, name, project)
        {
            Service = service;
        }

        public ReportingService2010 Service { get; private set; }

        private String FullName { get { return "/" + Project.Settings.ReportRoot + "/" + Name; } }

        public override void Push(byte[] data)
        {
            if (data == null)
            {
                Service.DeleteItem(FullName);
            }
            if (!this.IsExistsRemote)
            {
                Warning[] warn = new Warning[] { };
                Property[] prop = new Property[] { };

                String[] spl = FullName.Split('/');
                String name = spl[spl.Length - 1];
                String path = "";
                for (int i = 0; i < spl.Length - 1; i++)
                    path = path == "" ? spl[i] : path + "/" + spl[i];

                Service.CreateCatalogItem("DataSource", name, "/" + path, true, data, prop, out warn);
            }
        }

        public override byte[] Pull()
        {
            if (!this.IsExistsRemote)
                return null;

            byte[] def = Service.GetItemDefinition(FullName);
            String xml = Encoding.Unicode.GetString(def);
            def = Encoding.Default.GetBytes(xml);
            return def;
        }
    }
}
