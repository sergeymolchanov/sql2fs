using sql2fsbase.Adapters.Impl;
using sql2fsbase.Adapters.Impl.DBContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsmProc
{
    class SqlErrorView : ISqlErrorView
    {
        public bool ShowSQL(List<AdapterBaseSQL.AdapterSqlException> queryList)
        {
            return true;
        }
    }
}
