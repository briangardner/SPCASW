using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPCASW.Data.Repositories;
using SPCASW.Common;

namespace SPCASW.Data.Services
{
    public class ContactsService
    {
        public IContactsRepository _repository;

        public ContactsService() {

            _repository = new ContactRepository();
        
        }

        public Contact GetContact(int contactId)
        {
            return _repository.GetContact(contactId);
        }

        public List<Contact> GetContacts() {

            return _repository.GetContacts();
        }


        public bool Update(Contact model)
        {
            return _repository.Update(model);
        }

        public bool Create(Contact model)
        {
            return _repository.Create(model);
        }
    }
}
