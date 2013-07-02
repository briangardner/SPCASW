exec dbo.drop_if_exists 'dbo.object_type_name'
go
-- Converts type codes from sys.objects into their corresponding SQL keywords
create function dbo.object_type_name
(
	@type char(2)
)
returns varchar(10)
as
begin
	return case
		when @type in ('AF', 'FN', 'FS', 'FT', 'IF', 'TF') then 'function'
		when @type in ('C', 'D', 'F', 'PK', 'UQ') then 'constraint'
		when @type in ('IT', 'S', 'U') then 'table'
		when @type in ('P', 'PC', 'RF', 'X') then 'procedure'
		when @type in ('PG') then 'plan'
		when @type in ('R') then 'rule'
		when @type in ('SN') then 'synonym'
		when @type in ('SQ') then 'queue'
		when @type in ('TA', 'TR') then 'trigger'
		when @type in ('TT') then 'type'
		when @type in ('V') then 'view'
		else null
	end
end
go
exec dbo.drop_if_exists 'dbo.describe_object'
go
/*
Adds or updates a description to an object.
Specify object with two or three part names.
First part must be schema.
Second part must be a schema-scoped object. Supported types:
	Functions
	Procedures
	Tables
	Views
Optional third part must be a child object of the schema-scoped object. Supported types:
	Indexes
	Parameters
	Columns
	Constraints
	Triggers
*/
create procedure dbo.describe_object
	@object_fullname nvarchar(max),
	@description nvarchar(3750)
as
set nocount on

declare
	@level0name sysname,
	@level1name sysname,
	@level2name sysname,
	@level0type varchar(128) = 'schema',
	@level1type varchar(128),
	@level2type varchar(128)

select top 1
	@level0name = parsename(@object_fullname, n),
	@level1name = parsename(@object_fullname, n - 1),
	@level2name = parsename(@object_fullname, n - 2)
from dbo.sequence
where n between 1 and 3
and parsename(@object_fullname, n) is not null
order by n desc

declare @schema_id int = schema_id(@level0name)

if @schema_id is null or @level1name is null
begin
	raiserror('Only schema-scoped objects are supported right now. If you need to describe a database-level object, modify this procedure', 11, 1)
	return
end

select @level1type = dbo.object_type_name(type)
from sys.objects
where schema_id = @schema_id
and name = @level1name

if @level1type is null
begin
	raiserror('Object not found. (%s)', 11, 1, @object_fullname)
	return
end

-- Automatically detect level1 type & name for level2 items specified at level1
if @level1type in ('trigger', 'constraint')
begin
	set @level2type = @level1type
	set @level2name = @level1name
	
	select
		@level1type = dbo.object_type_name(parent.type),
		@level1name = parent.name
	from sys.objects parent
	join sys.objects child on child.parent_object_id = parent.object_id
	where child.schema_id = @schema_id
	and child.name = @level2name
end

declare @level1object_id int = object_id(quotename(@level0name) + '.' + quotename(@level1name))

if @level2name is not null and @level2type is null
begin
	if exists (
		select *
		from sys.indexes
		where object_id = @level1object_id
		and name = @level2name
	) set @level2type = 'INDEX' else
	if exists (
		select *
		from sys.parameters
		where object_id = @level1object_id
		and name = @level2name
	) set @level2type = 'PARAMETER' else
	if exists (
		select *
		from sys.columns
		where object_id = @level1object_id
		and name = @level2name
	) set @level2type = 'COLUMN' else
		select @level2type = dbo.object_type_name(type)
		from sys.objects
		where parent_object_id = @level1object_id
		and name = @level2name

	if @level2type is null
	begin
		raiserror('Object not found. (%s)', 11, 1, @object_fullname)
		return
	end
end

if exists (
	select *
	from fn_listextendedproperty(
		'MS_Description',
		@level0type, @level0name,
		@level1type, @level1name,
		@level2type, @level2name)
) exec sp_updateextendedproperty
	'MS_Description', @description,
	@level0type, @level0name,
	@level1type, @level1name,
	@level2type, @level2name
else exec sp_addextendedproperty
	'MS_Description', @description,
	@level0type, @level0name,
	@level1type, @level1name,
	@level2type, @level2name
go