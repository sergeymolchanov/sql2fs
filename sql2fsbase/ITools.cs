using sql2fsbase.Adapters.Impl;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql2fsbase
{
    public interface ITools
    {
        byte[] MergeFiles(byte[] dbData, byte[] vkData, byte[] baseData);

        bool ShowSQL(List<AdapterBaseSQL.AdapterSqlException> queryList);

        Common.MergeStyle AskHowToMerge();
    }
}
