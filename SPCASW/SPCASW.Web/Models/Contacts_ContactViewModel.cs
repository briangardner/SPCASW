using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPCASW.Common;

namespace SPCASW.Web.Models
{
	public class Contacts_ContactViewModel : ContactViewModel
	{
		public Contacts_ContactViewModel() : base()
		{
			ContactID = 0;
			FirstName = String.Empty;
			LastName = String.Empty;
			EmailAddress = String.Empty;
			IsVolunteer = false;
			IsDonor = false;
			IsAdopter = false;
			City = String.Empty;
			StateCode = String.Empty;
			PostalCode = String.Empty;
			IsEmailAllowed = false;
			IsMailAllowed = false;
			IsMailAddressValid = false;
			PetPointID = String.Empty;
			CreatedBy = Guid.Empty;
			CreatedOn = DateTime.Now;
			StreetAddress1 = String.Empty;
			StreetAddress2 = String.Empty;
			Phone1 = String.Empty;
			PhoneType1 = String.Empty;
			Phone2 = String.Empty;
			PhoneType2 = String.Empty;
			Phone3 = String.Empty;
			PhoneType3 = String.Empty;
			Phone4 = String.Empty;
			PhoneType4 = String.Empty;
			ModifiedBy = Guid.Empty;
			ModifiedOn = DateTime.Now;
			Notes = String.Empty;
		}

		public override void  UpdateModel(Contact model)
		{
			model.ContactID = ContactID;
			model.FirstName = FirstName ?? String.Empty;
			model.LastName = LastName ?? String.Empty;
			model.EmailAddress = EmailAddress ?? String.Empty;
			model.IsVolunteer = IsVolunteer;
			model.IsDonor = IsDonor;
			model.IsAdopter = IsAdopter;
			model.City = City ?? String.Empty;
			model.StateCode = StateCode ?? String.Empty;
			model.PostalCode = PostalCode ?? String.Empty;
			model.IsEmailAllowed = IsEmailAllowed;
			model.IsMailAllowed = IsMailAllowed;
			model.IsMailAddressValid = IsMailAddressValid;
			model.PetPointID = PetPointID ?? String.Empty;
			model.CreatedBy = CreatedBy ?? Guid.Empty;
			model.CreatedOn = CreatedOn ?? DateTime.Now;
			model.StreetAddress1 = StreetAddress1 ?? String.Empty;
			model.StreetAddress2 = StreetAddress2 ?? String.Empty;
			model.Phone1 = Phone1 ?? String.Empty;
			model.PhoneType1 = PhoneType1 ?? String.Empty;
			model.Phone2 = Phone2 ?? String.Empty;
			model.PhoneType2 = PhoneType2 ?? String.Empty;
			model.Phone3 = Phone3 ?? String.Empty;
			model.PhoneType3 = PhoneType3 ?? String.Empty;
			model.Phone4 = Phone4 ?? String.Empty;
			model.PhoneType4 = PhoneType4 ?? String.Empty;
			model.ModifiedBy = ModifiedBy ?? Guid.Empty;
			model.ModifiedOn = ModifiedOn ?? DateTime.Now;
			model.Notes = Notes ?? String.Empty;
		}
	}
}