using System;
using System.Collections.Generic;
using System.Linq;
using System.ComponentModel.DataAnnotations;
using System.Globalization;
using System.Web;
using SPCASW.Common;

namespace SPCASW.Web.Models
{
   public class CampaignViewModel
   {
      public Campaign Campaign { get; set;}

      //public string CreatedByName { get; }

      //private DateTime _createdOn;

      //public string CreatedOnForDisplay
      //{
      //   get
      //   {
      //      DateTime.TryParseExact(Campaign.CreatedOn.ToString(),
      //                   "yyyy-dd-MM hh:mm tt",
      //                   CultureInfo.InvariantCulture,
      //                   DateTimeStyles.None,
      //                   out _createdOn);
            
      //      return _createdOn.ToString("d");
      //   }
      //}

      public bool VolunteerGroup {get; set;}
      public bool DonarGroup { get; set; }
      public bool AdopterGroup { get; set; }

      public List<Contact> Contacts { get; set; }
      public List<Contact> VolunteerGroupContacts {get; set;}
      public List<Contact> DonarGroupContacts { get; set; }
      public List<Contact> AdopterGroupContacts { get; set; }
   }
}