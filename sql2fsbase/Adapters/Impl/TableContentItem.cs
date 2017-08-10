using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace sql2fsbase.Adapters.Impl
{
    public class TableContentItem : AdaptedItem
    {
        private SqlConnection connection;
        private TableContent.TableConfig config;
        private String filter;
        private String tableName;

        private Encoding _enc = Encoding.Unicode;

        public TableContentItem(AdapterBase adapter, String name, ProjectDirectory project, SqlConnection connection, TableContent.TableConfig config, String filter, String tableName)
            : base(adapter, name, project)
        {
            this.tableName = tableName;
            this.config = config;
            this.filter = filter;
            this.connection = connection;
        }

        public override void Push(byte[] data)
        {
            TableContent.TableData fileData = DeserializeTable(Common.ConvertFrom(data, _enc));
            TableContent.TableData DBData = PullTable();

            List<String> sql = new List<string>();

            GenerateMergeScript(sql, DBData, fileData);

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

        public enum RowState
        {
            DBOnly,
            FileOnly,
            Both
        }

        private void GenerateMergeScript(List<String> sql, TableContent.TableData DBData, TableContent.TableData fileData)
        {
            Dictionary<String, TableContent.TableRowLocal> DBRows = DBData.BuildRowDict();
            Dictionary<String, TableContent.TableRowLocal> fileRows = fileData.BuildRowDict();

            Dictionary<String, RowState> allKeys = new Dictionary<String, RowState>();

            foreach (var row in fileRows)
                allKeys.Add(row.Key, RowState.FileOnly);

            foreach (var row in DBRows)
            {
                if (allKeys.ContainsKey(row.Key))
                    allKeys[row.Key] = RowState.Both;
                else
                    allKeys.Add(row.Key, RowState.DBOnly);
            }

            foreach (var allKey in allKeys)
            {
                String key = allKey.Key;

                if (allKey.Value == RowState.FileOnly)
                {
                    // insert row

                    TableContent.TableRow row = fileRows[allKey.Key].Row;
                    TableContent.TableData data = fileRows[allKey.Key].Data;
                    String tableName = data.TableName;

                    String f = "";
                    String v = "";
                    bool isFirst = true;

                    foreach (var field in row.Data)
                    {
                        f += (isFirst ? "" : ", ") + field.Key;
                        v += (isFirst ? "" : ", ") + formatSqlField(field.Value);
                        isFirst = false;
                    }
                    String q = String.Format("insert into {0} ({1}) values ({2})", tableName, f, v);

                    sql.Add(q);
                }
                else if (allKey.Value == RowState.DBOnly)
                {
                    // delete row

                    TableContent.TableRow row = DBRows[allKey.Key].Row;
                    TableContent.TableData data = DBRows[allKey.Key].Data;
                    String tableName = data.TableName;
                    String primaryKey = data.PrimaryKey;

                    String q = String.Format(@"delete from {0} where {1} = {2}", tableName, primaryKey,
                        formatSqlField(row.Data[primaryKey]));

                    sql.Add(q);
                }
                else if (!DBRows[allKey.Key].Row.DataEquals(fileRows[allKey.Key].Row))
                {
                    // update row

                    TableContent.TableRow row = fileRows[allKey.Key].Row;
                    TableContent.TableData data = fileRows[allKey.Key].Data;
                    String tableName = data.TableName;
                    String primaryKey = data.PrimaryKey;

                    StringBuilder q = new StringBuilder();
                    q.AppendFormat("update {0} ", tableName);
                    bool isFirst = true;
                    String cond = null;

                    foreach (var field in row.Data)
                    {
                        if (field.Key.Equals(primaryKey))
                        {
                            cond = String.Format(" where {0} = {1} ", primaryKey, formatSqlField(field.Value));
                        }
                        else if (!fileData.FieldsInsertOnly.Contains<String>(field.Key))
                        {
                            q.AppendFormat(" {0} {1} = {2} ", (isFirst ? "set" : ","), field.Key,
                            formatSqlField(field.Value));
                            isFirst = false;
                        }
                    }

                    q.AppendFormat(cond);

                    sql.Add(q.ToString());
                }
            }
        }
        
        public TableContent.TableData PullTable()
        {
            String commaFields = "";
            String[] allFields = config.GetAllFields();
            foreach (var f in allFields)
            {
                commaFields += String.Format("{0} case when {1} is null then '[null]' else cast({1} as nvarchar(MAX)) end as {1}", commaFields.Length == 0 ? "" : ",", f);
            }

            String fullFilter = "";

            if (config.Filter.Length > 0)
            {
                fullFilter += " where ";
                fullFilter += config.Filter;
            }

            if (this.filter.Length > 0)
            {
                if (fullFilter.Length > 0)
                    fullFilter += " and ";
                else
                    fullFilter += " where ";
                                
                fullFilter += String.Format("{0} = '{1}'", config.SplitCond, this.filter);
            }
            
            List<TableContent.TableRow> rows = new List<TableContent.TableRow>();
            String sql = String.Format("Select {1} From {0} {3} order by {2}", tableName, commaFields, config.PrimaryKey, fullFilter);

            SqlCommand cmd = new SqlCommand(sql, connection);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    TableContent.TableRow row = new TableContent.TableRow();
                    row.Data = new TableContent.TableRowData();

                    for (int i = 0; i < dr.FieldCount; i++)
                        row.Data.Add(allFields[i], dr.GetValue(i).ToString());

                    rows.Add(row);
                }
            }

            foreach (TableContent.TableRow row in rows)
            {
                loadSubtables(row, config);
            }

            TableContent.TableData data = new TableContent.TableData()
            {
                Rows = rows.ToArray(),
                TableName = config.Name,
                PrimaryKey = config.PrimaryKey,
                FieldsInsertOnly = config.FieldsInsertOnly
            };

            return data;
        }

        private void loadSubtables(TableContent.TableRow parentRow, TableContent.TableConfig rowConfig)
        {
            TableContent.SubTableConfig[] subConf = rowConfig.SubTables;
            if (subConf == null || subConf.Length == 0)
                return;

            List<TableContent.TableData> subDataList = new List<TableContent.TableData>(subConf.Length);

            foreach (TableContent.SubTableConfig _conf in subConf)
            {
                String commaFields = "";
                String[] allFields = _conf.GetAllFields();
                foreach (var f in allFields)
                {
                    commaFields += String.Format("{0} case when {1} is null then '[null]' else cast({1} as nvarchar(MAX)) end as {1}", commaFields.Length == 0 ? "" : ",", f);
                }

                String fullFilter = "";
                if (_conf.Filter.Length > 0)
                {
                    fullFilter += " where ";
                    fullFilter += _conf.Filter;
                }

                if (fullFilter.Length > 0)
                        fullFilter += " and ";
                    else
                        fullFilter += " where ";

                if (!parentRow.Data.ContainsKey(rowConfig.PrimaryKey))
                    throw new Exception("В базовой таблице не найдёно ключевое поле для зависимой");

                fullFilter += String.Format("{0} = '{1}'", _conf.ParentCond, parentRow.Data[rowConfig.PrimaryKey]);

                List<TableContent.TableRow> rows = new List<TableContent.TableRow>();
                String sql = String.Format("Select {1} From {0} {3} order by {2}", _conf.Name, commaFields, _conf.PrimaryKey, fullFilter);

                SqlCommand cmd = new SqlCommand(sql, connection);
                using (SqlDataReader dr = cmd.ExecuteReader())
                {
                    while (dr.Read())
                    {
                        TableContent.TableRow row = new TableContent.TableRow();
                        row.Data = new TableContent.TableRowData();

                        for (int i = 0; i < dr.FieldCount; i++)
                            row.Data.Add(allFields[i], dr.GetValue(i).ToString());

                        rows.Add(row);
                    }
                }

                subDataList.Add(new TableContent.TableData()
                {
                    Rows = rows.ToArray<TableContent.TableRow>(),
                    TableName = _conf.Name,
                    PrimaryKey = _conf.PrimaryKey,
                    FieldsInsertOnly = _conf.FieldsInsertOnly
                }
                );

                foreach (TableContent.TableRow row in rows)
                {
                    loadSubtables(row, _conf);
                }
            }

            parentRow.SubTableData = subDataList.ToArray<TableContent.TableData>();
        }

        public override byte[] Pull()
        {
            return Common.ConvertTo(SerializeTable(PullTable()), _enc);
        }

        private String SerializeTable(TableContent.TableData table)
        {
            String data = Common.Serialize<TableContent.TableData>(table);
            
            return data;
        }

        private TableContent.TableData DeserializeTable(String data)
        {
            if (data == null)
                return new TableContent.TableData() { Rows = new TableContent.TableRow[0] };

            using (TextReader reader = new StringReader(data))
            {
                return Common.Deserialize<TableContent.TableData>(reader.ReadToEnd());
            }
        }

        private String formatSqlField(String value)
        {
            if (value == "[null]")
                return "null";

            return String.Format("N'{0}'", value.Replace("'", "''"));
        }
    }
}
