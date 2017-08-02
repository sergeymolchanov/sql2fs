using System;
using System.Data.SqlClient;
using System.IO;
using System.Text;

using sql2fsbase.Adapters.Impl;
using System.Collections.Generic;

namespace sql2fsbase.Adapters.Impl
{
    public class TableContentAdapter : AdapterBaseSQL
    {
        private TableContent.TableConfig[] Config = new TableContent.TableConfig[0];

        public TableContentAdapter(ProjectDirectory project, TableContent.ISqlErrorView sqlErrorView)
            : base(project, sqlErrorView)
        {
            byte[] fileData = Project.LoadFile(Prefix, "tables.xml");

            if (fileData == null)
            {
                BuildTablesXml();

                fileData = Project.LoadFile(Prefix, "tables.xml");

                if (fileData == null)
                    return;
            }

            String tableList = Encoding.GetEncoding(1251).GetString(fileData);

            Config = Common.Deserialize<TableContent.ContentConfig>(tableList).Tables;
        }

        private void BuildTablesXml()
        {
            // Использует старый tables.conf для создания tables.xml

            byte[] fileData = Project.LoadFile(Prefix, "tables.conf");

            if (fileData == null)
                return;

            String tableList = Encoding.GetEncoding(1251).GetString(fileData);

            List<TableContent.TableConfig> _conf = new List<TableContent.TableConfig>();

            foreach (String line in tableList.Split('\n'))
            {
                String name;

                List<String> fields = new List<string>();
                List<String> fieldsInsertOnly = new List<string>();

                if (line.Length < 5)
                    continue;

                String[] t = line.Replace("\r", "").Split(':');
                name = t[0];

                String pk = null;

                foreach (String f in t[1].Split(','))
                {
                    if (pk == null)
                        pk = f.Trim();
                    else if (f.Trim().StartsWith("+"))
                        fieldsInsertOnly.Add(f.Trim());
                    else
                        fields.Add(f.Trim());
                }

                _conf.Add(new TableContent.TableConfig()
                {
                    Name = name,
                    PrimaryKey = pk,
                    Fields = fields.ToArray(),
                    FieldsInsertOnly = fieldsInsertOnly.ToArray(),
                    SplitCond = name == "_localstr" ? "prog" : ""
                });
            }

            TableContent.ContentConfig cc = new TableContent.ContentConfig() { Tables = _conf.ToArray() };
            String serializedConfig = Common.Serialize<TableContent.ContentConfig>(cc);

            Project.StoreFile(Prefix, "tables.xml", Encoding.GetEncoding(1251).GetBytes(serializedConfig));
        }

        public override void AddItem(string name)
        {
            TableContent.TableConfig config = null;

            String[] nameArray = name.Split('-');
            String tableName = nameArray[0];
            String splitCond = (nameArray.Length > 1) ? nameArray[1] : "";

            foreach (TableContent.TableConfig tblConfig in Config)
            {
                if (tblConfig.Name.ToLower().Equals(tableName.ToLower()))
                {
                    config = tblConfig;
                    break;
                }
            }

            if (config == null)
                return;

            Items.Add(new TableContentItem(this, name, Project, Connection, config, splitCond, tableName));
        }

        public override String Postfix { get { return ".xml"; } }
        public override String Prefix { get { return "Content"; } }

        public override void LoadFromRemote()
        {
            foreach (TableContent.TableConfig config in Config)
            {
                if (config.SplitCond == null || config.SplitCond.Length == 0)
                {
                    AddItem(config.Name);
                }
                else
                {
                    String filter = config.Filter != null && config.Filter.Length > 0 ? "where " + config.Filter : "";

                    SqlCommand cmd = new SqlCommand(String.Format("Select distinct {1} From {0} {2} order by {1}", config.Name, config.SplitCond, filter), Connection);
                    using (SqlDataReader dr = cmd.ExecuteReader())
                    {
                        while (dr.Read())
                        {
                            String filterCond = dr.GetValue(0).ToString();

                            AddItem(config.Name + "-" + filterCond);
                        }
                    }
                }
            }
        }

        public override void LoadFromLocal()
        {
            foreach (var item in Items)
            {
                item.IsExistsLocal = true;
            }
        }
    }
}