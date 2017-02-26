using System;
using System.Collections.Generic;
using System.Text;
using sql2fsbase.Exceptions;

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

        public void Dump()
        {
            DateTime? remoteModifyDate = RemoteModifyDate;
            if (remoteModifyDate != null)
            {
                DateTime? localModifyDate = Project.GetLocalModifyTime(Adapter.Prefix, Name + Adapter.Postfix);
                if (localModifyDate != null && localModifyDate.Value.Equals(remoteModifyDate.Value))
                    return;
            }

            byte[] data = Pull();

            if (data != null && data.Length > 0)
            {
                byte[] hash = Common.CalculateMD5Hash(data);

                Project.StoreFile(Adapter.Prefix, Name + Adapter.Postfix, data);
                Project.StoreFile(Adapter.Prefix, Name + Adapter.Postfix + ".md5", hash);

                if (remoteModifyDate != null)
                    Project.SetLocalModifyTime(Adapter.Prefix, Name + Adapter.Postfix, remoteModifyDate.Value);
            }
            else
            {
                Project.DeleteFile(Adapter.Prefix, Name + Adapter.Postfix);
                Project.DeleteFile(Adapter.Prefix, Name + Adapter.Postfix + ".md5");
            }
        }

        public void Restore(bool isCheckOnly, bool force)
        {
            DateTime? remoteModifyDate = RemoteModifyDate;
            if (remoteModifyDate != null)
            {
                DateTime? localModifyDate = Project.GetLocalModifyTime(Adapter.Prefix, Name + Adapter.Postfix);
                if (localModifyDate != null && localModifyDate.Value.Equals(remoteModifyDate.Value))
                    return;
            }

            byte[] objData = Pull();
            byte[] objHash = Common.CalculateMD5Hash(objData);
            byte[] loadedObjHash = Project.LoadFile(Adapter.Prefix, Name + Adapter.Postfix + ".md5");

            if (force == false && objData != null && !ByteArrayEqual(objHash, loadedObjHash))
                throw new ObjectChangedException(this);

            if (isCheckOnly) return;

            byte[] localData = Project.LoadFile(Adapter.Prefix, Name + Adapter.Postfix);

            if (ByteArrayEqual(localData, objData))
                return;

            Push(localData);

            objData = Pull();
            objHash = Common.CalculateMD5Hash(objData);
            Project.StoreFile(Adapter.Prefix, Name + Adapter.Postfix + ".md5", objHash);
        }

        private bool ByteArrayEqual(byte[] b1, byte[] b2)
        {
            if (b1 == null || b2 == null || b1.Length != b2.Length)
                return false;

            for (int i = 0; i < b1.Length; i++)
            {
                if (b1[i] != b2[i])
                    return false;
            }

            return true;
        }

        public DateTime? RemoteModifyDate { get; set; }
    }
}
