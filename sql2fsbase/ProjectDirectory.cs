﻿using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using sql2fsbase.Adapters;

namespace sql2fsbase
{
    public class ProjectDirectory : IComparable<ProjectDirectory>
    {
        private const String VORLAGEN_DIR = "Vorlagen";
        private const String VALID_COMMIT_FILE = ".validcommit";
        private const String REPO_LOCK_FILE = ".repolock";

        public DirectoryInfo Dir { get; private set; }
        public String Name { get { return Dir.Name; } }
        public ProjectSettings Settings { get; private set; }

        public ProjectDirectory(DirectoryInfo dir)
        {
            Dir = dir;
            Settings = ProjectSettings.Load(dir);
        }

        public void Dump(bool force)
        {
            Common.Out("Dump '{0}'{1}", Name, force ? " with FORCE" : "");

            String actualCommitId = GetActualCommitId();
            byte[] validCommitBytes = this.LoadFile("", VALID_COMMIT_FILE);
            if (!force && actualCommitId != "" && validCommitBytes != null)
            {
                String validCommitId = Encoding.ASCII.GetString(validCommitBytes);

                if (!validCommitId.Equals(actualCommitId))
                    throw new Exception(
                        "Переключение на разработку БД произошло с другой версии репозитария. Для переключения обратно необходимо откатиться на коммит " +
                        validCommitId + " Иначе можно затереть изменения в репозитарии.");
            }

            AdapterManager manager = new AdapterManager(this);
            manager.Dump(force);

            IsRepoLocked = false;
        }

        public void Restore(bool force)
        {
            Common.Out("Restore '{0}'{1}", Name, force ? " with FORCE" : "");

            String actualCommitId = GetActualCommitId();
            this.StoreFile("", VALID_COMMIT_FILE, Encoding.ASCII.GetBytes(actualCommitId));

            AdapterManager manager = new AdapterManager(this);
            manager.Restore(force);

            IsRepoLocked = true;
        }

        public void Merge(bool onlyCheck)
        {
            if (onlyCheck)
                Common.Out("Check '{0}'", Name);
            else
                Common.Out("Merge '{0}'", Name);

            AdapterManager manager = new AdapterManager(this);
            manager.Merge(onlyCheck);
        }

        public bool IsRepoLocked
        {
            get { return this.LoadFile("", REPO_LOCK_FILE) != null; }
            private set
            {
                if (value)
                    this.StoreFile("", REPO_LOCK_FILE, new byte[1] { 0 });
                else
                    this.DeleteFile("", REPO_LOCK_FILE);
            }
        }

        public override string ToString()
        {
            return Name;
        }

        public int CompareTo(ProjectDirectory val)
        {
            return Name.CompareTo(val.Name);
        }

        public void SaveSettings()
        {
            Settings.Save();
        }

        public void Commit()
        {
            RunTortoiseGitCommand("commit");
        }

        public void Pull(bool gui)
        {
            if (gui)
                RunTortoiseGitCommand("pull");
            else
                RunGitCommand("pull");
        }

        public void Push()
        {
            RunTortoiseGitCommand("push");
        }

        public void MergeWindow()
        {
            RunTortoiseGitCommand("merge");
        }

        public void Log()
        {
            RunTortoiseGitCommand("log");
        }

        public void Switch()
        {
            RunTortoiseGitCommand("switch");
        }


        public void StoreFile(String prefix, String name, byte[] data)
        {
            if (data == null)
            {
                File.Delete(String.Format(@"{0}\{1}\{2}", Dir.FullName, prefix, name));
            }
            else
            {
                String _file = String.Format(@"{0}\{1}\{2}", Dir.FullName, prefix, name);

                String subPath = "";
                String[] pathElements = _file.Replace("/", "\\").Split('\\');
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

                File.WriteAllBytes(_file, data);
            }
        }

        public byte[] LoadFile(String prefix, String name)
        {
            String _file = String.Format(@"{0}\{1}\{2}", Dir.FullName, prefix, name);

            if (!File.Exists(_file))
                return null;

            return File.ReadAllBytes(_file);
        }

        public void DeleteFile(String prefix, String name)
        {
            StoreFile(prefix, name, null);
        }

        public DateTime? GetLocalModifyTime(String prefix, String name)
        {
            String _file = String.Format(@"{0}\{1}\{2}", Dir.FullName, prefix, name);
            if (!File.Exists(_file))
                return null;
            return File.GetLastWriteTime(_file);
        }

        public void SetLocalModifyTime(String prefix, String name, DateTime date)
        {
            String _file = String.Format(@"{0}\{1}\{2}", Dir.FullName, prefix, name);
            if (!File.Exists(_file))
                return;
            File.SetLastWriteTime(_file, date);
        }

        private String RunGitCommand(String cmd)
        {
            Common.Out("Execute " + cmd);
            String dest = Dir.FullName;
            String gitProc = String.Format(@"git", Common.RootDir.FullName);

            StringBuilder b = new StringBuilder();

            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.WorkingDirectory = dest;
            p.StartInfo.FileName = gitProc;
            p.StartInfo.Arguments = cmd;
            p.StartInfo.CreateNoWindow = true;
            p.OutputDataReceived += (sender, args) => b.AppendLine(args.Data);
            p.ErrorDataReceived += (sender, args) => b.AppendLine(args.Data);
            p.Start();

            p.WaitForExit();
            Common.Out(b.ToString());
            return b.ToString();
        }

        private String RunTortoiseGitCommand(String cmd)
        {
            Common.Out("Execute " + cmd);
            String dest = Dir.FullName;
            String gitProc = String.Format(@"TortoiseGitProc.exe", Common.RootDir.FullName);
            String fullCmd = String.Format(@"/command:{0} /path:{1}", cmd, Dir.FullName);
            Process p = new Process();
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.RedirectStandardOutput = true;
            p.StartInfo.WorkingDirectory = dest;
            p.StartInfo.FileName = gitProc;
            p.StartInfo.Arguments = fullCmd;
            p.Start();

            string output = p.StandardOutput.ReadToEnd();
            p.WaitForExit();

            return output;
        }

        private String GetActualCommitId()
        {
            String commit = RunGitCommand("show-ref --head HEAD");
            String commitHash = commit.Split(' ')[0];

            return commitHash.Trim();
        }

        public void CheckGitHooks()
        {
            try
            {
                DirectoryInfo di = new DirectoryInfo(Dir.FullName + "/Hooks/Git");
                foreach (FileInfo fi in di.GetFiles())
                {
                    try
                    {
                        File.Copy(fi.FullName, Dir.FullName + "/.git/hooks/" + fi.Name, true);
                    }
                    catch (IOException e)
                    { }
                }
            }
            catch (IOException e)
            { }
        }
    }
}