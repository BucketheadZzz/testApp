using System;
using System.Linq;
using System.Web.Mvc;
using TestApp.Extensions;
using TestApp.Models;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Controllers
{
    public class PlaylistController : Controller
    {
        private readonly IPlaylistService _playlistService;
        private readonly IUserService _userService;
        private readonly IFileService<PlayListFileMapping> _fileService;
        private readonly ITagService<PlayListTagMapping> _tagService; 

        public PlaylistController(IPlaylistService playlistService, IUserService userService, IFileService<PlayListFileMapping> fileService, ITagService<PlayListTagMapping> tagService)
        {
            _playlistService = playlistService;
            _userService = userService;
            _fileService = fileService;
            _tagService = tagService;


            ViewBag.IsAdmin = _userService.IsUserInRole("Admin");
        }


        public ActionResult List(string tag)
        {
            var model = string.IsNullOrEmpty(tag) ? _playlistService.GetModels() : _playlistService.GetModelsByTag(tag);

            return View(model);
        }

        public ActionResult ListAdmin()
        {
            var model = _playlistService.GetModels();

            return View(model);
        }

        public ActionResult Edit(int? id)
        {
            PlaylistModel model;
            if (id != null)
            {
                var entity = _playlistService.GetById((int) id);
                model = entity.ToModel();
                model.Tags = string.Join(",", _tagService.GetTagsByMapping((int) id).Select(x => x.Name));
            }
            else
            {
                model = new PlaylistModel();
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult Edit(PlaylistModel model)
        {
            if (model != null & ModelState.IsValid)
            {

                var entity = model.ToEntity();
                if (model.Id == 0)
                {
                    model.CreatedBy = _userService.GetCurrentUserId();
                    _playlistService.Add(entity);
                }
                else
                {
                    _playlistService.Update(entity);
                }

                if (!string.IsNullOrEmpty(model.Tags))
                {
                    var tagMapping = _tagService.PrepareTagMappingCollection(_tagService.Add(model.Tags.Split(',')), entity.Id);
                    _tagService.AddMapping(tagMapping);
                }

                if (model.Files.Count > 0 & model.Files[0] != null)
                {
                    var mapping = _fileService.PrepareFileMappingCollection(_fileService.Add(model.Files), model.Id);
                    _fileService.AddMapping(mapping);
                }

                return RedirectToAction("ListAdmin", "Playlist");
            }

            return View(model);
        }


        public ActionResult Delete(int id)
        {
            _tagService.RemoveMapping(id);
            _playlistService.Delete(id);

            return RedirectToAction("ListAdmin", "Playlist");
        }

        public ActionResult DeleteFileFromPlaylist(int mappingId, int playlistId, int fileId)
        {
            var removeMapped = new PlayListFileMapping { FileId = fileId, ObjectId = playlistId, Id = mappingId };
            _fileService.RemoveMapping(removeMapped);

            return RedirectToAction("Edit", "Playlist", new { id = playlistId });
        }


        public ActionResult PlaylistsTagsWidget()
        {
            var model = _tagService.TagsWidget();
            return View(model);
        }
        public ActionResult GetAudioFile(int fileId)
        {
            var file = _fileService.GetById(fileId);
            if (file != null)
            {
                return File(file.BinaryData, file.ContentType, file.FileName);
            }
            return null;
        }
    }
}
