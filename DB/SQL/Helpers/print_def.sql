exec dbo.drop_if_exists 'dbo.print_def'
GO
-- Outputs the definition of an object 
create procedure [dbo].[print_def]
	@name nvarchar(max)
as
set nocount on

declare
	@object_id int = object_id(@name)
	
declare
	@object_name sysname = object_name(@object_id),
	@object_schema_name sysname = object_schema_name(@object_id),
	@object_definition nvarchar(max)
	
if exists (select * from sys.sql_modules where object_id = @object_id)
	set @object_definition = '
' + object_definition(@object_id)
else
if exists (select * from sys.tables where object_id = @object_id)
begin
	declare @output nvarchar(max), @deferred_ddl nvarchar(max)
	exec dbo.tbl_def @name, @output out, @deferred_ddl out
	set @object_definition = @output + @deferred_ddl 
end
	
declare @def nvarchar(max) = 'exec dbo.drop_if_exists ''' + quotename(@object_schema_name) + '.' + quotename(@object_name) + '''
GO' + ltrim(rtrim(@object_definition)) + '
GO'

exec dbo.print_long @def
GO