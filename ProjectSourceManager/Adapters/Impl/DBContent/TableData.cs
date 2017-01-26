using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms.VisualStyles;
using System.Xml.Serialization;

namespace ProjectSourceManager.DBContent
{
    [Serializable]
    public class TableData
    {
        [XmlArrayItem]
        public String[] Fields { get; set; }

        [XmlArrayItem(ElementName = "R")]
        public TableRow[] Rows { get; set; }
    }
}
