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
        private readonly IFileMappingService<PlayListFileMapping> _playlistFileService;

        public PlaylistController(IPlaylistService playlistService, IUserService userService, IFileMappingService<PlayListFileMapping> playlistFileService)
        {
            _playlistService = playlistService;
            _userService = userService;
            _playlistFileService = playlistFileService;


            ViewBag.IsAdmin = _userService.IsUserInRole("Admin");
        }


        public ActionResult List(string tag)
        {
            var model = String.IsNullOrEmpty(tag) ? _playlistService.GetPlaylistModels() : new object();

            return View(model);
        }

        public ActionResult ListAdmin()
        {
            var model = _playlistService.GetPlaylistModels();

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
            var removeMapped = new PlayListFileMapping() {FileId = fileId, PlaylistId = playlistId, Id = mappingId};
            _playlistFileService.RemoveMapping(removeMapped);

            return RedirectToAction("Edit","Playlist", new {id = playlistId});
        }

    }
}
