using System;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using Kendo.Mvc.Extensions;
using Kendo.Mvc.UI;
using SPCASW.Common;
using SPCASW.Data.Services;
using SPCASW.Web.Models;
using SPCASW.Web.Security;

namespace SPCASW.Web.Controllers
{
    public class ContactsController : BaseController
    {
        //
        // GET: /Contacts/
        private ContactsService _contactsService;

        public ContactsController()
        {
            _contactsService = new ContactsService();
        }

        public ActionResult Index()
        {
            return View();
        }

        public virtual ActionResult Select([DataSourceRequest] DataSourceRequest request)
        {
            using(var context = new SPCAContactsEntities())
            {
                return Json(context.Contacts
                    .Select(Contacts_ContactViewModel.Initializer)
                    .ToDataSourceResult(request));
            }
        }

        public ActionResult Details(int id)
        {
            ViewBag.ReturnUrl = Request.UrlReferrer != null ? Request.UrlReferrer.ToString() : Url.Action("Index");

            var model = _contactsService.GetContact(id);
            if(User.IsInRole(UserRoles.ContactEditor) || User.IsInRole(UserRoles.Admin))
            {
                return View("DetailsEditable", model);
            }
            return View(model);
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Update([DataSourceRequest] DataSourceRequest request, Contacts_ContactViewModel viewModel)
        {
            try
            {
                if(ModelState.IsValid && viewModel != null)
                {
                    using(var context = new SPCAContactsEntities())
                    {
                        var model = context.Contacts.FirstOrDefault(c => c.ContactID == viewModel.ContactID);
                        if(model != null)
                        {
                            viewModel.UpdateModel(model);
                            model.ModifiedBy = (Guid)Membership.GetUser().ProviderUserKey;
                            context.SaveChanges();
                        }
                    }
                }
                return Json(ModelState.ToDataSourceResult());
            }
            catch(Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return Content(e.Message);
            }
        }

        [HttpPost]
        public ActionResult Save(Contact model)
        {
            if(ModelState.IsValid && model != null)
            {
                var user = Membership.GetUser(User.Identity.Name);

                if(user != null && user.ProviderUserKey != null)
                {
                    model.ModifiedBy = (Guid)user.ProviderUserKey;
                }

                var result = _contactsService.Update(model);
                if(!result)
                {
                    TempData["Error"] = String.Format("Record for {0} {1} cannot be updated. Contact the administrator.", model.FirstName, model.LastName);
                    return RedirectToAction("Index");
                }
            }
            TempData["Info"] = String.Format("Record for {0} {1} was update successfully", model.FirstName, model.LastName);
            return RedirectToAction("Index");
        }

        [AcceptVerbs(HttpVerbs.Post)]
        public ActionResult Create([DataSourceRequest] DataSourceRequest request, Contacts_ContactViewModel viewModel)
        {
            try
            {
                if(ModelState.IsValid && viewModel != null)
                {
                    using(var context = new SPCAContactsEntities())
                    {
                        var model = context.Contacts.CreateObject();
                        viewModel.UpdateModel(model);
                        model.CreatedBy = (Guid)Membership.GetUser().ProviderUserKey;
                        model.ModifiedBy = (Guid)Membership.GetUser().ProviderUserKey;
                        context.Contacts.AddObject(model);
                        context.SaveChanges();
                        viewModel.CopyModel(model);
                    }
                }
                return Json(new[] { viewModel }.ToDataSourceResult(request, ModelState));
            }
            catch(Exception e)
            {
                Response.StatusCode = (int)System.Net.HttpStatusCode.InternalServerError;
                return Content(e.Message);
            }
        }
    }
}
