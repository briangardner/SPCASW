using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace SPCASW.Web.Models
{
   public class UserViewModel
   {
      [Required]
      public string Username { get; set; }

      [Required]
      public string Email { get; set; }

      [Required]
      [DataType(DataType.Password)]
      public string Password { get; set; }

      public IEnumerable<string> Roles { get; set; }

      public bool IsLockedOut { get; set; }

      public bool IsApproved { get; set; }
   }
}