using System;
using System.Web.Mvc;
using TestApp.Models;
using TestApp.Services.Interfaces;

namespace TestApp.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;
        private readonly IUserService _userService;
        private readonly IPlaylistFileService _playlistFileService;

        public PlaylistController(IPlaylistService playlistService, IUserService userService, IPlaylistFileService playlistFileService)
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

        public ActionResult DeleteFileFromPlaylist(int playlistId, int fileId)
        {
            _playlistFileService.RemoveMapping(playlistId,fileId);

            return RedirectToAction("Edit","Playlist", new {id = playlistId});
        }


    }
}
