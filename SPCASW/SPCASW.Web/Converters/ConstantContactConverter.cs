using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPCASW.Marketing.Models;
using SPCASW.Common;

namespace SPCASW.Web.Converters
{
    public class ConstantContactConverter
    {
        public ConstantContactRecord ConvertFrom(CampaignRecipient recipient)
        {
            ConstantContactRecord result = new ConstantContactRecord();

            result.AddressLine1 = recipient.Contact.StreetAddress1;
            result.City = recipient.Contact.City;
            result.State = recipient.Contact.StateCode;
            result.ZipCode = recipient.Contact.PostalCode;
            result.EmailAddress = recipient.Contact.EmailAddress;
            result.FirstName = recipient.Contact.FirstName;
            result.LastName = recipient.Contact.LastName;

            if (!string.IsNullOrEmpty(recipient.Contact.PhoneType1) && !string.IsNullOrEmpty(recipient.Contact.Phone1) && recipient.Contact.PhoneType1 == "W")
            {
                result.WorkPhone = recipient.Contact.Phone1;
            }
            else if (!string.IsNullOrEmpty(recipient.Contact.PhoneType2) && !string.IsNullOrEmpty(recipient.Contact.Phone2) && recipient.Contact.PhoneType2 == "W")
            {
                result.WorkPhone = recipient.Contact.Phone2;
            }
            else if (!string.IsNullOrEmpty(recipient.Contact.PhoneType3) && !string.IsNullOrEmpty(recipient.Contact.Phone3) && recipient.Contact.PhoneType3 == "W")
            {
                result.WorkPhone = recipient.Contact.Phone3;
            }
            else if (!string.IsNullOrEmpty(recipient.Contact.PhoneType4) && !string.IsNullOrEmpty(recipient.Contact.Phone4) && recipient.Contact.PhoneType4 == "W")
            {
                result.WorkPhone = recipient.Contact.Phone4;
            }

            if (!string.IsNullOrEmpty(recipient.Contact.PhoneType1) && !string.IsNullOrEmpty(recipient.Contact.Phone1) && recipient.Contact.PhoneType1 == "H")
            {
                result.HomePhone = recipient.Contact.Phone1;
            }
            else if (!string.IsNullOrEmpty(recipient.Contact.PhoneType2) && !string.IsNullOrEmpty(recipient.Contact.Phone2) && recipient.Contact.PhoneType2 == "H")
            {
                result.HomePhone = recipient.Contact.Phone2;
            }
            else if (!string.IsNullOrEmpty(recipient.Contact.PhoneType3) && !string.IsNullOrEmpty(recipient.Contact.Phone3) && recipient.Contact.PhoneType3 == "H")
            {
                result.HomePhone = recipient.Contact.Phone3;
            }
            else if (!string.IsNullOrEmpty(recipient.Contact.PhoneType4) && !string.IsNullOrEmpty(recipient.Contact.Phone4) && recipient.Contact.PhoneType4 == "H")
            {
                result.HomePhone = recipient.Contact.Phone4;
            }

            return result;
        }
    }
}