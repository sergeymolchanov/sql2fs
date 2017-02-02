using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSourceManager.Adapters.Impl
{
    public class ReportItem : AdaptedItem
    {
        public ReportItem(AdapterBase adapter, String name, ProjectDirectory project, ReportingService2010 service)
            : base(adapter, name, project)
        {
            Service = service;
        }

        public ReportingService2010 Service { get; private set; }

        public override void Push(byte[] data)
        {
            Service.SetItemDefinition("/" + Name, data, new Property[] {});
        }

        public override byte[] Pull()
        {
            byte[] reportDefinition = Service.GetItemDefinition("/" + Name);
            /*
            System.Xml.XmlDocument doc = new System.Xml.XmlDocument();
            reportDefinition = Service.GetItemDefinition("/" + Name);
            MemoryStream stream = new MemoryStream(reportDefinition, 0, reportDefinition.Length);

            string myDocumentsFolder =
                Environment.GetFolderPath(
                    Environment.SpecialFolder.Personal);

            byte[] retval = new byte[stream.Length];
            stream.GetBuffer().CopyTo(retval, stream.Length);
            */
            return reportDefinition;
        }
    }
}
