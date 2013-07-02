using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPCASW.Common;

namespace SPCASW.Data.Repositories
{
   public class CampaignRepository : ICampaignRepository
   {
      public CampaignRepository()
      {
      }

      public List<Campaign> GetCampaigns()
      {
         var list = new List<Campaign>();
         using (var db = new SPCAContactsEntities())
         {
            var result = (from campaigns in db.Campaigns.Include("CampaignRecipients") select campaigns).ToList();
            foreach (var dbcampaign in result)
            {
               list.Add(dbcampaign);
            }
            return list;
         }
      }

      public Campaign GetCampaign(int campaignID)
      {
          using (var db = new SPCAContactsEntities())
          {
              return (from campaigns in db.Campaigns.Include("CampaignRecipients").Include("CampaignRecipients.Contact")
                      where campaigns.CampaignID == campaignID
                      select campaigns).FirstOrDefault();
          }
      }

      public bool Update(Campaign campaign)
      {
         try
         {
            using (var db = new SPCAContactsEntities())
            {
               var result = (from campaigns in db.Campaigns where campaign.CampaignID == campaign.CampaignID select campaigns).FirstOrDefault();
               result.CampaignName = campaign.CampaignName;
               result.CreatedBy = campaign.CreatedBy;
               result.CreatedOn = campaign.CreatedOn;
               db.SaveChanges();

               return true;
            }
         }
         catch (Exception e)
         {
            return false;
         }

      }


      public bool Create(Campaign campaign)
      {
         try
         {
            using (var db = new SPCAContactsEntities())
            {
               db.AddToCampaigns(campaign);
               db.SaveChanges();
               return true;
            }
         }
         catch (Exception e)
         {
            return false;
         }

      }

   }
}
