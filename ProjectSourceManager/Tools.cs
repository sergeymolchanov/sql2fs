using ProjectSourceManager.Adapters.Impl.DBContent;
using sql2fsbase;
using sql2fsbase.Adapters.Impl;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSourceManager
{
    public class Tools : ITools
    {
        private SQLErrorView fSQLErrorView = new SQLErrorView();

        public byte[] MergeFiles(byte[] dbData, byte[] vkData, byte[] baseData)
        {
            String tempPath = Environment.GetEnvironmentVariable("TEMP");
            String dbFile = tempPath + "\\dbFile";
            String vkFile = tempPath + "\\vkFile";
            String baseFile = tempPath + "\\baseFile";
            String resultFile = tempPath + "\\resultFile";

            if (dbData == null)
                dbData = new byte[0];
            if (vkData == null)
                vkData = new byte[0];
            if (baseData == null)
                baseData = new byte[0];

            Common.StoreFile(dbFile, dbData);
            Common.StoreFile(vkFile, vkData);
            Common.StoreFile(baseFile, baseData);
            Common.StoreFile(resultFile, null);

            String cmd = "diff";

            Common.Out("Execute " + cmd);
            String fullCmd = String.Format("/base:\"{0}\" /mine:\"{1}\" /theirs:\"{2}\" /merged:\"{3}\"", baseFile, dbFile, vkFile, resultFile);
            Process p = new Process();
            p.StartInfo.UseShellExecute = true;
            p.StartInfo.WorkingDirectory = tempPath;
            p.StartInfo.FileName = "TortoiseGitMerge";
            p.StartInfo.Arguments = fullCmd;
            p.Start();
            p.WaitForExit();

            return Common.LoadFile(resultFile);
        }

        public bool ShowSQL(List<AdapterBaseSQL.AdapterSqlException> queryList)
        {
            return fSQLErrorView.ShowSQL(queryList);
        }

        public Common.MergeStyle AskHowToMerge()
        {
            return AskMergeStyleForm.Ask();
        }
    }
}
