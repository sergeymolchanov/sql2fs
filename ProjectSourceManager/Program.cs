using ProjectSourceManager;
using ProjectSourceManager.Adapters.Impl.DBContent;
using sql2fsbase.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace sql2fsbase
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main(String[] param)
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            AdapterManager.SqlErrorViewInstance = new SQLErrorView();
            AdapterManager.DiffTool = new DiffTool();

            Application.Run(new ProjectListForm());
        }
    }
}
