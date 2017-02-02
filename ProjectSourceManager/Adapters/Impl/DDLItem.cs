using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSourceManager.Adapters.Impl
{
    public class DDLItem : AdaptedItem
    {
        private const String QueryString = "select CommandText from _DDL_Log where RowPointer = '{0}'";

        public DateTime ActionDateTime { get; private set; }
        public String ActionPtr { get; private set; }

        private static readonly Encoding _enc = Encoding.Unicode;

        private SqlConnection Connection;

        public DDLItem(AdapterBase adapter, String name, ProjectDirectory project, SqlConnection connection)
            : base(adapter, name, project)
        {
            Connection = connection;
            String[] val = name.Split('_');
            ActionDateTime = DateTime.ParseExact(val[0], "yyyyMMddHHmmss", CultureInfo.InvariantCulture);
            ActionPtr = val[1];
        }

        public override void Push(byte[] data)
        {
            /*
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
            }*/
        }

        public override byte[] Pull()
        {
            SqlCommand cmd = new SqlCommand(String.Format(QueryString, ActionPtr), Connection);
            Object data = cmd.ExecuteScalar();

            if (data == null || data is DBNull)
                return null;

            String dataStr = (string) data;

            return Common.ConvertTo(dataStr, _enc);
        }
    }
}
