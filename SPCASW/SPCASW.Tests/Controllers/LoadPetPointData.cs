using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using SPCASW.PetPoint;
using System.IO;

namespace SPCASW.Tests.Controllers
{
    [TestClass]
    public class LoadPetPointData
    {
        [TestMethod]
        public void LoadData()
        {
            String lPath = Path.Combine("D:\\SPCASW\\DB\\Legacy Data\\PersonByAssociation.xml");
            XmlParser lParser = new XmlParser(lPath);
        }
    }
}
