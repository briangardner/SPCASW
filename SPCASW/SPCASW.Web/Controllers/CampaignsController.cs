using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SPCASW.Common;
using SPCASW.Data.Services;
using SPCASW.Marketing;
using SPCASW.Web.Converters;
using SPCASW.Web.Security;

namespace SPCASW.Web.Controllers
{
    [MustBeInRole(UserRoles.CampaignManager)]
    public class CampaignsController : BaseController
    {
        // GET: /Campaigns/
        private CampaignsService _campaignsService;

        public CampaignsController()
        {
            _campaignsService = new CampaignsService();
        }

        //
        // GET: /Campaigns/

        public ActionResult Index()
        {
			ViewData["users"] = Membership.GetAllUsers();
			var model = new SPCASW.Web.Models.CampaignsViewModel();
            model.Campaigns = GetCampaigns();

            return View(model);
        }

        private List<Campaign> GetCampaigns()
        {
            var list = _campaignsService.GetCampaigns();
            return list.ToList();
        }

        //
        // GET: /Campaigns/Details/5

        public ActionResult Details(int id)
        {
            var campaignService = new CampaignsService();
			var model = new SPCASW.Web.Models.CampaignViewModel();
            model.Campaign = campaignService.GetCampaign(id);
            model.Contacts = GetCurrentRecipients(model.Campaign);
            //model.CreatedByName = GetContactByGUID(model.Campaign.CreatedBy);
            return View(model);
        }

        private List<Contact> GetCurrentRecipients(Campaign campaign)
        {
            return campaign.CampaignRecipients.Select(o => o.Contact).ToList();
        }

        public ActionResult Create()
        {
            var model = new SPCASW.Web.Models.CampaignViewModel
                {
                    Contacts = GetContacts(),
                    DonarGroup = false,
                    AdopterGroup = false,
                    VolunteerGroup = false
                };

            return View(model);
        }

        //
        // POST: /Campaigns/Create

        [HttpPost]
        public ActionResult Create(Campaign campaign, FormCollection collection)
        {
            if(ModelState.IsValid && campaign != null)
            {
                var user = Membership.GetUser(User.Identity.Name);
                var recipients = GetRecipients(collection);
                recipients.ForEach(
                    o => campaign.CampaignRecipients.Add(new CampaignRecipient() {ContactID = o.ContactID}));

                campaign.CreatedOn = DateTime.Now;
                campaign.CreatedBy = (Guid)user.ProviderUserKey;
                var result = _campaignsService.Create(campaign);
                if(!result)
                {
                    TempData["Error"] = String.Format("Record for {0} cannot be created. Contact the administrator.",
                                                      campaign.CampaignName);
                    return RedirectToAction("Index");
                }
            }
            TempData["Info"] = String.Format("Record for {0} was created successfully.", campaign.CampaignName);
            return RedirectToAction("Details", campaign.CampaignID);
        }

        private List<Contact> GetRecipients(FormCollection collection)
        {
            var contactsService = new ContactsService();
            var allContacts = contactsService.GetContacts();

            var list = new List<Contact>();
            var contactList = new List<int>();
            foreach(string key in collection.AllKeys)
            {
                if(key.StartsWith("contact"))
                    contactList.Add(ParseOutContactID(key));
                if(key == "VolunteerGroup")
                    GetVolunteerContacts(list, allContacts);
                if(key == "DonarGroup")
                    GetDonarContacts(list, allContacts);
                if(key == "AdopterGroup")
                    GetAdopterContacts(list, allContacts);
            }
            foreach(int id in contactList)
            {
                list.Add(FindContactById(id));
            }
            return list;
        }

        private List<Contact> GetVolunteerContacts(List<Contact> recipients, List<Contact> allContacts)
        {
            recipients.AddRange(allContacts.Where(c => c.IsVolunteer == true));
            return recipients;
        }

        private List<Contact> GetDonarContacts(List<Contact> recipients, List<Contact> allContacts)
        {
            recipients.AddRange(allContacts.Where(c => c.IsDonor == true));
            return recipients;
        }

        private List<Contact> GetAdopterContacts(List<Contact> recipients, List<Contact> allContacts)
        {
            recipients.AddRange(allContacts.Where(c => c.IsAdopter == true));
            return recipients;
        }

        private Contact FindContactById(int id)
        {
            return GetContacts().FirstOrDefault(c => c.ContactID == id);
        }

        private int ParseOutContactID(string key)
        {
            var id = Convert.ToInt32(key.Substring(8));
            return id;
        }

        private List<Contact> GetContacts()
        {
            var contactsService = new ContactsService();
            return contactsService.GetContacts();
        }

        public ActionResult ExportCsv(int id)
        {
            Campaign campaign = _campaignsService.GetCampaign(id);
            if(campaign == null)
            {
                throw new Exception("No campaign found.");
            }

            using(var ms = new MemoryStream())
            {
                var converter = new ConstantContactConverter();
                var ccrecords = campaign.CampaignRecipients.Select(converter.ConvertFrom);

                ConstantContactFileEngine.WriteToStream(ms, ccrecords);

                ms.Seek(0, SeekOrigin.Begin);

                using(BinaryReader reader = new BinaryReader(ms))
                {
                    byte[] bytes = reader.ReadBytes((int)ms.Length);
                    return File(bytes, "text/csv", "Export.csv");
                }
            }
        }
    }
}
