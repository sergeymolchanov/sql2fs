IF object_id('DDL_Log') is null begin
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

	end

/*SPLIT*/

if exists (select 1 from sys.triggers where name = 'TR_DDL_Events')
	DROP TRIGGER TR_DDL_Events ON DATABASE

if exists (select 1 from sys.all_objects where name = 'Push_SQL')
	DROP PROCEDURE Push_SQL

/*SPLIT*/

create procedure Push_SQL(
	@originalKey nvarchar(1000),
	@originalTime datetime,
	@sql	nvarchar(max))
as
begin
	if exists (select 1 from DDL_Log where RowKey = @originalKey)
		return

	declare @existsPtr table (ptr nvarchar(1000))
	declare @newKey	nvarchar(1000)
	declare @sqlExec nvarchar(MAX) = REPLACE(@sql, '$DBNAME', db_name())
	
	BEGIN TRANSACTION;

	BEGIN TRY
	insert into @existsPtr
	select RowKey from DDL_Log

	execute sp_sqlexec @sqlExec

	select @newKey = RowKey from DDL_Log where RowKey not in (select ptr from @existsPtr)

	if @newKey is not null
		update DDL_Log set IsOriginal = 0, OriginalTime = @originalTime, RowKey = @originalKey, CommandText = @sql where RowKey = @newKey
	else
		INSERT INTO DDL_Log(RowKey,EventType,PostTime,OriginalTime,ObjectType,CommandText,IsOriginal,LoginName,ServerName,DatabaseName,SchemaName,ObjectName)
     VALUES (@originalKey,'DML',GETDATE(),@originalTime,'DML',@sql,0,'?','?','?','?','DML')

	IF @@TRANCOUNT > 0
		COMMIT TRANSACTION;
	END TRY
	BEGIN CATCH
    SELECT 
        ERROR_NUMBER() AS ErrorNumber
        ,ERROR_SEVERITY() AS ErrorSeverity
        ,ERROR_STATE() AS ErrorState
        ,ERROR_PROCEDURE() AS ErrorProcedure
        ,ERROR_LINE() AS ErrorLine
        ,ERROR_MESSAGE() AS ErrorMessage;
	
    IF @@TRANCOUNT > 0
        ROLLBACK TRANSACTION;
    declare @err nvarchar(500) = '?????? ??? ?????????? SQL(' + @originalKey + '): ' + ERROR_MESSAGE() 
    raiserror(@err, 16, 1)
	END CATCH;
end

/*SPLIT*/

CREATE TRIGGER TR_DDL_Events
ON DATABASE
AFTER DDL_DATABASE_LEVEL_EVENTS
AS
BEGIN
	SET NOCOUNT ON
	DECLARE @data XML
	SET @data = EVENTDATA()

	DECLARE @originalTime	datetime = @data.value('(/EVENT_INSTANCE/PostTime)[1]','datetime')
	DECLARE @postTime		datetime = @data.value('(/EVENT_INSTANCE/PostTime)[1]','datetime')
	DECLARE @eventType		sysname = @data.value('(/EVENT_INSTANCE/EventType)[1]','sysname')
	DECLARE @objectName		sysname = @data.value('(/EVENT_INSTANCE/ObjectName)[1]','sysname')
	DECLARE @objectType		sysname = @data.value('(/EVENT_INSTANCE/ObjectType)[1]','sysname')
	DECLARE	@RowKeyBase		nvarchar(2000) = REPLACE(REPLACE(CONVERT(nvarchar(max),@originalTime,(120)), ' ', 'T'), ':', '-')+'_'+@eventType+'_'+@objectName
	DECLARE @RowKey			nvarchar(2000)
	DECLARE @AppendNum		int = 0
	DECLARE	@commandText	nvarchar(max) = @data.value('(/EVENT_INSTANCE/TSQLCommand/CommandText)[1]','nvarchar(max)')

    if @objectType in ('TRIGGER', 'PROCEDURE', 'FUNCTION', 'VIEW')
        return

	if upper(@commandText) like 'ALTER%INDEX%REBUILD%'
		return

	select @AppendNum = count(*)
	  from DDL_Log 
	 where cast(PostTime as smalldatetime) = cast(@postTime as smalldatetime)

	set @RowKey = @RowKeyBase + case when @AppendNum = 0 then '' else '_' + cast(@AppendNum as nvarchar(10)) end

	insert into DDL_Log
		(RowKey, EventType, PostTime, OriginalTime, SPID, LoginName, ServerName, DatabaseName, SchemaName, ObjectName, ObjectType, CommandText, IsOriginal)
	select
		RowKey = @RowKey,
		EventType = @eventType,
		PostTime = @postTime,
		OriginalTime = @originalTime,
		SPID = @data.value('(/EVENT_INSTANCE/SPID)[1]','int'),
		LoginName = @data.value('(/EVENT_INSTANCE/LoginName)[1]','sysname'),
		ServerName = @data.value('(/EVENT_INSTANCE/ServerName)[1]','sysname'),
		DatabaseName = @data.value('(/EVENT_INSTANCE/DatabaseName)[1]','sysname'),
		SchemaName = isnull(@data.value('(/EVENT_INSTANCE/SchemaName)[1]','sysname'), '-'),
		ObjectName = @objectName,
		ObjectType = @objectType,
		CommandText = @commandText,
		IsOriginal = 1
END

/*SPLIT*/

if object_id('psm_data') is null 
	CREATE TABLE [dbo].[psm_data] (ukey uniqueidentifier NULL)