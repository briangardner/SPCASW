-- Run "aspnet_regsql -S <server_name> -U <user> -P <password> -A all -d <database_name>" first
-- Edit and run "Helpers\Install.bat" first
use SPCA
go
exec dbo.drop_if_exists 'dbo.FK_CampaignRecipients_CampaignID'
exec dbo.drop_if_exists 'FK_CampaignRecipients_ContactID'
go
exec dbo.drop_if_exists 'dbo.Contacts'
go
create table dbo.Contacts (
	ContactID int identity
		constraint PK_Contacts
			primary key,
	FirstName varchar(100),
	LastName varchar(100),
	EmailAddress varchar(100),
	IsVolunteer bit not null
		constraint DF_Contacts_IsVolunteer
			default 0,
	IsDonor bit not null
		constraint DF_Contacts_IsDonor
			default 0,
	IsAdopter bit not null
		constraint DF_Contacts_IsAdoptee
			default 0,
	StreetAddress1 varchar(200),
	StreetAddress2 varchar(200),
	City varchar(100),
	StateCode char(2),
	PostalCode varchar(10),
	Phone1 varchar(20),
	PhoneType1 char(1),
	Phone2 varchar(20),
	PhoneType2 char(1),
	Phone3 varchar(20),
	PhoneType3 char(1),
	Phone4 varchar(20),
	PhoneType4 char(1),
	IsEmailAllowed bit not null
		constraint DF_Contacts_IsEmailAllowed
			default 0,
	IsMailAllowed bit not null
		constraint DF_Contacts_IsMailAllowed
			default 0,
	IsMailAddressValid bit not null
		constraint DF_Contacts_IsMailAddressValid
			default 1,
	PetPointID varchar(50),
	CreatedBy uniqueidentifier,
	CreatedOn datetime
		constraint DF_Contacts_CreatedOn
			default getdate()
)
go
exec dbo.drop_if_exists 'dbo.CampaignRecipients'
go
create table dbo.CampaignRecipients (
	CampaignRecipientID int identity
		constraint PK_CompaignRecipients
			primary key,
	CampaignID int not null,
	ContactID int not null
)
go
exec dbo.drop_if_exists 'dbo.Campaigns'
go
create table dbo.Campaigns (
	CampaignID int identity
		constraint PK_Campaign
			primary key,
	CampaignName varchar(100),
	CreatedBy uniqueidentifier,
	CreatedOn datetime
		constraint DF_Campaigns_CreatedOn
			default getdate()
)
go
alter table dbo.CampaignRecipients add
	constraint FK_CampaignRecipients_CampaignID
		foreign key (CampaignID)
		references dbo.Campaigns (CampaignID),
	constraint FK_CampaignRecipients_ContactID
		foreign key (ContactID)
		references dbo.Contacts (ContactID)
go
create index IX_CampaignRecipients_CampaignID_ContactID on dbo.CampaignRecipients (CampaignID, ContactID)
create index IX_CampaignRecipients_ContactID_CampaignID on dbo.CampaignRecipients (ContactID, CampaignID)
go
alter table dbo.Contacts add
	ModifiedBy uniqueidentifier,
	ModifiedOn datetime
		constraint DF_Contacts_ModifiedOn
			default getdate()
go
alter table dbo.Contacts add
	constraint FK_Contacts_CreatedBy
		foreign key (CreatedBy)
		references dbo.aspnet_Users (UserId),
	constraint FK_Contacts_ModifiedBy
		foreign key (ModifiedBy)
		references dbo.aspnet_Users (UserId)
go
create index IX_Contacts_CreatedBy on dbo.Contacts (CreatedBy)
create index IX_Contacts_ModifiedBy on dbo.Contacts (ModifiedBy)
go
alter table dbo.Contacts
	alter column EmailAddress varchar(128)
go
alter table dbo.Contacts
	alter column Phone1 varchar(30)
alter table dbo.Contacts
	alter column Phone2 varchar(30)
alter table dbo.Contacts
	alter column Phone3 varchar(30)
alter table dbo.Contacts
	alter column Phone4 varchar(30)
	
alter table dbo.Contacts_temp
	alter column Phone1 varchar(30)
alter table dbo.Contacts_temp
	alter column Phone2 varchar(30)
alter table dbo.Contacts_temp
	alter column Phone3 varchar(30)
alter table dbo.Contacts_temp
	alter column Phone4 varchar(30)
alter table dbo.Contacts add
	Notes varchar(max)
go