using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql2fsbase.Adapters.Impl
{
    public class StoredProcItem : AdaptedItem
    {
        private const String QueryString = "SELECT OBJECT_DEFINITION (OBJECT_ID(N'{0}'))";
        public String ObjectName { get; private set; }
        public String ObjectType { get; private set; }
        public String ObjectTypeFull
        {
            get
            {
                switch (ObjectType)
                {
                    case("V "):
                        return "VIEW";
                    case("V"):
                        return "VIEW";
                    case("P "):
                        return "PROCEDURE";
                    case("P"):
                        return "PROCEDURE";
                    case("IF"):
                        return "FUNCTION";
                    case("TF"):
                        return "FUNCTION";
                    case("FN"):
                        return "FUNCTION";
                    case("TR"):
                        return "TRIGGER";
                    default:
                        throw new Exception("Unknown object type: " + ObjectType);
                }
            }
        }

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
            String dataStr = Common.ConvertFrom(data, _enc);

            if (dataStr == null)
            {
                String sql = String.Format("DROP {0} {1}", ObjectTypeFull, ObjectName);
                try
                {
                    SqlCommand cmd = new SqlCommand(sql, Connection);
                    cmd.ExecuteNonQuery();
                }
                catch(Exception ex)
                {
                    ((AdapterBaseSQL)this.Adapter).AddError(sql, ex);
                }
                return;
            }

            int idx = firstWordIndex(dataStr);

            if (idx < 0)
                throw new Exception("Для объекта БД " + ObjectName + " невозможно заменить CREATE на ALTER");

            String sqlText = dataStr.Substring(0, idx) + "ALTER" + dataStr.Substring(idx + 6);

            try
            {
                SqlCommand cmd = new SqlCommand(sqlText, Connection);
                cmd.ExecuteNonQuery();
            }
            catch (Exception ex)
            {
                try
                {
                    SqlCommand cmd = new SqlCommand(dataStr, Connection);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    ((AdapterBaseSQL)this.Adapter).AddError(sqlText, e);
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
            dataStr = doUpperFirstLine(dataStr);

            return Common.ConvertTo(dataStr, _enc);
        }

        private String doUpperFirstLine(String data)
        {
            int idxFrom = firstWordIndex(data);
            if (idxFrom < 0)
                return data;
            int idxTo1 = data.IndexOf(' ', idxFrom + 9);
            int idxTo2 = data.IndexOf('\t', idxFrom + 9);
            int idxTo;

            if (idxTo1 > 0 && idxTo2 > 0)
                idxTo = Math.Min(idxTo1, idxTo2);
            else if (idxTo1 > 0)
                idxTo = idxTo1;
            else if (idxTo2 > 0)
                idxTo = idxTo2;
            else
                return data;

            String ret = data.Substring(0, idxFrom) + data.Substring(idxFrom, idxTo-idxFrom).ToUpper() + data.Substring(idxTo);
            return ret;
        }

        private int firstWordIndex(String dataStr)
        {
            int i = 0;
            bool isComment = false;
            bool isMultilineComment = false;
            int idx = -1;

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

            return idx;
        }
    }
}
