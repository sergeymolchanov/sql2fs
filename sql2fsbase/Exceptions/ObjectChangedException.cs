using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using sql2fsbase.Adapters;

namespace sql2fsbase.Exceptions
{
    public class ObjectChangedException : Exception
    {
        public AdaptedItem Item { get; private set; }

        public ObjectChangedException(AdaptedItem item)
        {
            Item = item;
        }
    }
}
