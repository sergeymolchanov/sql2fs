using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
using System.Xml.Serialization;

namespace ProjectSourceManager
{
    public class Common
    {
        static readonly MD5 md5 = System.Security.Cryptography.MD5.Create();

        public static readonly DirectoryInfo RootDir = new DirectoryInfo(Application.StartupPath);

        public static byte[] CalculateMD5Hash(byte[] inputBytes)
        {
            if (inputBytes == null)
                return null;

            byte[] hash = md5.ComputeHash(inputBytes);

            StringBuilder sb = new StringBuilder();

            for (int i = 0; i < hash.Length; i++)
            {
                sb.Append(hash[i].ToString("X2"));
            }

            return Encoding.UTF8.GetBytes(sb.ToString());
        }

        public static readonly Encoding DefaultEncoding = Encoding.GetEncoding(1251);

        public static byte[] ConvertTo(String data, Encoding encoding)
        {
            byte[] dataBytes = encoding.GetBytes(data);
            return Encoding.Convert(encoding, DefaultEncoding, dataBytes);
        }

        public static String ConvertFrom(byte[] data, Encoding encoding)
        {
            if (data == null)
                return null;

            byte[] convBytes = Encoding.Convert(DefaultEncoding, encoding, data);

            return encoding.GetString(convBytes);
        }

        public static Encoding GetEncoding(String line)
        {
            Encoding retval = DefaultEncoding;

            String firstLine = line;
            int lineEnd = firstLine.IndexOf('\n');
            if (lineEnd > 0)
                firstLine = firstLine.Substring(0, lineEnd);
            firstLine = firstLine.Replace('"', '\'').ToLower().Replace(" ", "");
            String estr = "encoding='";
            int startPos = firstLine.IndexOf("encoding='");
            if (startPos <= 0 || firstLine.Length < startPos + 3)
                return retval;
            startPos += estr.Length;
            int endPos = firstLine.IndexOf("'", startPos + 1);
            if (startPos > estr.Length && endPos > 0)
            {
                String encodingName = firstLine.Substring(startPos, endPos - startPos);
                return Encoding.GetEncoding(encodingName);
            }

            return retval;
        }

        public static string Serialize<T>(T value)
        {
            if (value == null)
            {
                return null;
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlWriterSettings settings = new XmlWriterSettings()
            {
                Encoding = DefaultEncoding,
                CloseOutput = false,
                OmitXmlDeclaration = false,
                Indent = true
            };

            using (StringWriter textWriter = new StringWriterWithEncoding(Common.DefaultEncoding))
            {
                using (XmlWriter xmlWriter = XmlWriter.Create(textWriter, settings))
                {
                    serializer.Serialize(xmlWriter, value);
                }
                String retval = textWriter.ToString();
                return retval;
            }
        }

        public static T Deserialize<T>(string xml)
        {
            if (string.IsNullOrEmpty(xml))
            {
                return default(T);
            }

            XmlSerializer serializer = new XmlSerializer(typeof(T));

            XmlReaderSettings settings = new XmlReaderSettings();

            using (StringReader textReader = new StringReader(xml))
            {
                using (XmlReader xmlReader = XmlReader.Create(textReader, settings))
                {
                    return (T)serializer.Deserialize(xmlReader);
                }
            }
        }

        public sealed class StringWriterWithEncoding : System.IO.StringWriter
        {
            private readonly System.Text.Encoding encoding;

            public StringWriterWithEncoding(System.Text.StringBuilder sb)
                : base(sb)
            {
                this.encoding = System.Text.Encoding.Unicode;
            }

            public StringWriterWithEncoding(System.Text.Encoding encoding)
            {
                this.encoding = encoding;
            }

            public StringWriterWithEncoding(System.Text.StringBuilder sb, System.Text.Encoding encoding)
                : base(sb)
            {
                this.encoding = encoding;
            }

            public override System.Text.Encoding Encoding
            {
                get { return encoding; }
            }
        }

        [Serializable]
        public class GlobalSettings
        {
            private static readonly String _file = Common.RootDir.FullName + @"/global.conf";
            private static GlobalSettings _instance = null;
            public static GlobalSettings Instance
            {
                get
                {
                    if (_instance == null)
                        Load();

                    return _instance;
                }
            }

            [XmlAttribute]
            public String UserName { get; set; }
            [XmlAttribute]
            public String Email { get; set; }
            [XmlAttribute]
            public String LastProject { get; set; }

            public static void Save()
            {
                using (StreamWriter w = new StreamWriter(_file))
                {
                    w.WriteLine(Common.Serialize<GlobalSettings>(Instance));
                }
            }

            public static void Load()
            {
                if (!File.Exists(_file))
                {
                    _instance = new GlobalSettings();
                    return;
                }

                using (TextReader r = new StreamReader(_file))
                {
                    _instance = Common.Deserialize<GlobalSettings>(r.ReadToEnd());
                }
            }
        }
    }
}