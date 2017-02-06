using System;
using System.Collections.Generic;
using System.IO;
using System.Windows.Forms;
using ProjectSourceManager.Adapters.Impl.DBContent;
using System.Data.SqlClient;

namespace ProjectSourceManager.Adapters
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

        public virtual void LoadFromLocal()
        {
            if (!Directory.Exists(DataPath))
                return;

            foreach (var fi in new DirectoryInfo(DataPath).GetFiles())
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
                    AddItem(iname);
                }
            }
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

            ProgressBarForm.Instance.Timer1Pos = 0;
            ProgressBarForm.Instance.Timer1Max = Items.Count;

            foreach (var item in Items)
            {
                item.Dump();

                ProgressBarForm.Instance.Timer1Pos++;
                ProgressBarForm.Instance.Timer1Text = item.Name;
            }
        }

        public void Restore(bool force)
        {
            checkTargetDir();
            clearErrors();

            ProgressBarForm.Instance.Timer1Pos = 0;
            ProgressBarForm.Instance.Timer1Max = Items.Count;
            Application.DoEvents();

            DoSort();

            foreach (var item in Items)
            {
                ProgressBarForm.Instance.Timer1Pos++;
                ProgressBarForm.Instance.Timer1Text = item.Name;

                item.Restore(false);
                Application.DoEvents();
            }

            processErrors();
        }

        public void Check()
        {
            checkTargetDir();

            ProgressBarForm.Instance.Timer1Pos = 0;
            ProgressBarForm.Instance.Timer1Max = Items.Count;

            foreach (var item in Items)
            {
                ProgressBarForm.Instance.Timer1Pos++;
                ProgressBarForm.Instance.Timer1Text = item.Name;

                item.Restore(true);
                Application.DoEvents();
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
