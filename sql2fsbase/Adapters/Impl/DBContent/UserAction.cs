using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql2fsbase.Adapters.Impl.DBContent
{
    public enum UserAction
    {
        Replace,
        ReplaceAll,
        Skip,
        Cancel,
        SkipAll
    }
}
