using System.ComponentModel.DataAnnotations;

namespace SPCASW.Common
{
    [MetadataType(typeof(Campaign.CampaignMetadata))]
    public partial class Campaign
    {
        private class CampaignMetadata
        {
            private CampaignMetadata()
            {
            }

            [ScaffoldColumn(false)]
            public int CampaignID { get; set; }
        }
    }

    [MetadataType(typeof(Contact.ContactMetadata))]
    public partial class Contact
    {
        private class ContactMetadata
        {
            private ContactMetadata()
            {
            }

            [ScaffoldColumn(false)]
            public int ContactID { get; set; }

            [Display(Name="First Name")]
            public string FirstName { get; set; }

            [Display(Name = "Last Name")]
            public string LastName { get; set; }

            [Display(Name = "Address 1")]
            public string StreetAddress1 { get; set; }

            [Display(Name = "Address 2")]
            public string StreetAddress2 { get; set; }

            [Display(Name = "State")]
            public string StateCode { get; set; }

            [Display(Name = "Postal Code")]
            public string PostalCode { get; set; }

            [Display(Name = "Email")]
            public string EmailAddress { get; set; }

            [Display(Name = "Phone 1")]
            public string Phone1 { get; set; }

            [Display(Name = "Phone 2")]
            public string Phone2 { get; set; }

            [Display(Name = "Phone 3")]
            public string Phone3 { get; set; }

            [Display(Name = "Phone 4")]
            public string Phone4 { get; set; }
        }
    }
}