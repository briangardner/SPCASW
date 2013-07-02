using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPCASW.Common;

namespace SPCASW.Data.Repositories
{
    public interface IContactsRepository
    {
       Contact GetContact(int contactId);

       List<Contact> GetContacts();

       bool Update(Contact model);

       bool Create(Contact model);

    }
}
