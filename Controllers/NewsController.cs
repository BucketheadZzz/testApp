using System;
using System.Web.Mvc;
using TestApp.Models;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IUserService _userService;
        private readonly IFileService<NewsFileMapping> _fileMappingService;
        private readonly ITagService<NewsTagMapping> _tagService; 

        public NewsController(IUserService userService, INewsService newsService, IFileService<NewsFileMapping> fileMappingService, ITagService<NewsTagMapping> tagService)
        {
            _newsService = newsService;
            _fileMappingService = fileMappingService;
            _tagService = tagService;
            _userService = userService;

            ViewBag.IsAdmin = _userService.IsUserInRole("Admin");
        }



        public ActionResult List(string tag)
        {
            var model = String.IsNullOrEmpty(tag) ? _newsService.GetModels() : _newsService.GetModelsByTag(tag);

            return View(model);
        }

        public ActionResult ListAdmin()
        {
            var model = _newsService.GetModels();

            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            var model = id != null ? _newsService.GetById((int)id) : new NewsModel();
            if (model == null)
            {
                return RedirectToAction("ListAdmin");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(NewsModel model)
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

        public ActionResult DeleteFileFromNew(int mappingId, int newsId, int fileId)
        {
            var removedMapping = new NewsFileMapping() { Id = mappingId, ObjectId = newsId, FileId = fileId };
            _fileMappingService.RemoveMapping(removedMapping);

            return RedirectToAction("Edit", new { id = newsId });
        }


        public ActionResult NewsTagsWidget()
        {
            var model = _tagService.TagsWidget();
            return View(model);
        }
    }
}
