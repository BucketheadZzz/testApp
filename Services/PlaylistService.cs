using System.Collections.Generic;
using System.Linq;
using TestApp.DAL;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class PlaylistService : IPlaylistService
    {
        private readonly IRepository<Playlist> _playListRepository;

        public PlaylistService(IRepository<Playlist> playListRepository)
        {
            _playListRepository = playListRepository;
        }

        public void Add(Playlist playlist)
        {
            _playListRepository.Insert(playlist);
        }

        public void Update(Playlist playlist)
        {
            _playListRepository.Update(playlist);
        }

        public void Delete(int id)
        {
            var deletedItem = _playListRepository.Table.SingleOrDefault(x => x.Id == id);
            if (deletedItem != null)
            {
                _playListRepository.Delete(deletedItem);
            }
        }

        public Playlist GetById(int id)
        {
            var enity = _playListRepository.GetById(id);
            return enity;
        }

        public IList<Playlist> GetPlayListsByTag(string tag)
        {
            return
            (from playList in _playListRepository.Table
             where playList.PlayListTagMapping.Count(x => x.Tag.Name == tag) > 0
             select playList).ToList();
        }

        public IList<Playlist> GetPlayLists()
        {
            return _playListRepository.Table.ToList();
        }

    }
}