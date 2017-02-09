using System;
using System.Data.SqlClient;

namespace ProjectSourceManager.Adapters.Impl
{
    public class DDLAdapter : AdapterBaseSQL
    {
        private const String QueryString = @"select RowKey, OriginalTime, CommandText from DDL_Log where IsOriginal = 1";

        public DDLAdapter(ProjectDirectory project)
            : base(project)
        {
            SqlCommand cmdInit = new SqlCommand(INIT_SQL, Connection);
            cmdInit.ExecuteNonQuery();
        }
        
        public override void AddItem(string name)
        {
            Items.Add(new DDLItem(this, name, Project, Connection));
        }

        public override void LoadFromRemote()
        {
            SqlCommand cmd = new SqlCommand(QueryString, Connection);
            using (SqlDataReader dr = cmd.ExecuteReader())
            {
                while (dr.Read())
                {
                    AddItem(dr.GetString(0));
                }
            }
        }

        protected override void DoSort()
        {
            Items.Sort();
        }

        public override String Prefix { get { return "DatabaseDDL"; } }
        public override String Postfix { get { return ".sql"; } }

        // TODO: Common.VersionedObjectTypes
        private const String INIT_SQL = @"
IF object_id('DDL_Log') is not null
	return

create table DDL_Log (
	RowKey	nvarchar(1000) NOT NULL,
	EventType nvarchar(100), 
	PostTime datetime, 
	OriginalTime datetime, 
	SPID	int, 
	LoginName sysname, 
	ServerName sysname, 
	DatabaseName	sysname, 
	SchemaName	sysname, 
	ObjectName	sysname, 
	ObjectType	sysname, 
	CommandText	nvarchar(max),
	IsOriginal	int not null)

CREATE UNIQUE CLUSTERED INDEX PK_DDL_Log ON DDL_Log(RowKey ASC)

declare @proc nvarchar(max) = '
create procedure Push_SQL(
	@originalKey nvarchar(1000),
	@originalTime datetime,
	@sql	nvarchar(max))
as
begin
	declare @existsPtr table (ptr nvarchar(1000))
	declare @newKey	nvarchar(1000)

	insert into @existsPtr
	select RowKey from DDL_Log

	execute sp_sqlexec @sql

	select @newKey = RowKey from DDL_Log where RowKey not in (select ptr from @existsPtr)

	update DDL_Log set IsOriginal = 0, OriginalTime = @originalTime, RowKey = @originalKey where RowKey = @newKey
end'

declare @trgsql nvarchar(max) = '
CREATE TRIGGER TR_DDL_Events
ON DATABASE
AFTER DDL_DATABASE_LEVEL_EVENTS
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @data XML
	SET @data = EVENTDATA()

	DECLARE @originalTime datetime = @data.value(''(/EVENT_INSTANCE/PostTime)[1]'',''datetime'')
	DECLARE @eventType sysname = @data.value(''(/EVENT_INSTANCE/EventType)[1]'',''sysname'')
	DECLARE @objectName sysname = @data.value(''(/EVENT_INSTANCE/ObjectName)[1]'',''sysname'')
	DECLARE @objectType sysname = @data.value(''(/EVENT_INSTANCE/ObjectType)[1]'',''sysname'')

    if @objectType in (''TRIGGER'', ''PROCEDURE'', ''FUNCTION'', ''VIEW'')
        return 

	insert into DDL_Log
		(RowKey, EventType, PostTime, OriginalTime, SPID, LoginName, ServerName, DatabaseName, SchemaName, ObjectName, ObjectType, CommandText, IsOriginal)
	select
		RowKey = REPLACE(REPLACE(CONVERT(nvarchar(max),@originalTime,(120)), '' '', ''T''), '':'', ''-'')+''_''+@eventType+''_''+@objectName,
		EventType = @eventType,
		PostTime = @data.value(''(/EVENT_INSTANCE/PostTime)[1]'',''datetime''),
		OriginalTime = @originalTime,
		SPID = @data.value(''(/EVENT_INSTANCE/SPID)[1]'',''int''),
		LoginName = @data.value(''(/EVENT_INSTANCE/LoginName)[1]'',''sysname''),
		ServerName = @data.value(''(/EVENT_INSTANCE/ServerName)[1]'',''sysname''),
		DatabaseName = @data.value(''(/EVENT_INSTANCE/DatabaseName)[1]'',''sysname''),
		SchemaName = @data.value(''(/EVENT_INSTANCE/SchemaName)[1]'',''sysname''),
		ObjectName = @objectName,
		ObjectType = @objectType,
		CommandText = @data.value(''(/EVENT_INSTANCE/TSQLCommand/CommandText)[1]'',''nvarchar(max)''),
		IsOriginal = 1
END'

execute sp_sqlexec @proc
execute sp_sqlexec @trgsql
";
    }
}