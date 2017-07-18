using sql2fsbase.Exceptions;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace sql2fsbase.Adapters.Impl
{
    public abstract class AdapterBaseSQL : AdapterBase
    {
        public AdapterBaseSQL(ProjectDirectory project, TableContent.ISqlErrorView sqlErrorView)
            : base(project)
        {
            SqlErrorViewInstance = sqlErrorView;
        }

        private TableContent.ISqlErrorView SqlErrorViewInstance;

        private SqlConnection _connection = null;
        protected SqlConnection Connection
        {
            get
            {
                if (_connection == null)
                {
                    _connection = new SqlConnection(Project.Settings.ConnectionString);
                    _connection.Open();
                    new SqlCommand("set dateformat 'mdy'", _connection).ExecuteNonQuery();
                    new SqlCommand("SET LANGUAGE Russian", _connection).ExecuteNonQuery();
                }
                return _connection;
            }
        }

        private List<AdapterSqlException> _errorSQL = new List<AdapterSqlException>();
        public void AddError(AdapterSqlException exception)
        {
            _errorSQL.Add(exception);
        }
        public void AddError(String sql, Exception exception)
        {
            _errorSQL.Add(new AdapterSqlException(sql, exception));
        }

        protected override void processErrors()
        {
            while (true)
            {
                int PrevErrorCount = _errorSQL.Count;
                if (PrevErrorCount == 0)
                    return;

                List<AdapterSqlException> sql = _errorSQL;
                _errorSQL = new List<AdapterSqlException>();

                foreach (var q in sql)
                {
                    try
                    {
                        new SqlCommand(q.Sql, Connection).ExecuteNonQuery();
                    }
                    catch (Exception e)
                    {
                        _errorSQL.Add(new AdapterSqlException(q.Sql, e));
                    }
                }

                if (_errorSQL.Count == PrevErrorCount)
                {
                    bool doCancel = SqlErrorViewInstance.ShowSQL(_errorSQL);

                    if (doCancel)
                        throw new SyncErrorsException(_errorSQL);
                }
            }
        }

        public class AdapterSqlException : Exception
        {
            public String Sql { get; private set; }
            public Exception Exception { get; private set; }

            public AdapterSqlException(String sql, Exception exception) : base(exception.Message, exception)
            {
                Sql = sql;
                Exception = exception;
            }
        }
    }
}
