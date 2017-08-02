using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql2fsbase
{
    public interface IDiffTool
    {
        byte[] MergeFiles(byte[] dbData, byte[] vkData, byte[] baseData);
    }
}
