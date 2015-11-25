﻿using System;
using System.Web.Mvc;
using TestApp.Models;
using TestApp.Services.Interfaces;

namespace TestApp.Controllers
{
    public class NewsController : Controller
    {
        private readonly INewsService _newsService;
        private readonly IUserService _userService;
        private readonly INewsTagService _newsTagService;
        private readonly INewsFileService _newsFileService;

        public NewsController(IUserService userService, INewsService newsService, INewsTagService newsTagService, INewsFileService newsFileService)
        {
            _newsService = newsService;
            _newsTagService = newsTagService;
            _newsFileService = newsFileService;
            _userService = userService;

            ViewBag.IsAdmin = _userService.IsUserInRole("Admin");
        }



        public ActionResult List(string tag)
        {
            var model = String.IsNullOrEmpty(tag) ? _newsService.List() : _newsService.GetNewsByTag(tag);

            return View(model);
        }

        public ActionResult ListAdmin()
        {
            var model = _newsService.List();

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

        public ActionResult DeleteFileFromNew(int newsId, int fileId)
        {
            _newsFileService.RemoveMappingByNewsIdAndFileId(newsId,fileId);

            return RedirectToAction("Edit", new {id = newsId});
        }


        public ActionResult NewsTagsWidget()
        {
            var model = _newsTagService.GetNewsTagsList();
            return View(model);
        }
    }
}
