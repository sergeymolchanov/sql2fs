using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace sql2fsbase
{
    [Serializable]
    public class ProjectSettings
    {
        private const String CONFIG_FILE = ".projconfig.xml";
        private String _fileName;

        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(ProjectSettings));

        [XmlAttribute]
        public String ConnectionString { get; set; }
        [XmlAttribute]
        public String ReportServerURL { get; set; }
        [XmlAttribute]
        public String ReportServerUsername { get; set; }
        [XmlAttribute]
        public String ReportServerPassword { get; set; }
        [XmlAttribute]
        public String ReportRoot { get; set; }
        [XmlAttribute]
        public String VorlagenDir { get; set; }

        private ProjectSettings()
        {
        }

        public static ProjectSettings Load(DirectoryInfo dir)
        {
            ProjectSettings retval = null;
            String fn = dir.FullName + @"\" + CONFIG_FILE;

            if (File.Exists(fn))
            {
                using (TextReader reader = new StreamReader(fn))
                {
                    retval = (ProjectSettings) serializer.Deserialize(reader);
                }
            }
            else
            {
                retval = new ProjectSettings();
            }

            retval._fileName = fn;
            return retval;
        }

        public void Save()
        {
            using (TextWriter writer = new StreamWriter(_fileName))
            {
                serializer.Serialize(writer, this);
                writer.Flush();
            }
        }
    }
}
