using System.Linq;
using System.Web.Mvc;
using TestApp.DAL;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Controllers
{
    public class HomeController : Controller
    {


        public HomeController(IUserService userService)
        {
     
            ViewBag.IsAdmin = userService.IsUserInRole("Admin");
        }

        public ActionResult Index()
        {
            return View();
        }

    }
}
