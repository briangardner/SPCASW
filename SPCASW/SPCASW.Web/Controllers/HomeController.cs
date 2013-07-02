using System.Web.Mvc;

namespace SPCASW.Web.Controllers
{
   public class HomeController : BaseController
   {
      public ActionResult Index()
      {
         return RedirectToAction("Index", "Contacts");
      }
   }
}
