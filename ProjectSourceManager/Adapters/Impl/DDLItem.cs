using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectSourceManager.Adapters.Impl
{
    public class DDLItem : AdaptedItem, IComparable
    {
        private const String QueryString = "select CommandText from DDL_Log where RowKey = '{0}'";

        public DateTime ActionDateTime { get; private set; }

        private static readonly Encoding _enc = Encoding.Unicode;

        private SqlConnection Connection;

        public DDLItem(AdapterBase adapter, String name, ProjectDirectory project, SqlConnection connection)
            : base(adapter, name, project)
        {
            Connection = connection;
            String[] val = name.Split('_'); // 2017-02-07T14:57:33
            ActionDateTime = DateTime.ParseExact(val[0].Replace('T', ' '), "yyyy-MM-dd HH-mm-ss", CultureInfo.InvariantCulture);
        }

        public override void Push(byte[] data)
        {
            if (this.IsExistsRemote)
                return;

            String dataStr = Common.ConvertFrom(data, _enc);

            SqlCommand cmd = new SqlCommand("Push_SQL", Connection);
            cmd.CommandType = System.Data.CommandType.StoredProcedure;
            cmd.Parameters.AddWithValue("@originalKey", Name);
            cmd.Parameters.AddWithValue("@originalTime", ActionDateTime);
            cmd.Parameters.AddWithValue("@sql", dataStr);
            cmd.ExecuteNonQuery();
        }

        public override byte[] Pull()
        {
            SqlCommand cmd = new SqlCommand(String.Format(QueryString, Name), Connection);
            Object data = cmd.ExecuteScalar();

            if (data == null || data is DBNull)
                return null;

            String dataStr = (string) data;

            return Common.ConvertTo(dataStr, _enc);
        }

        public int CompareTo(Object item)
        {
            return this.ActionDateTime.CompareTo(((DDLItem)item).ActionDateTime);
        }
    }
}
