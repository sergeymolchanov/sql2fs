using System;
using System.Data.SqlClient;

namespace ProjectSourceManager.Adapters.Impl
{
    public class DDLAdapter : AdapterBaseSQL
    {
        private const String QueryString = @"select cast(RowPointer as nvarchar(100)) RowPointer, OriginalTime, CommandText from _DDL_Log where IsOriginal = 1";

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
                    AddItem(String.Format("{0}_{1}", dr.GetDateTime(1).ToString("yyyyMMddHHmmss"), dr.GetString(0)));
                }
            }
        }

        protected override void DoSort()
        {
            Items.Sort();
        }

        public override String Prefix { get { return "DatabaseDDL"; } }
        public override String Postfix { get { return ".sql"; } }

        private const String INIT_SQL = @"
IF object_id('_DDL_Log') is not null
	return

create table _DDL_Log (
	RowPointer	uniqueidentifier,
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

declare @proc nvarchar(max) = '
create procedure _Push_SQL(
	@ptr uniqueidentifier,
	@originalTime datetime,
	@sql	nvarchar(max))
as
begin
	declare @existsPtr table (ptr uniqueidentifier)
	declare @newPtr	uniqueidentifier

	insert into @existsPtr
	select RowPointer from _DDL_Log

	execute sp_sqlexec @sql

	select @newPtr = RowPointer from _DDL_Log where RowPointer not in (select ptr from @existsPtr)

	update _DDL_Log set IsOriginal = 0, OriginalTime = @originalTime, RowPointer = @ptr where RowPointer = @newPtr
end '

declare @trgsql nvarchar(max) = '
create TRIGGER [TR_DDL_Events]
ON DATABASE
AFTER DDL_DATABASE_LEVEL_EVENTS
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @data XML
	SET @data = EVENTDATA()
	insert into _DDL_Log
		(RowPointer, EventType, PostTime, OriginalTime, SPID, LoginName, ServerName, DatabaseName, SchemaName, ObjectName, ObjectType, CommandText, IsOriginal)
	select
		RowPointer = NEWID(),
		EventType = @data.value(''(/EVENT_INSTANCE/EventType)[1]'',''sysname''),
		PostTime = @data.value(''(/EVENT_INSTANCE/PostTime)[1]'',''datetime''),
		OriginalTime = @data.value(''(/EVENT_INSTANCE/PostTime)[1]'',''datetime''),
		SPID = @data.value(''(/EVENT_INSTANCE/SPID)[1]'',''int''),
		LoginName = @data.value(''(/EVENT_INSTANCE/LoginName)[1]'',''sysname''),
		ServerName = @data.value(''(/EVENT_INSTANCE/ServerName)[1]'',''sysname''),
		DatabaseName = @data.value(''(/EVENT_INSTANCE/DatabaseName)[1]'',''sysname''),
		SchemaName = @data.value(''(/EVENT_INSTANCE/SchemaName)[1]'',''sysname''),
		ObjectName = @data.value(''(/EVENT_INSTANCE/ObjectName)[1]'',''sysname''),
		ObjectType = @data.value(''(/EVENT_INSTANCE/ObjectType)[1]'',''sysname''),
		CommandText = @data.value(''(/EVENT_INSTANCE/TSQLCommand/CommandText)[1]'',''nvarchar(max)''),
		IsOriginal = 1
END'

execute sp_sqlexec @proc
execute sp_sqlexec @trgsql
";
    }
}