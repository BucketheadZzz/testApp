using System;
using System.Web.Mvc;
using TestApp.Models;
using TestApp.Services;

namespace TestApp.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IUserService _userService;

        public NewsController(IUserService userService, INewsService newsService)
        {
            _newsService = newsService;
            _userService = userService;

            ViewBag.IsAdmin = _userService.IsUserInRole("Admin");
        }

        public ActionResult List()
        {
            var model = _newsService.List();

            return View(model);
        }

        public ActionResult ListAdmin()
        {
            var model = _newsService.List();

            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            var model = id != null ? _newsService.GetById((int)id) : new News();
            if (model == null)
            {
                return RedirectToAction("ListAdmin");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(News model)
        {
            if (model != null & ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    model.Created = DateTime.Now;
                    model.CreatedBy = _userService.GetCurrentUserId();
                    _newsService.Add(model);
                }
                else
                {
                    _newsService.Update(model);
                }
                return RedirectToAction("ListAdmin");
            }

            return View(model);
        }


        public ActionResult Delete(int id)
        {
            _newsService.Delete(id);

            return RedirectToAction("ListAdmin");
        }
    }
}
