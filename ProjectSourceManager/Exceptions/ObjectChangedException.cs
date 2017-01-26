using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ProjectSourceManager.Adapters;

namespace ProjectSourceManager.Exceptions
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
