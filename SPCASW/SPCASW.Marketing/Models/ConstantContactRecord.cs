using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using FileHelpers;
using SPCASW.Common.Attributes;

namespace SPCASW.Marketing.Models
{
    [DelimitedRecord(ConstantContactFileEngine.FILE_DELIMITER)]
    public class ConstantContactRecord
    {
        [HeaderText("Email Address")]
        [FieldQuoted]
        public string EmailAddress;

        [HeaderText("First Name")]
        [FieldQuoted]
        public string FirstName;

        [HeaderText("Middle Name")]
        [FieldQuoted]
        public string MiddleName;

        [HeaderText("Last Name")]
        [FieldQuoted]
        public string LastName;

        [HeaderText("Company Name")]
        [FieldQuoted]
        public string CompanyName;

        [HeaderText("City")]
        [FieldQuoted]
        public string City;

        [HeaderText("US State/CA Province")]
        [FieldQuoted]
        public string State;

        [HeaderText("Other State/Province")]
        [FieldQuoted]
        public string Province;

        [HeaderText("Zip/Postal Code")]
        [FieldQuoted]
        public string ZipCode;

        [HeaderText("Sub Zip/Postal Code")]
        [FieldQuoted]
        public string SubZipCode;

        [HeaderText("Country")]
        [FieldQuoted]
        public string Country;

        [HeaderText("Address Line 1")]
        [FieldQuoted]
        public string AddressLine1;

        [HeaderText("Address Line 2")]
        [FieldQuoted]
        public string AddressLine2;

        [HeaderText("Address Line 3")]
        [FieldQuoted]
        public string AddressLine3;

        [HeaderText("Home Phone")]
        [FieldQuoted]
        public string HomePhone;

        [HeaderText("Work Phone")]
        [FieldQuoted]
        public string WorkPhone;

        [HeaderText("Job Title")]
        [FieldQuoted]
        public string JobTitle;

        [HeaderText("Notes")]
        [FieldQuoted]
        public string Notes;

        [HeaderText("Custom Field 1")]
        [FieldQuoted]
        public string CustomField1;

        [HeaderText("Custom Field 2")]
        [FieldQuoted]
        public string CustomField2;

        [HeaderText("Custom Field 3")]
        [FieldQuoted]
        public string CustomField3;

        [HeaderText("Custom Field 4")]
        [FieldQuoted]
        public string CustomField4;

        [HeaderText("Custom Field 5")]
        [FieldQuoted]
        public string CustomField5;

        [HeaderText("Custom Field 6")]
        [FieldQuoted]
        public string CustomField6;

        [HeaderText("Custom Field 7")]
        [FieldQuoted]
        public string CustomField7;

        [HeaderText("Custom Field 8")]
        [FieldQuoted]
        public string CustomField8;

        [HeaderText("Custom Field 9")]
        [FieldQuoted]
        public string CustomField9;

        [HeaderText("Custom Field 10")]
        [FieldQuoted]
        public string CustomField10;

        [HeaderText("Custom Field 11")]
        [FieldQuoted]
        public string CustomField11;

        [HeaderText("Custom Field 12")]
        [FieldQuoted]
        public string CustomField12;

        [HeaderText("Custom Field 13")]
        [FieldQuoted]
        public string CustomField13;

        [HeaderText("Custom Field 14")]
        [FieldQuoted]
        public string CustomField14;

        [HeaderText("Custom Field 15")]
        [FieldQuoted]
        public string CustomField15;
    }
}