namespace SPCASW.Web.Security
{
   public static class UserRoles
   {
      public const string Admin = "admin";
      public const string CampaignManager = "campaignManager";
      public const string ContactEditor = "contactEditor";
      public const string ReadOnly = "readOnly";

      public static string FriendlyName( string role )
      {
         switch( role )
         {
            case Admin:
               return "Administrator";
               
            case CampaignManager:
               return "Campaign Manager";
               
            case ContactEditor:
               return "Contact Editor";
               
            case ReadOnly:
               return "Read Only";

            default:
               return role;
         }
      }
   }
}