using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace ProjectSourceManager
{
    public class TestClass
    {

        public static void DoTest()
        {
            XmlSerializer _serializer = new XmlSerializer(typeof(TestObject));

            TestObjectRow r1 = new TestObjectRow();
            TestObjectRow r2 = new TestObjectRow();
            TestObject o = new TestObject();
            o.Rows = new TestObjectRow[2];

            r1.Add("id", "1");
            r1.Add("name", "sergey");

            r2.Add("id", "2");
            r2.Add("name", "oleg");

            o.Rows[0] = r1;
            o.Rows[1] = r2;

            using (var writer = new StreamWriter(@"C:\Temp\1.txt"))
            {
                _serializer.Serialize(writer, o);
            }

            TestObject o2;

            using (FileStream stream = new FileStream(@"C:\Temp\1.txt", FileMode.Open))
            using (XmlReader reader = XmlReader.Create(stream))
            {
                o2 = (TestObject)_serializer.Deserialize(reader);
            }

            System.Windows.Forms.MessageBox.Show(String.Format("Len = {0}", o2.Rows.Length));
        
                foreach (String key in o2.Rows[0].Keys)
                {
                    System.Windows.Forms.MessageBox.Show(String.Format("Row1 {0} {1}", key, o2.Rows[0][key]));
                }
        }

        [XmlRoot("Object")]
        public class TestObject
        {
            [XmlArrayItem(ElementName = "Row")]
            public TestObjectRow[] Rows {get; set;}
        }

        [XmlRoot("item")]
        public class TestObjectRow : Dictionary<String, String>, IXmlSerializable
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
    }
}
