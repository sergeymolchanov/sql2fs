using System;
using System.Collections.Generic;
using System.Text;
using sql2fsbase.Exceptions;
using sql2fsbase.Adapters.Impl;

namespace sql2fsbase.Adapters
{
    public abstract class AdaptedItem
    {
        System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create();

        protected AdaptedItem(AdapterBase adapter, String name, ProjectDirectory project) : base()
        {
            RemoteModifyDate = null;
            Adapter = adapter;
            Name = name;
            Project = project;
        }

        public String Name { get; protected set; }
        public AdapterBase Adapter { get; private set; }
        public ProjectDirectory Project { get; private set; }
        public bool IsExistsLocal { get; set; }
        public bool IsExistsRemote { get; set; }


        public abstract void Push(byte[] data);
        public abstract byte[] Pull();

        public virtual List<String> ProcessError()
        {
            return new List<string>();
        }

        public void Merge(Common.MergeStyle mergeStyle)
        {
            bool baseExists = Project.ExistsFile(Adapter.Prefix, Name + Adapter.Postfix + ".base");

            DateTime? remoteModifyDate = RemoteModifyDate;
            if (remoteModifyDate != null && baseExists)
            {
                DateTime? localModifyDate = Project.GetLocalModifyTime(Adapter.Prefix, Name + Adapter.Postfix);
                if (localModifyDate != null && localModifyDate.Value.Equals(remoteModifyDate.Value))
                    return;
            }

            byte[] remoteData = Pull();
            byte[] remoteHash = Common.CalculateMD5Hash(remoteData);
            byte[] localData = Project.LoadFile(Adapter.Prefix, Name + Adapter.Postfix);
            byte[] localHash = Common.CalculateMD5Hash(localData);

            byte[] masterHash = Project.LoadFile(Adapter.Prefix, Name + Adapter.Postfix + ".md5");

            bool changedLocal = !Common.ByteArrayEqual(localHash, masterHash);
            bool changedRemote = !Common.ByteArrayEqual(remoteHash, masterHash);
            bool changedHash = (changedLocal || changedRemote) && Common.ByteArrayEqual(localHash, remoteHash);

            if (mergeStyle == Common.MergeStyle.Normal)
            {
                changedLocal = !Common.ByteArrayEqual(localHash, masterHash);
                changedRemote = !Common.ByteArrayEqual(remoteHash, masterHash);
                changedHash = (changedLocal || changedRemote) && Common.ByteArrayEqual(localHash, remoteHash);
            }
            else if (mergeStyle == Common.MergeStyle.Db2Repo)
            {
                changedLocal = false;
                changedRemote = true;
                changedHash = false;
            }
            else if (mergeStyle == Common.MergeStyle.Repo2Db)
            {
                changedLocal = true;
                changedRemote = false;
                changedHash = false;
            }
            else
            {
                throw new NotImplementedException();
            }

            if (changedHash)
            {
                Project.StoreFile(Adapter.Prefix, Name + Adapter.Postfix + ".md5", remoteHash);
                Project.StoreFile(Adapter.Prefix, Name + Adapter.Postfix + ".base", remoteData);
                changedLocal = false;
                changedRemote = false;
            }
            
            if (changedLocal && changedRemote)
            {
                byte[] mergeData = null;

                byte[] baseData = Project.LoadFile(Adapter.Prefix, Name + Adapter.Postfix + ".base");
                mergeData = Project.Tools.MergeFiles(remoteData, localData, baseData);

                if (mergeData == null)
                    throw new ObjectChangedException(this);

                byte[] newHash = Common.CalculateMD5Hash(mergeData);

                Push(mergeData);
                Project.StoreFile(Adapter.Prefix, Name + Adapter.Postfix + ".md5", newHash);
                Project.StoreFile(Adapter.Prefix, Name + Adapter.Postfix + ".base", mergeData);
            }
            else if (changedLocal)
            {
                Push(localData);

                byte[] newRemoteData = Pull();
                byte[] newRemoteHash = Common.CalculateMD5Hash(newRemoteData);
                Project.StoreFile(Adapter.Prefix, Name + Adapter.Postfix + ".md5", newRemoteHash);
                Project.StoreFile(Adapter.Prefix, Name + Adapter.Postfix + ".base", newRemoteData);
            }
            else if (changedRemote)
            {
                if (remoteData != null && remoteData.Length > 0)
                {
                    Project.StoreFile(Adapter.Prefix, Name + Adapter.Postfix, remoteData);
                    Project.StoreFile(Adapter.Prefix, Name + Adapter.Postfix + ".md5", remoteHash);
                    Project.StoreFile(Adapter.Prefix, Name + Adapter.Postfix + ".base", remoteData);

                    if (remoteModifyDate != null)
                        Project.SetLocalModifyTime(Adapter.Prefix, Name + Adapter.Postfix, remoteModifyDate.Value);
                }
                else
                {
                    Project.DeleteFile(Adapter.Prefix, Name + Adapter.Postfix);
                    Project.DeleteFile(Adapter.Prefix, Name + Adapter.Postfix + ".md5");
                    Project.DeleteFile(Adapter.Prefix, Name + Adapter.Postfix + ".base");
                }
            }
            else
            {
                // Всё ОК, только проверим наличие базы

                if (!baseExists)
                {
                    Project.StoreFile(Adapter.Prefix, Name + Adapter.Postfix + ".base", remoteData);
                }
            }
        }

        public DateTime? RemoteModifyDate { get; set; }
    }
}
