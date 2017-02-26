using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;
using sql2fsbase.DBContent;
using sql2fsbase.Adapters.Impl.DBContent;

namespace sql2fsbase.Adapters.Impl
{
    public class TableContentItem : AdaptedItem
    {
        private static readonly XmlSerializer serializer = new XmlSerializer(typeof(TableData));

        public String[] Fields { get; private set; }
        private SqlConnection connection;

        private Encoding _enc = Encoding.Unicode;

        public TableContentItem(AdapterBase adapter, String name, ProjectDirectory project, SqlConnection connection, ITableRowComparer tableRowComparer)
            : base(adapter, name, project)
        {
            TableRowComparerInstance = tableRowComparer;

            String[] parts = name.Split(':');
            String[] fields = parts[1].Split(',');

            for (int i = 0; i < fields.Length; i++)
            {
                fields[i] = fields[i].Trim();
            }

            Name = parts[0];
            Fields = fields;
            this.connection = connection;
        }

        private ITableRowComparer TableRowComparerInstance;

        public override void Push(byte[] data)
        {
            TableData fileData = DeserializeTable(Common.ConvertFrom(data, _enc));
            TableData DBData = PullTable();

            Dictionary<String, TableRow> srcRows = new Dictionary<string, TableRow>();
            foreach (var row in fileData.Rows)
                srcRows.Add(row.Value[0], row);

            List<String> sql = new List<string>();

            bool IsReplaceAll = false;
            for (int i = 0; i < DBData.Rows.Length; i++)
            {
                String key = DBData.Rows[i].Value[0];
                if (srcRows.ContainsKey(key))
                {
                    // Check valid
                    if (!IsArrayEq(DBData.Rows[i].Value, srcRows[key].Value))
                    {
                        StringBuilder q = new StringBuilder();
                        q.AppendFormat("update {0} ", Name);
                        for (int j = 1; j < DBData.Fields.Length; j++)
                        {
                            q.AppendFormat(" {0} {1} = {2} ", (j == 1 ? "set" : ","), DBData.Fields[j],
                                formatSqlField(srcRows[key].Value[j]));
                        }
                        q.AppendFormat(" where {0} = {1} ", DBData.Fields[0], formatSqlField(srcRows[key].Value[0]));

                        UserAction act = IsReplaceAll? UserAction.Replace : TableRowComparerInstance.CheckRow(DBData.Fields, DBData.Rows[i].Value, srcRows[key].Value,
                                "Изменение");

                        if (act == UserAction.ReplaceAll)
                        {
                            act = UserAction.Replace;
                            IsReplaceAll = true;
                        }

                        if (act == UserAction.Replace)
                        {
                            sql.Add(q.ToString());
                        }
                        else if (act == UserAction.Cancel)
                        {
                            return;
                        }
                    }

                    srcRows.Remove(key);
                }
                else
                {
                    // Del

                    String q = String.Format(@"delete from {0} where {1} = {2}", Name, DBData.Fields[0],
                        formatSqlField(DBData.Rows[i].Value[0]));

                    UserAction act = IsReplaceAll ? UserAction.Replace : TableRowComparerInstance.CheckRow(DBData.Fields, DBData.Rows[i].Value, null, "Удаление");

                    if (act == UserAction.ReplaceAll)
                    {
                        act = UserAction.Replace;
                        IsReplaceAll = true;
                    }

                    if (act == UserAction.Replace)
                    {
                        sql.Add(q);
                    }
                    else if (act == UserAction.Cancel)
                    {
                        return;
                    }
                }
            }

            bool skipAll = false;
            foreach (TableRow row in srcRows.Values)
            {
                // ins

                String f = "";
                String v = "";

                for (int j = 0; j < DBData.Fields.Length; j++)
                {
                    f += (j == 0 ? "" : ", ") + DBData.Fields[j];
                    v += (j == 0 ? "" : ", ") + formatSqlField(row.Value[j]);
                }
                String q = String.Format("insert into {0} ({1}) values ({2})", Name, f, v);

                UserAction act = IsReplaceAll ? UserAction.Replace : TableRowComparerInstance.CheckRow(DBData.Fields, null, row.Value, "Добавление");

                if (act == UserAction.ReplaceAll)
                {
                    act = UserAction.Replace;
                    IsReplaceAll = true;
                }

                if (act == UserAction.Replace)
                {
                    sql.Add(q);
                }
                else if (act == UserAction.SkipAll)
                {
                    skipAll = true;
                }
                else if (act == UserAction.Cancel)
                {
                    return;
                }
            }

            foreach (var q in sql)
            {
                try
                {
                    new SqlCommand(q, connection).ExecuteNonQuery();
                }
                catch (Exception e)
                {
                    ((AdapterBaseSQL)this.Adapter).AddError(q, e);
                }
            }
        }

        public TableData PullTable()
        {
            String commaFields = "";
            foreach (var f in Fields)
            {
                commaFields += String.Format("{0} case when {1} is null then '[null]' else cast({1} as nvarchar(MAX)) end as {1}", commaFields.Length == 0 ? "" : ",", f);
            }

            List<TableRow> rows = new List<TableRow>();
            SqlCommand cmd = new SqlCommand(String.Format("Select {1} From {0} order by {2}", Name, commaFields, Fields[0]), connection);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    TableRow row = new TableRow();
                    row.Value = new string[dr.FieldCount];

                    for (int i = 0; i < dr.FieldCount; i++)
                        row.Value[i] = dr.GetValue(i).ToString();
                    rows.Add(row);
                }
            }

            TableData data = new TableData();
            data.Rows = rows.ToArray();
            data.Fields = Fields;

            return data;
        }

        public override byte[] Pull()
        {
            return Common.ConvertTo(SerializeTable(PullTable()), _enc);
        }

        private String SerializeTable(TableData table)
        {
            String data = Common.Serialize<TableData>(table);

            data = data.Replace(@"
<T />
", @"<T></T>");
            data = data.Replace(@"<T />
", @"<T></T>");
            data = data.Replace(@"
<T />", @"<T></T>");

            data = data.Replace(@"<R>
      <Value>", @"<R><Value>");
            data = data.Replace(@"<Value>
        <T>", @"<Value><T>");
            data = data.Replace(@"</T>
      </Value>", @"</T></Value>");
            data = data.Replace(@"</T>
        <T>", @"</T><T>");
            data = data.Replace(@"</Value>
    </R>", @"</Value></R>");

            return data;
        }

        private TableData DeserializeTable(String data)
        {
            using (TextReader reader = new StringReader(data))
            {
                return (TableData)serializer.Deserialize(reader);
            }
        }

        private bool IsArrayEq(String[] a1, String[] a2)
        {
            if (a1.Length != a2.Length)
                return false;

            for (int i = 0; i < a1.Length; i++)
                if (!a1[i].Trim().Equals(a2[i].Trim()))
                    return false;

            return true;
        }

        private String formatSqlField(String value)
        {
            if (value == "[null]")
                return "null";

            return String.Format("N'{0}'", value.Replace("'", "''"));
        }
    }
}
