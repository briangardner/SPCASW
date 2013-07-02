using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using SPCASW.Data.Repositories;
using SPCASW.Common;

namespace SPCASW.Data.Services
{
    public class CampaignsService
    {
        public ICampaignRepository _repository;

        public CampaignsService()
        {
            _repository = new CampaignRepository();
        }

        public List<Campaign> GetCampaigns()
        {

            return _repository.GetCampaigns();
        }

        public Campaign GetCampaign(int campaignID)
        {
            return _repository.GetCampaign(campaignID);
        }

        public bool Update(Campaign campaign)
        {
           return _repository.Update(campaign);
        }

        public bool Create(Campaign campaign)
        {
           return _repository.Create(campaign);
        }

    }
}