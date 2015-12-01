using System;
using System.Linq;
using System.Web.Mvc;
using TestApp.Extensions;
using TestApp.Models;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IUserService _userService;
        private readonly IFileService<NewsFileMapping> _fileService;
        private readonly ITagService<NewsTagMapping> _tagService;

        public NewsController()
        {
            
        }

        public NewsController(IUserService userService, INewsService newsService, IFileService<NewsFileMapping> fileService, ITagService<NewsTagMapping> tagService)
        {
            _newsService = newsService;
            _fileService = fileService;
            _tagService = tagService;
            _userService = userService;

            ViewBag.IsAdmin = _userService.IsUserInRole("Admin");
        }



        public ActionResult List(string tag = null)
        {
            var model = string.IsNullOrEmpty(tag) ? _newsService.GetModels() : _newsService.GetModelsByTag(tag);

            return View(model);
        }

        public ActionResult ListAdmin()
        {
            var model = _newsService.GetModels();

            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            NewsModel model;
            if (id != null)
            {
                var entity = _newsService.GetById((int) id);
                model = entity.ToModel();
                model.Tags = string.Join(",",_tagService.GetTagsByMapping(model.Id).Select(x => x.Name));
            }
            else
            {
                model = new NewsModel();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(NewsModel model)
        {
            if (model != null & ModelState.IsValid)
            {
                var entity = model.ToEntity();
                if (model.Id == 0)
                {
                    model.Created = DateTime.Now;
                    model.CreatedBy = _userService.GetCurrentUserId();

                    
                    _newsService.Add(entity);
                }
                else
                {
                    _newsService.Update(entity);
                }

                if (!string.IsNullOrEmpty(model.Tags))
                {
                    var tagMapping = _tagService.PrepareTagMappingCollection(_tagService.Add(model.Tags.Split(',')), entity.Id);
                    _tagService.AddMapping(tagMapping);
                }
                if (model.Files.Count > 0 & model.Files[0] != null)
                {
                    var fileMapping = _fileService.PrepareFileMappingCollection(_fileService.Add(model.Files), entity.Id);
                    _fileService.AddMapping(fileMapping);
                }

                return RedirectToAction("ListAdmin");
            }

            return View(model);
        }


        public ActionResult Delete(int id)
        {
            _tagService.RemoveMapping(id);
            _newsService.Delete(id);

            return RedirectToAction("ListAdmin");
        }

        public ActionResult DeleteFileFromNew(int mappingId, int newsId, int fileId)
        {
            var removedMapping = new NewsFileMapping() { Id = mappingId, ObjectId = newsId, FileId = fileId };
            _fileService.RemoveMapping(removedMapping);

            return RedirectToAction("Edit", new { id = newsId });
        }


        public ActionResult NewsTagsWidget()
        {
            var model = _tagService.TagsWidget();
            return View(model);
        }
    }
}
