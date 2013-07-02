using System;
using System.Collections.Generic;
using System.Linq;
using SPCASW.Common;

namespace SPCASW.Data.Repositories
{
    public class ContactRepository : IContactsRepository
    {
        public ContactRepository()
        {
        }

        public List<Contact> GetContacts()
        {
            var list = new List<Contact>();
            using (var db = new SPCAContactsEntities())
            {
                var result = (from contacts in db.Contacts select contacts).ToList();
                foreach (var dbcontact in result)
                {
                    list.Add(dbcontact);
                }
                return list;

            }
        }


        public SPCASW.Common.Contact GetContact(int contactId)
        {
            using (var db = new SPCAContactsEntities())
            {
                var result = (from contacts in db.Contacts where contacts.ContactID == contactId select contacts).FirstOrDefault();
                return result;

            }
        }



        public bool Update(Contact model)
        {
            try
            {
                using (var db = new SPCAContactsEntities())
                {
                    var result = (from contacts in db.Contacts where contacts.ContactID == model.ContactID select contacts).FirstOrDefault();
                    result.FirstName = model.FirstName;
                    result.LastName = model.LastName;
                    result.EmailAddress = model.EmailAddress;
                    result.IsAdopter = model.IsAdopter;
                    result.IsVolunteer = model.IsVolunteer;
                    result.StreetAddress1 = model.StreetAddress1;
                    result.StreetAddress2 = model.StreetAddress2;
                    result.ModifiedBy = model.ModifiedBy;
                    result.ModifiedOn = DateTime.Now;
                    result.Phone1 = model.Phone1;
                    result.Phone2 = model.Phone2;
                    result.Phone3 = model.Phone3;
                    result.Phone4 = model.Phone4;
                    result.City = model.City;
                    result.IsDonor = model.IsDonor;
                    result.IsMailAllowed = model.IsMailAllowed;
                    result.IsEmailAllowed = model.IsEmailAllowed;
                    result.StateCode = model.StateCode;
                    result.PostalCode = model.PostalCode;
                    result.Notes = model.Notes;
                    result.IsMailAddressValid = model.IsMailAddressValid;

                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                return false;
            }

        }


        public bool Create(Contact contact)
        {
            try
            {

                using (var db = new SPCAContactsEntities())
                {

                    db.Contacts.AddObject(contact);
                    db.SaveChanges();
                    return true;
                }

                return false;

            }
            catch (Exception e)
            {
                return false;
            }
        }


        
        
    }
}