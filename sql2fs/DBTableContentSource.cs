using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Data.SqlClient;

namespace sql2fs
{
    public class DBTableContentSource
    {
        private SqlConnection connection = null;

        protected SqlConnection Connection
        {
            get
            {
                if (connection.State != ConnectionState.Open)
                    connection.Open();
                return connection;
            }
        }

        public DBTableContentSource(SqlConnection connection)
        {
            this.connection = connection;
        }
        
        private const String NL_REPLACE = "[E06A50AC9EEA]";
        private const String DL_REPLACE = "[7DE7893B0E1C]";
        private const Char NL ='\n';
        private const Char DL = ';';

        public String Dump(String name, String[] onlyFields)
        {
            StringBuilder ret = new StringBuilder();

            String onlyFieldsList = "";
            if (onlyFields != null)
            {
                foreach (var f in onlyFields)
                {
                    if (f.Length > 0)
                        onlyFieldsList += String.Format("{0} case when {1} is null then '[null]' else cast({1} as nvarchar(MAX)) end as {1}", onlyFieldsList.Length == 0 ? "" : ",",  f);
                }
            }

            SqlCommand cmd = new SqlCommand(String.Format("Select {1} From {0} order by 1", name, onlyFieldsList.Length > 0 ? onlyFieldsList : "*"), Connection);
            using (SqlDataReader dr = cmd.ExecuteReader(CommandBehavior.CloseConnection))
            {
                for (int i = 0; i < dr.FieldCount; i++)
                    ret.AppendFormat("{1}{0}", dr.GetName(i).ToString().Trim(), i==0?"":";");
                ret.Append(NL);

                while (dr.Read())
                {
                    for (int i = 0; i < dr.FieldCount; i++)
                        ret.AppendFormat("{1}{0}", Encode(dr.GetValue(i).ToString()), i==0?"":";");
                    ret.Append(NL);
                }
            }

            return ret.ToString();
        }

        public void Restore(String name, String data, String[] onlyFields)
        {
            String[] srcText = data.Split(NL);
            String[] destText = Dump(name, onlyFields).Split(NL);
            List<String> cmdAdd = new List<string>();
            List<String> cmdDel = new List<string>();
            List<String> cmdUpd = new List<string>();

            if (!srcText[0].Equals(destText[0]))
                throw new Exception("Invalid fields");

            String[] fields = srcText[0].Split(DL);

            Dictionary<String, String[]> src = new Dictionary<string, string[]>();
            Dictionary<String, String[]> dest = new Dictionary<string, string[]>();
            HashSet<String> keys = new HashSet<string>();

            for (int i = 1; i < srcText.Length; i++)
            {
                String[] values = srcText[i].Split(DL);
                src.Add(values[0], values);
                keys.Add(values[0]);
            }

            for (int i = 1; i < destText.Length; i++)
            {
                String[] values = destText[i].Split(DL);
                dest.Add(values[0], values);
                if (!keys.Contains(values[0]))
                    keys.Add(values[0]);
            }

            foreach (String key in keys)
            {
                if (dest.ContainsKey(key) && src.ContainsKey(key))
                {
                    if (!IsArrayEq(dest[key],src[key]))
                    {
                        StringBuilder q = new StringBuilder(); 
                        q.AppendFormat("update {0} ", name);
                        for (int i = 1; i < fields.Length; i++)
                        {
                            q.AppendFormat(" {0} {1} = {2} ", (i == 1 ? "set" : ","), fields[i], formatSqlField(src[key][i]));
                        }
                        q.AppendFormat(" where {0} = {1} ", fields[0], formatSqlField(src[key][0]));
                        cmdUpd.Add(q.ToString());
                    }
                }
                else if (dest.ContainsKey(key))
                {
                    String q = String.Format(@"delete from {0} where {1} = {2}", name, fields[0],
                        formatSqlField(dest[key][0]));
                    cmdDel.Add(q);
                }
                else if (src.ContainsKey(key))
                {
                    String f = "";
                    String v = "";

                    for (int i = 0; i < fields.Length; i++)
                    {
                        f += (i == 0? "" : ", ") + fields[i];
                        v += (i == 0? "" : ", ") + formatSqlField(src[key][i]);
                    }
                    String q = String.Format("insert into {0} ({1}) values ({2})", name, f, v);
                    cmdAdd.Add(q);
                }
                else
                {
                    throw new Exception("Impossible");
                }
            }

            foreach (var cmd in cmdDel)
            {
                new SqlCommand(cmd, Connection).ExecuteNonQuery();
            }

            foreach (var cmd in cmdUpd)
            {
                new SqlCommand(cmd, Connection).ExecuteNonQuery();
            }

            foreach (var cmd in cmdAdd)
            {
                new SqlCommand(cmd, Connection).ExecuteNonQuery();
            }
        }

        private String formatSqlField(String value)
        {
            if (value == "[null]")
                return "null";

            return String.Format("N'{0}'", value.Replace("'", "''"));
        }

        private bool IsArrayEq(String[] a1, String[] a2)
        {
            if (a1.Length != a2.Length)
                return false;
            
            for(int i=0; i<a1.Length; i++)
                if (!a1[i].Equals(a2[i]))
                    return false;

            return true;
        }

        public String Encode(String value)
        {
            String ret = value;
            ret = value.Replace(NL.ToString(), NL_REPLACE);
            ret = value.Replace(DL.ToString(), DL_REPLACE);

            return ret;
        }

        public String Decode(String value)
        {
            String ret = value;
            ret = ret.Replace(NL_REPLACE, NL.ToString());
            ret = ret.Replace(DL_REPLACE, DL.ToString());

            return ret;
        }
    }
}