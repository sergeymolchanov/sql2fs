using sql2fsbase.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sql2fsbase.Adapters.Impl;

namespace sql2fsbase.Exceptions
{
    public class SyncErrorsException : Exception
    {
        public List<AdapterBaseSQL.AdapterSqlException> ErrorList { get; private set; }

        public SyncErrorsException(List<AdapterBaseSQL.AdapterSqlException> errorList)
            : base ("Есть неисправимые ошибки. Невозможно обновить данные.")
        {
            ErrorList = errorList;
        }
    }
}
