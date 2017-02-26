using sql2fsbase.Adapters.Impl.DBContent;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PsmProc
{
    class TableRowComparer : ITableRowComparer
    {
        public UserAction CheckRow(String[] fields, String[] before, String[] after, String state)
        {
            return UserAction.Replace;
        }
    }
}
