using System;
using System.Collections.Generic;
using System.IO;
using System.Data.SqlClient;

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

        private void LoadFromLocal(String basePath, String path)
        {
            DirectoryInfo dir = new DirectoryInfo(basePath + "/" + path);

            foreach (var fi in dir.GetFiles())
            {
                String iname = fi.Name;
                if (iname.EndsWith(".md5") || iname.EndsWith(".date"))
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

        public void Dump(bool force)
        {
            checkTargetDir();

            LongOperationState.Timer1Pos = 0;
            LongOperationState.Timer1Max = Items.Count;

            foreach (var item in Items)
            {
                item.Dump();

                LongOperationState.Timer1Pos++;
                LongOperationState.Timer1Text = item.Name;
            }
        }

        public void Restore(bool force)
        {
            checkTargetDir();
            clearErrors();

            LongOperationState.Timer1Pos = 0;
            LongOperationState.Timer1Max = Items.Count;

            DoSort();

            foreach (var item in Items)
            {
                LongOperationState.Timer1Pos++;
                LongOperationState.Timer1Text = item.Name;

                item.Restore(false, force);
            }

            processErrors();
        }

        public void Check()
        {
            checkTargetDir();

            LongOperationState.Timer1Pos = 0;
            LongOperationState.Timer1Max = Items.Count;

            foreach (var item in Items)
            {
                LongOperationState.Timer1Pos++;
                LongOperationState.Timer1Text = item.Name;

                item.Restore(true, false);
            }
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
    }
}
