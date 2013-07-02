using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Web.Security;
using SPCASW.Common;
using SPCASW.Web.Models;
using SPCASW.Web.Security;

namespace SPCASW.Web.Controllers
{
   [MustBeInRole(UserRoles.Admin)]
   public class UsersController : BaseController
   {
      private IEnumerable<UserViewModel> GetAllUsersViewModel()
      {
         var allUsers = Membership.GetAllUsers();
         return (from MembershipUser mUser in allUsers
                 orderby mUser.IsApproved descending
                 select
                    new UserViewModel
                       {
                          Username = mUser.UserName,
                          Email = mUser.Email,
                          IsApproved = mUser.IsApproved,
                          Roles = Roles.GetRolesForUser(mUser.UserName)
                       });
      }

      [HttpGet]
      public ActionResult Index()
      {
         return View(GetAllUsersViewModel());
      }

      [HttpGet]
      public ActionResult All()
      {
         return PartialView("All", GetAllUsersViewModel());
      }

      [HttpGet]
      public ActionResult Edit(string username)
      {
         var membershipUser = Membership.GetUser(username);
         if (membershipUser == null)
         {
            throw new Exception("That user does not exist");
         }
         return
            PartialView(new UserViewModel
               {
                  Username = username,
                  Email = membershipUser.Email,
                  IsApproved = membershipUser.IsApproved,
                  Roles = Roles.GetRolesForUser(username)
               });
      }


      [HttpPost]
      public ActionResult Save(UserViewModel user)
      {
         var result = new JsonSuccess<UserViewModel> {Success = false};

         try
         {
            var mUser = Membership.GetUser(user.Username);

            if (mUser == null)
            {
               throw new Exception(string.Format("User {0} does not exist.", user.Username));
            }

            mUser.Email = user.Email;
            mUser.IsApproved = user.IsApproved;

            Membership.UpdateUser(mUser);

            var currentRoles = Roles.GetRolesForUser(user.Username);

            // remove the "false" values that MVC puts in the page for checkboxes
            user.Roles = user.Roles.Where(x => x != "false").ToArray();

            var rolesToRemove = currentRoles.Except(user.Roles).ToArray();
            var rolesToAdd = user.Roles.Except( currentRoles ).ToArray();

            var usernameArray = new[] {user.Username};

            if (rolesToRemove.Any())
            {
               Roles.RemoveUsersFromRoles(usernameArray, rolesToRemove);
            }

            if (rolesToAdd.Any())
            {
               Roles.AddUsersToRoles(usernameArray, rolesToAdd);
            }

            result.Success = true;
         }
         catch (Exception ex)
         {
            result.Message = ex.Message;
         }

         return Json(result);
      }

      [HttpPost]
      public ActionResult Unlock(string username)
      {
         bool result = false;
         var mUser = Membership.GetUser(username);
         if( mUser != null && mUser.IsLockedOut )
         {
            result =  mUser.UnlockUser();
         }
         return Content(result ? string.Format("User {0} was unlocked", username) : string.Format("Unable to unlock user {0}", username));
      }

      [HttpPost]
      public ActionResult Delete(string username)
      {
         if (!string.IsNullOrEmpty(username) && Membership.GetUser(username) != null)
         {
            bool result = Membership.DeleteUser(username);
            return Content(result ? string.Format("User {0} deleted successfully", username) : string.Format("Unable to delete user {0}", username));
         }
         return Content(string.Format("User {0} does not exist", username));
      }
   }
}