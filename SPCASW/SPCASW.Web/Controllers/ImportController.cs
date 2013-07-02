using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using SPCASW.PetPoint;
using SPCASW.Web.Models;
using SPCASW.Web.Security;
using Kendo.Mvc.UI;
using Kendo.Mvc.Extensions;
using SPCASW.Common;

namespace SPCASW.Web.Controllers
{
    [MustBeInRole(UserRoles.Admin)]
    public class ImportController : Controller
    {
        [HttpGet]
        public ActionResult ImportFile()
        {
            return View();
        } 

        [HttpPost]
        public ActionResult ImportFile(ImportFileModel fileModel)
        {
            if (ModelState.IsValid)
            {
                if (fileModel != null)
                {
                    if (fileModel.File != null)
                    {
                        string filePath = SaveUpload(fileModel);

                        XmlParser parse = new XmlParser(filePath);

                        //Display which files had issues importing in the database; isMainAddressValid(?) = false
						TempData["ViewBag.Title"] = "Success! The file has been uploaded and imported.";
                        return RedirectToAction("InvalidMailingAddresses");

                    }

                }
            }
            return View();
        }

		public ActionResult InvalidMailingAddresses()
		{
			return View();
		}

		public virtual ActionResult ImportFileSuccess_Select([DataSourceRequest] DataSourceRequest request)
		{
			using (var context = new SPCAContactsEntities())
			{
				return Json(context.Contacts
					.Where(c => !c.IsMailAddressValid)
					.Select(c => new Import_ContactViewModel { 
						ContactID = c.ContactID,
						FirstName = c.FirstName,
						LastName = c.LastName,
						Email = c.EmailAddress,
						Zip = c.PostalCode
					})
					.ToDataSourceResult(request));
			}
		}

        #region Private Methods
        private Boolean CreateDirectory(string filePath)
        {
            bool Result = true;

            //if the directory doesn't exist, create it
            if (!Directory.Exists(filePath))
            {
                Directory.CreateDirectory(filePath);
            }

            return Result;
        }

        private string SaveUpload(ImportFileModel fileModel)
        {
            var fileName = Path.GetFileName(fileModel.File.FileName);
            var filePath = ConfigurationManager.AppSettings["ImportFileLocation"];

            CreateDirectory(filePath);

            string path = Path.Combine(Server.MapPath(filePath), fileName);
            fileModel.File.SaveAs(path);

            return path;
        }

        #endregion

    }
}
