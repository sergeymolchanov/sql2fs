using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSourceManager.Adapters.Impl
{
    public class ReportAdapter : AdapterBase
    {
        private ReportingService2010 rs = new ReportingService2010();

        public ReportAdapter(ProjectDirectory project) : base(project)
        {
            rs.Url = String.Format("{0}/ReportService2010.asmx", Project.Settings.ReportServerURL); // http://localhost/ReportServer
            rs.Credentials = new NetworkCredential(Project.Settings.ReportServerUsername, Project.Settings.ReportServerPassword);
        }

        public override void LoadFromRemote()
        {
            foreach (var item in rs.ListChildren("/" + Project.Settings.ReportRoot, true))
            {
                if (item.TypeName == "Report")
                {                    
                    String name = item.Path;
                    if (name.StartsWith("/"))
                        name = name.Substring(1);
                    ReportItem i = new ReportItem(this, name, Project, rs);
                    i.RemoteModifyDate = item.ModifiedDate;
                    Items.Add(i);
                }
            }
        }

        public override void AddItem(string name)
        {
            Items.Add(new ReportItem(this, name, Project, rs));
        }

        public override String Prefix { get { return "Reports"; } }
        public override String Postfix { get { return ".rdl"; } }
    }
}
