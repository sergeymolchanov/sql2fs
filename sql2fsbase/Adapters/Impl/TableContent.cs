using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace sql2fsbase.Adapters.Impl
{
    public class TableContent
    {
        [XmlRoot("Object")]
        public class TableData
        {
            [XmlElement("TableName")]
            public String TableName { get; set; }

            [XmlElement("PrimaryKey")]
            public String PrimaryKey { get; set; }

            [XmlArrayItem(ElementName = "FieldsInsertOnly")]
            public String[] FieldsInsertOnly { get; set; }

            [XmlArrayItem(ElementName = "Row")]
            public TableRow[] Rows { get; set; }

            public Dictionary<String, TableRowLocal> BuildRowDict()
            {
                Dictionary<String, TableRowLocal> ret = new Dictionary<String, TableRowLocal>();
                foreach (TableRow r in Rows)
                {
                    ret.Add(TableName + "--" + r.Data[PrimaryKey], new TableRowLocal() { Row = r, Data = this });

                    if (r.SubTableData != null)
                    {
                        foreach (TableData d in r.SubTableData)
                        {
                            foreach (KeyValuePair<String, TableRowLocal> kv in d.BuildRowDict())
                            {
                                ret.Add(kv.Key, kv.Value);
                            }
                        }
                    }
                }

                return ret;
            }
        }

        public class TableRowLocal
        {
            public TableRow Row { get; set; }
            public TableData Data { get; set; }
        }
        
        [Serializable]
        [XmlRoot("item")]
        public class TableRow
        {
            [XmlElement("Data")]
            public TableRowData Data { get; set; }

            [XmlElement("SubTableData")]
            public TableData[] SubTableData { get; set; }

            public bool DataEquals(TableRow other)
            {
                if (other == null)
                    return false;

                if (other.Data.Count != this.Data.Count)
                    return false;

                foreach (var r in other.Data)
                {
                    if (!this.Data.ContainsKey(r.Key))
                        return false;

                    if (!this.Data[r.Key].Equals(r.Value))
                        return false;
                }

                return true;
            }
        }

        [Serializable]
        [XmlRoot("itemData")]
        public class TableRowData : Dictionary<String, String>, IXmlSerializable
        {
            public System.Xml.Schema.XmlSchema GetSchema()
            {
                return null;
            }

            public void ReadXml(System.Xml.XmlReader reader)
            {
                bool wasEmpty = reader.IsEmptyElement;
                reader.Read();
                if (wasEmpty)
                    return;

                while (reader.NodeType != System.Xml.XmlNodeType.EndElement)
                {
                    reader.ReadStartElement();
                    String value = reader.ReadContentAsString();
                    String name = reader.Name;

                    this.Add(name, value);
                    reader.ReadEndElement();
                    reader.MoveToContent();
                }
                reader.ReadEndElement();
            }

            public void WriteXml(System.Xml.XmlWriter writer)
            {
                XmlSerializer valueSerializer = new XmlSerializer(typeof(String));
                foreach (String key in this.Keys)
                {
                    String value = this[key];

                    writer.WriteStartElement(key);
                    writer.WriteCData(value);
                    writer.WriteEndElement();
                }
            }
        }

        [Serializable]
        [XmlRoot("ContentConfig")]
        public class ContentConfig
        {
            [XmlArrayItem(ElementName = "Table")]
            public TableConfig[] Tables { get; set; }
        }

        [Serializable]
        [XmlRoot("Table")]
        public class TableConfig
        {
            public TableConfig()
            {
                Name = "";
                PrimaryKey = "";
                SplitCond = "";
                Filter = "";
                Fields = new string[0];
                FieldsInsertOnly = new string[0];
                SubTables = new SubTableConfig[0];
            }

            [XmlElement("Name")]
            public String Name { get; set; }

            [XmlElement("PrimaryKey")]
            public String PrimaryKey { get; set; }

            [XmlElement("SplitCond")]
            public String SplitCond { get; set; }

            [XmlElement("Filter")]
            public String Filter { get; set; }

            [XmlArrayItem(ElementName = "Field")]
            public String[] Fields { get; set; }

            [XmlArrayItem(ElementName = "FieldsInsertOnly")]
            public String[] FieldsInsertOnly { get; set; }

            [XmlArrayItem(ElementName = "SubTable")]
            public SubTableConfig[] SubTables { get; set; }

            public String[] GetAllFields()
            {
                String[] ret = new String[Fields.Length + FieldsInsertOnly.Length + 1];

                ret[0] = PrimaryKey;

                for (int i = 0; i < Fields.Length; i++)
                    ret[i + 1] = Fields[i];

                for (int i = 0; i < FieldsInsertOnly.Length; i++)
                    ret[Fields.Length + i + 1] = FieldsInsertOnly[i];

                return ret;
            }
        }

        [Serializable]
        [XmlRoot("SubTable")]
        public class SubTableConfig : TableConfig
        {
            [XmlElement("ParentCond")]
            public String ParentCond { get; set; }
        }
    }
}
