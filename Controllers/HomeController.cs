using System.Web.Mvc;
using TestApp.Services.Interfaces;

namespace TestApp.Controllers
{
    public class HomeController : Controller
    {

        public HomeController()
        {
            
        }

        public HomeController(IUserService userService)
        {
            ViewBag.IsAdmin = userService.IsUserInRole("Admin");
        }

        public ActionResult Index()
        {
            return View("Index");
        }

    }
}
