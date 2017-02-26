using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace sql2fsbase.DBContent
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
