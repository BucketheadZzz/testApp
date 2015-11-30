using System;
using System.Web.Mvc;
using TestApp.Models;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;
        private readonly IUserService _userService;
        private readonly IFileService<PlayListFileMapping> _playlistFileService;
        private readonly ITagService<PlayListTagMapping> _tagService; 

        public PlaylistController(IPlaylistService playlistService, IUserService userService, IFileService<PlayListFileMapping> playlistFileService, ITagService<PlayListTagMapping> tagService)
        {
            _playlistService = playlistService;
            _userService = userService;
            _playlistFileService = playlistFileService;
            _tagService = tagService;


            ViewBag.IsAdmin = _userService.IsUserInRole("Admin");
        }


        public ActionResult List(string tag)
        {
            var model = String.IsNullOrEmpty(tag) ? _playlistService.GetModels() : _playlistService.GetModelsByTag(tag);

            return View(model);
        }

        public ActionResult ListAdmin()
        {
            var model = _playlistService.GetModels();

            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            var model = id != null ? _playlistService.GetById((int) id) : new PlaylistModel();
            if (model == null)
            {
                return RedirectToAction("ListAdmin", "Playlist");
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PlaylistModel model)
        {
            if (model != null & ModelState.IsValid)
            {
                if (model.Id == 0)
                {
                    model.CreatedBy = _userService.GetCurrentUserId();
                    _playlistService.Add(model);
                }
                else
                {
                    _playlistService.Update(model);
                }
                return RedirectToAction("ListAdmin", "Playlist");
            }

            return View(model);
        }


        public ActionResult Delete(int id)
        {
            _playlistService.Delete(id);

            return RedirectToAction("ListAdmin","Playlist");
        }

        public ActionResult DeleteFileFromPlaylist(int mappingId, int playlistId, int fileId)
        {
            var removeMapped = new PlayListFileMapping() {FileId = fileId, ObjectId = playlistId, Id = mappingId};
            _playlistFileService.RemoveMapping(removeMapped);

            return RedirectToAction("Edit","Playlist", new {id = playlistId});
        }


        public ActionResult PlaylistsTagsWidget()
        {
            var model = _tagService.TagsWidget();
            return View(model);
        }
        public ActionResult GetAudioFile(int fileId)
        {
            var file = _playlistFileService.GetById(fileId);
            if (file != null)
            {
                return File(file.BinaryData, file.ContentType, file.FileName);
            }
            return null;
        }
    }
}
