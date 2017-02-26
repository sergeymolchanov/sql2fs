using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql2fsbase
{
    public static class LongOperationState
    {
        public static int Timer1Max { get; set; }
        public static int Timer1Pos { get; set; }
        public static String Timer1Text { get; set; }

        public static int Timer2Max { get; set; }
        public static int Timer2Pos { get; set; }
        public static String Timer2Text { get; set; }
    }
}
