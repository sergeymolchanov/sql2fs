using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace ProjectSourceManager.DBContent
{
    [Serializable]
    public class TableRow
    {
        [XmlArrayItem(ElementName = "T")]
        public String[] Value { get; set; }
    }
}
