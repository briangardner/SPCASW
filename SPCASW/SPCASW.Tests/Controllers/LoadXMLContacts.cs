using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.IO;
using System.Xml.Linq;
using SPCASW.Common;


namespace SPCASW.Tests.Controllers
{
    [TestClass]
    public class LoadXMLContacts
    {
        [TestMethod]
        public void BuildXMLContacts()
        {
            String lPath = Path.Combine("D:\\SPCASW\\DB\\Legacy Data\\contacts.xml");
            XDocument lDoc = XDocument.Load(lPath);
            foreach(XElement lAddressBook in lDoc.Elements("addressbook"))
            {
                foreach(XElement lContactRecord in lAddressBook.Elements("contact"))
                {
                    ParseContact(lContactRecord);
                }
            }

    
        }

        

        private void ParseContact(XElement lContactRecord)
        {
            Contacts_temp lContact = new Contacts_temp();
 	        if(lContactRecord.Element("name") != null)
            {
                XElement lName = lContactRecord.Element("name");
                ParseName(lContact, lName);
            }

            ParsePhones(lContact, lContactRecord);

            if(lContactRecord.Element("email").Value != null)
            {
                lContact.EmailAddress = Truncate(lContactRecord.Element("email").Value ,128);
            }

            ParseHomeAddress(lContact, lContactRecord);

            if (lContactRecord.Element("notes") != null)
            {
                lContact.Notes = Truncate( lContactRecord.Element("notes").Value , 1000);
            }

            using (SPCAContactsEntities lEntities = new SPCAContactsEntities())
            {
                lEntities.AddToContacts_temp(lContact);
                lEntities.SaveChanges();
            }
            
        }

        private String Truncate(string aTemp, int aLength)
        {
            return Utils.Truncate(aTemp, aLength);

        }

        private void ParseName(Contacts_temp aContact, XElement aName)
        {
            String lFirstName = String.Empty;
            String lLastName = String.Empty;


            if (aName.Element("first") != null)
            {
                lFirstName = aName.Element("first").Value;
            }
            if (aName.Element("last") != null)
            {
                lLastName = aName.Element("last").Value;
            }

            //Parse records with only a display name;
            if (aName.Element("first") == null && aName.Element("last") == null && aName.Element("display") != null)
            {
                String lDisplay = aName.Element("display").Value;

                if (lDisplay.Contains(','))
                {
                    String[] lNames = lDisplay.Split(',');
                    lLastName = lNames[0];
                    lFirstName = lNames[1];
                }
                else
                {
                    lFirstName = lDisplay;
                }
            }
            aContact.FirstName =  Truncate(lFirstName, 100);
            aContact.LastName = Truncate(lLastName,100);
        }

        private void ParsePhones(Contacts_temp aContact, XElement aContactXML)
        {
            if (aContactXML.Element("home_phone_1") != null)
            {
                aContact.Phone1 = Truncate(aContactXML.Element("home_phone_1").Value, 30);
                aContact.PhoneType1 = "H";
            }
            if (aContactXML.Element("home_phone_2") != null)
            {
                aContact.Phone2 = Truncate( aContactXML.Element("home_phone_2").Value, 30);
                aContact.PhoneType2 = "H";
            }
            if (aContactXML.Element("mobile_phone") != null)
            {
                aContact.Phone3 = Truncate( aContactXML.Element("mobile_phone").Value, 30);
                aContact.PhoneType3 = "M";
            }

        }

        private void ParseHomeAddress(Contacts_temp aContact, XElement aContactRecord)
        {
            if (aContactRecord.Element("home_address_street") != null)
            {
                aContact.StreetAddress1 = Truncate(aContactRecord.Element("home_address_street").Value, 200);
            }
            if (aContactRecord.Element("home_address_city") != null)
            {
                aContact.City = Truncate( aContactRecord.Element("home_address_city").Value , 100);
            }
            if (aContactRecord.Element("home_address_state") != null)
            {
                aContact.StateCode = Utils.StateToStateCode(aContactRecord.Element("home_address_state").Value.ToUpper());
            }
            
            if (aContactRecord.Element("home_address_zip") != null)
            {
                aContact.PostalCode = Truncate(aContactRecord.Element("home_address_zip").Value, 10);
            }
        }
    }
}
