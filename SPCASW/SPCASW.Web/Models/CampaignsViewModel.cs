using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SPCASW.Common;

namespace SPCASW.Web.Models
{
   public class CampaignsViewModel
   {
      public List<Campaign> Campaigns { get; set; }
      public List<Contact> Contacts { get; set; }
   }
}