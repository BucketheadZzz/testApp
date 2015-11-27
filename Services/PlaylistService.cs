using System;
using System.Collections.Generic;
using System.Linq;
using TestApp.DAL;
using TestApp.Extensions;
using TestApp.Models;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class PlaylistService: IPlaylistService
    {
        private readonly IRepository<Playlist> _playListRepository;
        private readonly IPlaylistFileService _playlistFileService;
        private readonly IPlaylistTagService _playlistTagService;

        public PlaylistService(IRepository<Playlist> playListRepository, IPlaylistFileService playlistFileService, IPlaylistTagService playlistTagService)
        {
            _playListRepository = playListRepository;
            _playlistFileService = playlistFileService;
            _playlistTagService = playlistTagService;
        }

        public int Add(PlaylistModel playlist)
        {

            var item = playlist.ToEntity();
            var added = _playListRepository.Insert(item);

            if (!String.IsNullOrEmpty(playlist.Tags))
            {
                _playlistTagService.SaveMapping(playlist.Tags, added.Id);
            }
            if (playlist.Files.Count > 0 & playlist.Files[0] != null)
            {
                _playlistFileService.AddMapping(playlist.Files, added.Id);
            }
            return added.Id;
        }

        public void Update(PlaylistModel playlist)
        {
            var entity = playlist.ToEntity();

            _playListRepository.Update(entity);

            if (!String.IsNullOrEmpty(playlist.Tags))
            {
                _playlistTagService.SaveMapping(playlist.Tags, entity.Id);
            }
            if (playlist.Files.Count > 0 & playlist.Files[0] != null)
            {
                _playlistFileService.AddMapping(playlist.Files, playlist.Id);
            }
        }

        public void Delete(int id)
        {
            var deletedItem = _playListRepository.Table.SingleOrDefault(x => x.Id == id);
            if (deletedItem != null)
            {
                _playlistTagService.RemoveMapping(id);
                _playlistFileService.RemoveMapping(id);
                _playListRepository.Delete(deletedItem);
            }
        }

        public PlaylistModel GetById(int id)
        {
            return _playListRepository.GetById(id).ToModel();
        }

        public IQueryable<Playlist> GetAll()
        {
            return _playListRepository.Table;
        }

        public IList<PlaylistModel> GetPlaylistModels()
        {
            return _playListRepository.Table.ToListModel();
        }
    }
}