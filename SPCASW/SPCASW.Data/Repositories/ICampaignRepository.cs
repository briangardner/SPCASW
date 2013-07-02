using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPCASW.Common;

namespace SPCASW.Data.Repositories
{
   public interface ICampaignRepository
   {
      List<Campaign> GetCampaigns();
      Campaign GetCampaign(int campaignID);
      bool Update(Campaign campaign);
      bool Create(Campaign campaign);

   }
}
