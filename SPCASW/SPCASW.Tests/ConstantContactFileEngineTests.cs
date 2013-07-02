using System.Collections.Generic;
using System.IO;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPCASW.Marketing;
using SPCASW.Marketing.Models;

namespace SPCASW.Tests
{
    [TestClass]
    public class ConstantContactFileEngineTests
    {
        [TestMethod]
        public void ConstantContactFileEngineTest()
        {
            List<ConstantContactRecord> records = new List<ConstantContactRecord>();
            records.Add(new ConstantContactRecord()
            {
                EmailAddress = "test@test.com",
                
                FirstName = "Test",
                MiddleName = "A",
                LastName = "User",
                AddressLine1 = "1234 Somewhere Dr.",
                AddressLine2 = "Address Line 2",
                AddressLine3 = "Address Line 3",
                City = "Timbuktoo",
                State = "AK",
                Province = "Province",
                ZipCode = "99999",
                SubZipCode = "9999",                
                Country = "Los Estados Unidos",
                HomePhone = "555-555-1111",
                WorkPhone = "555-555-2222",                
                Notes = "Test note",
                CompanyName = "Acme Co.",
                JobTitle = "Important Job Title!",
                CustomField1 = "Custom 1",
                CustomField2 = "Custom 2",
                CustomField3 = "Custom 3", 
                CustomField4 = "Custom 4",
                CustomField5 = "Custom 5",
                CustomField6 = "Custom 6",
                CustomField7 = "Custom 7",
                CustomField8 = "Custom 8",
                CustomField9 = "Custom 9",
                CustomField10 = "Custom 10",
                CustomField11 = "Custom 11",
                CustomField12 = "Custom 12",
                CustomField13 = "Custom 13",
                CustomField14 = "Custom 14", 
                CustomField15 = "Custom 15"
            });

            using (FileStream fs = File.Open(@"C:\TEMP\Test.txt", FileMode.Create, FileAccess.ReadWrite))
            {
                ConstantContactFileEngine.WriteToStream( fs, records );

                fs.Seek(0, SeekOrigin.Begin);

                StreamReader reader = new StreamReader(fs);
                string s = reader.ReadToEnd();
            }
        }
    }
}
