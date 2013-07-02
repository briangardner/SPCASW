using System.Linq;
using System.Web.Mvc;
using System.Web.Routing;

namespace SPCASW.Web.Security
{
	public class MustBeInRoleAttribute : FilterAttribute, IAuthorizationFilter
	{
		private readonly string[] _roleNames;

		public MustBeInRoleAttribute(params string[] roleNames)
		{
            this._roleNames = roleNames;
		}

		public void OnAuthorization(AuthorizationContext filterContext)
		{
            if (!filterContext.HttpContext.User.IsInRole(UserRoles.Admin) && !_roleNames.Any(o => filterContext.HttpContext.User.IsInRole(o)))
            {
                filterContext.Result = new RedirectToRouteResult(new RouteValueDictionary(new { controller = "Account", action = "NotAuthorized" }));
            }
		}
	}
}