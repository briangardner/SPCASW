/*
-- Questions --
Is there any way in DonorPerfect to tell if a contact is an adopter or volunteer?
I'm using non-null donor_type to mean the contact is a donor of some sort
	-- also use donor amount if available
*/
--exec sp_addlinkedserver 'daspnet_spca', @provider = 'SQLNCLI', @datasrc = 'sql2k804.discountasp.net', @srvproduct='', @catalog = 'SQL2008R2_900203_spcacontacts'
--exec sp_addlinkedsrvlogin 'daspnet_spca', @useself = 'false', @rmtuser = 'SQL2008R2_900203_spcacontacts_user', @rmtpassword = '$pc@$@'

-- Can't easily use transactions to remote server
--begin tran
--set xact_abort on

insert daspnet_spca.SQL2008R2_900203_spcacontacts.dbo.Contacts_temp (
	FirstName,
	LastName,
	EmailAddress,
	IsVolunteer,
	IsDonor,
	IsAdopter,
	StreetAddress1,
	StreetAddress2,
	City,
	StateCode,
	PostalCode,
	Phone1,
	PhoneType1,
	Phone2,
	PhoneType2,
	Phone3,
	PhoneType3,
	Phone4,
	PhoneType4,
	IsEmailAllowed,
	IsMailAllowed,
	IsMailAddressValid,
	PetPointID,
	CreatedBy,
	CreatedOn
)
select
	left(first_name, 100),
	left(last_name, 100),
	left(email, 100),
	0,
	case when donor_type is not null then 1 else 0 end,
	0,
	left(address, 200),
	left(address2, 200),
	left(city, 100),
	left(state, 2),
	left(zip, 10),
	left(home_phone, 20),
	'H',
	left(business_phone, 20),
	'W',
	left(fax_phone, 20),
	'F',
	left(mobile_phone, 20),
	'M',
	case no_email when 'N' then 1 else 0 end,
	case nomail when 'N' then 1 else 0 end,
	case when nomail_reason is null then 1 else 0 end,
	null,
	null,
	getdate()
from dbo.dp
go 
--if @@trancount > 0 rollback