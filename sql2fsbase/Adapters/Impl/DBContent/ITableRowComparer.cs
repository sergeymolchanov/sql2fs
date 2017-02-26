using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql2fsbase.Adapters.Impl.DBContent
{
    public interface ITableRowComparer
    {
        UserAction CheckRow(String[] fields, String[] before, String[] after, String state);
    }
}
