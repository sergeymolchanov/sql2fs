using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Xml;
using System.Xml.Serialization;

namespace sql2fsbase
{
    public class Common
    {
        static readonly MD5 md5 = System.Security.Cryptography.MD5.Create();

        public static readonly String GitProc = "TortoiseGitProc.exe";

        public static readonly DirectoryInfo RootDir = new DirectoryInfo(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location));
        public static readonly String VersionedObjectTypes = "'FN', 'IF', 'P', 'TF', 'TR', 'V'";

        public static Action<String> OutProc { get; set; }

        public static void Out(String line, params String[] param)
        {
            if (OutProc != null)
                OutProc(String.Format(line, param));
        }

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

        public static byte[] LoadFile(String name)
        {
            if (!File.Exists(name))
                return null;

            return File.ReadAllBytes(name);
        }

        public static void StoreFile(String name, byte[] data)
        {
            if (data == null)
            {
                File.Delete(name);
            }
            else
            {
                String subPath = "";
                String[] pathElements = name.Replace("/", "\\").Split('\\');
                for (int i = 0; i < pathElements.Length - 1; i++)
                {
                    String subdir = pathElements[i];

                    if (subdir.Length == 0)
                        continue;

                    if (subPath == "")
                        subPath = subdir;
                    else
                        subPath += "\\" + subdir;
                    if (!Directory.Exists(subPath))
                        Directory.CreateDirectory(subPath);
                }

                File.WriteAllBytes(name, data);
            }
        }

        public static bool ByteArrayEqual(byte[] b1, byte[] b2)
        {
            if (b1 == null && b2 == null)
                return true;

            if (b1 == null || b2 == null || b1.Length != b2.Length)
                return false;

            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i])
                    return false;
            }

            return true;
        }

        public enum MergeStyle
        {
            Normal,
            Repo2Db,
            Db2Repo
        }
    }
}