using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSourceManager.Adapters.Impl
{
    public class StoredProcItem : AdaptedItem
    {
        private const String QueryString = "SELECT OBJECT_DEFINITION (OBJECT_ID(N'{0}'))";
        public String ObjectName { get; private set; }
        public String ObjectType { get; private set; }

        private static readonly Encoding _enc = Encoding.Unicode;

        private SqlConnection Connection;

        public StoredProcItem(AdapterBase adapter, String name, ProjectDirectory project, SqlConnection connection)
            : base(adapter, name, project)
        {
            Connection = connection;
            String[] val = name.Split('.');
            ObjectType = val[0];
            ObjectName = name.Substring(ObjectType.Length+1);
        }

        public override void Push(byte[] data)
        {
            int i = 0;
            int idx = -1;
            bool isComment = false;
            bool isMultilineComment = false;

            String dataStr = Common.ConvertFrom(data, _enc);

            String t = dataStr.ToLower();
            while (i < t.Length)
            {
                if (i < t.Length - 1)
                {
                    if (t[i] == '-' && t[i + 1] == '-')
                        isComment = true;
                    if (t[i] == '/' && t[i + 1] == '*')
                        isMultilineComment = true;
                    if (t[i] == '*' && t[i + 1] == '/')
                        isMultilineComment = false;
                }
                if (t[i] == '\r' || t[i] == '\n')
                    isComment = false;
                if (!isComment && !isMultilineComment && t[i] == 'c' && t.Substring(i).StartsWith("create"))
                {
                    idx = i;
                    break;
                }
                i++;
            }

            if (idx < 0)
                throw new Exception("Для объекта БД " + ObjectName + " невозможно заменить CREATE на ALTER");

            String sql = dataStr.Substring(0, idx) + "ALTER" + dataStr.Substring(idx + 6);

            try
            {
                SqlCommand cmd = new SqlCommand(sql, Connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                if (ex.Message.StartsWith("Invalid obje"))
                {
                    SqlCommand cmd = new SqlCommand(dataStr, Connection);
                    cmd.ExecuteNonQuery();
                }
                else
                {
                    throw;
                }
            }
        }

        public override byte[] Pull()
        {
            SqlCommand cmd = new SqlCommand(String.Format(QueryString, ObjectName), Connection);
            Object data = cmd.ExecuteScalar();

            if (data == null || data is DBNull)
                return null;

            String dataStr = (string) data;

            return Common.ConvertTo(dataStr, _enc);
        }
    }
}
