using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using SPCASW.Common;
using System.Text.RegularExpressions;

namespace SPCASW.PetPoint
{

   

    public class XmlParser
    {
        private static int NAME_LENGTH = 100;
        private static int PHONE_LENGTH = 30;
        private static int STREET_LENGTH = 200;
        private static int STATE_LENGTH = 2;
        private static int POSTAL_LENGTH = 10;
        private static int EMAIL_LENGTH = 100;
        private static int CITY_LENGTH = 100;
        private static int PHONE_TYPE_LENGTH = 1;


        private XmlDocument xml;
        private List<Contact> contacts;
        enum AddressItem {NONE, StreetNumber, StreetName, City, State, Postal}

        public XmlParser(string filepath)        
        {
            xml = new XmlDocument();
            xml.Load(filepath);
            contacts = new List<Contact>();

            Parse();
            Process();
        }

        public XmlParser(XmlDocument xml)
        {
            this.xml = xml;
            contacts = new List<Contact>();

            Parse();
            Process();
        }


        private void Parse()
        {

            // First Person
            Contact c = null;
            Regex reg;
            XmlNode node = xml.ChildNodes[1].FirstChild.FirstChild.FirstChild;
            XmlNode address, phone, email;

            string temp;
            string[] temps;
            int xx;

            // While there are still contacts
            while (node != null)
            {
                // Create Contact
                c = new Contact();

                // NAME
                temps = node.Attributes[0].Value.Trim().Split(' ');
                if (temps.Length == 2)
                {
                    c.FirstName = Utils.Truncate(temps[0], NAME_LENGTH);
                    c.LastName = Utils.Truncate(temps[1], NAME_LENGTH);
                }
                else if (temps.Length == 1)
                {
                    c.FirstName = Utils.Truncate(temps[0], NAME_LENGTH);
                }
                else
                {
                    // Assume last name is always one word. Some name entries are "Mark & Sue"
                    c.FirstName = Utils.Truncate(temps[0], NAME_LENGTH);
                    c.LastName = Utils.Truncate(temps[temps.Length - 1], NAME_LENGTH);
                    for (int i = 1; i < temps.Length - 1; i++)
                    {
                        c.FirstName += " " + temps[i];
                    }
                    c.FirstName = Utils.Truncate(c.FirstName, NAME_LENGTH);
                }

                // ID
                c.PetPointID = node.Attributes[1].Value.Trim();

                // Association
                temp = node.Attributes[2].Value.Trim();
                if (temp == "Adopter")
                {
                    c.IsAdopter = true;
                }
                else if (temp == "Donor")
                {
                    c.IsDonor = true;
                }
                else if (temp == "Volunteer")
                {
                    c.IsVolunteer = true;
                }

                // ADDRESS
                // If contact has an address AND is not empty
                if (node.FirstChild.FirstChild.FirstChild.FirstChild.FirstChild.HasChildNodes && node.FirstChild.FirstChild.FirstChild.FirstChild.FirstChild.FirstChild.FirstChild.Attributes.Count > 0)
                {
                    // Get first address node
                    address = node.FirstChild.FirstChild.FirstChild.FirstChild.FirstChild.FirstChild.FirstChild;
                    temp = address.Attributes[1].Value.Trim();
                    temps = temp.Split(' ');
                    
                    // Some contacts only have a State
                    if (temp.Split(' ').Length == 1)
                    {
                        c.StateCode = Utils.StateToStateCode(temp);
                        c.IsMailAddressValid = false;
                    }
                    else
                    {
                        reg = new Regex("   ");
                        temps = reg.Split(temp);

                        // Simple one line address
                        if (temps.Length == 2)
                        {
                            c.StreetAddress1 = Utils.Truncate(temps[0], STREET_LENGTH);

                            temps = temps[1].Split(' ');

                            // Simple one word city and state
                            if (temps.Length == 3)
                            {
                                c.City = Utils.Truncate(temps[0], CITY_LENGTH);
                                c.StateCode = Utils.StateToStateCode(temps[1]);
                                c.PostalCode = Utils.Truncate(temps[2], POSTAL_LENGTH);
                            }
                            else
                            {
                                temp = "";
                                for (int i = 0; i < temps.Length; i++)
                                {
                                    // CITY, Regex only letters
                                    if (Regex.IsMatch(temps[i], @"^[\p{L}]+$") && temps[i] == temps[i].ToUpper())
                                    {
                                        if (c.City == null)
                                        {
                                            c.City = temps[i];
                                        }
                                        else
                                        {
                                            c.City += " " + temps[i];
                                        }
                                        c.City = Utils.Truncate(c.City, CITY_LENGTH);
                                    }

                                    // POSTAL CODE
                                    else if (Int32.TryParse(temps[i], out xx))
                                    {
                                        c.PostalCode = Utils.Truncate( temps[i], POSTAL_LENGTH);
                                    }

                                    // STATE, Regex for only letters
                                    else if (Regex.IsMatch(temps[i], @"^[\p{L}]+$")) 
                                    {
                                        if (temp == "")
                                        {
                                            temp = temps[i];
                                        }
                                        else
                                        {
                                            temp += " " + temps[i];
                                        }
                                    }
                                    else{
                                        //c.StreetAddress2 = 
                                        c.IsMailAddressValid = false;
                                    }
                                }

                                if (temp != "")
                                {
                                    c.StateCode = Utils.StateToStateCode(temp);
                                }
                            }
                        }

                        // Generally (not always) uses 2nd address field
                        /*else if (temps.Length == 1)
                        {
                            reg = new Regex("  ");
                            temps = reg.Split(temp);

                            if (temps.Length == 3)
                            {
                                c.StateCode = Utils.StateToStateCode(temp);

                            }
                        }*/

                        else
                        {
                            c.StreetAddress1 = Utils.Truncate(temp.Trim(), STREET_LENGTH);
                            c.IsMailAddressValid = false;
                        }
                    }
                }
                else
                {
                    address = null;
                }

                // PHONE
                // If contact has a phone AND is not empty
                if (node.FirstChild.FirstChild.FirstChild.NextSibling.FirstChild.FirstChild.HasChildNodes && node.FirstChild.FirstChild.FirstChild.NextSibling.FirstChild.FirstChild.FirstChild.FirstChild.Attributes.Count > 0)
                {
                    // Get first phone node
                    phone = node.FirstChild.FirstChild.FirstChild.NextSibling.FirstChild.FirstChild.FirstChild.FirstChild;
                    for(int i = 1; i < 5 && phone != null; i++)
                    {
                        switch (i)
                        {
                            case 1:
                                c.PhoneType1 = Utils.Truncate(phone.Attributes[0].Value, PHONE_TYPE_LENGTH);
                                c.Phone1 = Utils.Truncate(phone.Attributes[1].Value.Trim(), PHONE_LENGTH);
                                break;

                            case 2:
                                c.PhoneType2 = Utils.Truncate(phone.Attributes[0].Value, PHONE_TYPE_LENGTH);
                                c.Phone2 = Utils.Truncate( phone.Attributes[1].Value.Trim(), PHONE_LENGTH);
                                break;

                            case 3:
                                c.PhoneType3 = Utils.Truncate(phone.Attributes[0].Value, PHONE_TYPE_LENGTH);
                                c.Phone3 = Utils.Truncate(phone.Attributes[1].Value.Trim(), PHONE_LENGTH);
                                break;

                            case 4:
                                c.PhoneType4 = Utils.Truncate(phone.Attributes[0].Value, PHONE_TYPE_LENGTH);
                                c.Phone4 = Utils.Truncate(phone.Attributes[1].Value.Trim(), PHONE_LENGTH);
                                break;

                        }
                        phone = phone.NextSibling;
                    }
                }
                else
                {
                    phone = null;
                }

                // EMAIL
                // If contact has an email AND is not empty
                if (node.FirstChild.FirstChild.FirstChild.NextSibling.NextSibling.FirstChild.FirstChild.HasChildNodes && node.FirstChild.FirstChild.FirstChild.NextSibling.NextSibling.FirstChild.FirstChild.FirstChild.FirstChild.Attributes.Count > 0)
                {
                    // Get first email node
                    email = node.FirstChild.FirstChild.FirstChild.NextSibling.NextSibling.FirstChild.FirstChild.FirstChild.FirstChild;
                    
                    
                    c.EmailAddress = Utils.Truncate(email.Attributes[1].Value.Trim(), EMAIL_LENGTH);
                      
                }
                else
                {
                    email = null;
                }

                contacts.Add(c);
                node = node.NextSibling;
            }
        }

        private void Process()
        {

            using (SPCAContactsEntities n = new SPCASW.Common.SPCAContactsEntities())
            {
                foreach (Contact c in contacts)
                {
                    var ct = from cont in n.Contacts
                             where cont.PetPointID == c.PetPointID
                             select cont;

                    if (ct.Count() == 0)
                    {
                        n.AddToContacts(c);
                        try
                        {
                            n.SaveChanges();
                        }
                        catch (Exception ex)
                        {
                            throw ex;
                        }
                    }
                }

                

                
            }
            
        }
    }
}
