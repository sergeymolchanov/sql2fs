using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;
using System.Text.RegularExpressions;

namespace sql2fsbase.Adapters
{
    public abstract class AdapterBase
    {
        public ProjectDirectory Project { get; private set; }
        public List<AdaptedItem> Items { get; private set; }

        public String DataPath { get { return Project.Dir.FullName + @"\" + Prefix; } }

        protected AdapterBase(ProjectDirectory project)
        {
            Project = project;
            Items = new List<AdaptedItem>();
        }

        public abstract String Prefix { get; }
        public virtual String Postfix { get { return ""; } }
        public abstract void LoadFromRemote();

        private List<Regex> excludeObjects = null;
        private List<Regex> includeObjects = null;

        private Regex getRegex(String line)
        {
            return new Regex(line.Replace("*", ".*"), RegexOptions.IgnoreCase);
        }
        private void loadExceptions()
        {
            if (excludeObjects != null)
                return;

            excludeObjects = new List<Regex>();
            includeObjects = new List<Regex>();

            if (File.Exists(DataPath + @"\.exclude.conf"))
            {
                using (StreamReader sr = new StreamReader(DataPath + @"\.exclude.conf"))
                {
                    String line = null;
                    while ((line = sr.ReadLine()) != null)
                        if (line.Replace(" ", "").Length > 0)
                            excludeObjects.Add(getRegex(line.Replace(" ", "")));
                }
            }

            if (File.Exists(DataPath + @"\.include.conf"))
            {
                using (StreamReader sr = new StreamReader(DataPath + @"\.include.conf"))
                {
                    String line = null;
                    while ((line = sr.ReadLine()) != null)
                        if (line.Replace(" ", "").Length > 0)
                            includeObjects.Add(getRegex(line.Replace(" ", "")));
                }
            }
        }

        public bool IsExcluded(String line)
        {
            loadExceptions();

            foreach (Regex reg in includeObjects)
                if (reg.IsMatch(line))
                    return false;

            foreach (Regex reg in excludeObjects)
                if (reg.IsMatch(line))
                    return true;

            return false;
        }

        private void LoadFromLocal(String basePath, String path)
        {
            DirectoryInfo dir = new DirectoryInfo(basePath + "/" + path);

            foreach (var fi in dir.GetFiles())
            {
                String iname = fi.Name;
                if (iname.EndsWith(".md5") || iname.EndsWith(".date") || iname.EndsWith(".base"))
                    continue;
                if (!iname.EndsWith(Postfix))
                    continue;

                iname = iname.Substring(0, iname.Length - Postfix.Length);

                AdaptedItem foundItem = null;
                foreach (var item in Items)
                {
                    if (item.Name.Equals(iname))
                        foundItem = item;
                }

                if (foundItem != null)
                {
                    foundItem.IsExistsLocal = true;
                }
                else
                {
                    AddItem(path == ""? iname : path + "/" + iname);
                }
            }

            foreach (DirectoryInfo sdi in dir.GetDirectories())
            {
                LoadFromLocal(basePath, path == "" ? sdi.Name : path + "/" + sdi.Name);
            }
        }

        public virtual void LoadFromLocal()
        {
            if (!Directory.Exists(DataPath))
                return;

            LoadFromLocal(DataPath, "");
        }

        public abstract void AddItem(String name);

        private void checkTargetDir()
        {
            String path = Project.Dir.FullName + @"\" + Prefix;
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);
        }

        public void Merge()
        {
            OnBeforeSync();

            checkTargetDir();
            clearErrors();

            LongOperationState.Timer1Pos = 0;
            LongOperationState.Timer1Max = Items.Count;

            DoSort();

            foreach (var item in Items)
            {
                LongOperationState.Timer1Pos++;
                LongOperationState.Timer1Text = item.Name;

                item.Merge();
            }

            OnAfterSync();

            processErrors();
        }

        protected virtual void processErrors()
        {
        }

        protected virtual void clearErrors()
        {
        }

        protected virtual void DoSort()
        {
        }

        public virtual void OnBeforeSync()
        {
        }
        public virtual void OnAfterSync()
        {
        }
    }
}
