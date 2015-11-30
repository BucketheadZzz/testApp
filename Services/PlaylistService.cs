using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        private readonly IFileService<PlayListFileMapping> _fileService;
        private readonly ITagService<PlayListTagMapping> _tagService;

        public PlaylistService(IRepository<Playlist> playListRepository, IFileService<PlayListFileMapping> fileService, ITagService<PlayListTagMapping> tagService)
        {
            _playListRepository = playListRepository;
            _fileService = fileService;
            _tagService = tagService;
        }


        public int Add(PlaylistModel playlist)
        {

            var item = playlist.ToEntity();
            var added = _playListRepository.Insert(item);

            if (!String.IsNullOrEmpty(playlist.Tags))
            {
                var tagMapping = PrepareTagMappingCollection(_tagService.Add(playlist.Tags.Split(',')), added.Id);
                _tagService.AddMapping(tagMapping);
            }
            if (playlist.Files.Count > 0 & playlist.Files[0] != null)
            {
               var mapping = PrepareMappingCollection(_fileService.Add(playlist.Files),added.Id);
               _fileService.AddMapping(mapping);
            }
            return added.Id;
        }

        public void Update(PlaylistModel playlist)
        {
            var entity = playlist.ToEntity();

            _playListRepository.Update(entity);

            if (!String.IsNullOrEmpty(playlist.Tags))
            {
                var tagMapping = PrepareTagMappingCollection(_tagService.Add(playlist.Tags.Split(',')), playlist.Id);
                _tagService.AddMapping(tagMapping);
            }
            if (playlist.Files.Count > 0 & playlist.Files[0] != null)
            {
                var mapping = PrepareMappingCollection(_fileService.Add(playlist.Files), playlist.Id);
                _fileService.AddMapping(mapping);
            }
        }

        public void Delete(int id)
        {
            var deletedItem = _playListRepository.Table.SingleOrDefault(x => x.Id == id);
            if (deletedItem != null)
            {
                _tagService.RemoveMapping(id);
                _fileService.Delete(id);
                _playListRepository.Delete(deletedItem);
            }
        }

        public PlaylistModel GetById(int id)
        {
            var enity = _playListRepository.GetById(id);
            if (enity != null)
            {
                var model = enity.ToModel();
                model.Tags = String.Join(",", _tagService.GetTagsByMapping(id).Select(x => x.Name));
                return model;
            }
            return new PlaylistModel();
        }

        public IQueryable<Playlist> GetAll()
        {
            return _playListRepository.Table;
        }

        public IList<PlaylistModel> GetModelsByTag(string tag)
        {
            return
            (from playList in _playListRepository.Table
             where playList.PlayListTagMapping.Count(x => x.Tag.Name == tag) > 0
             select playList).ToListModel();
        }

        public IList<PlaylistModel> GetModels()
        {
            return _playListRepository.Table.ToListModel();
        }


        private ICollection<PlayListTagMapping> PrepareTagMappingCollection(IEnumerable<Tag> tags, int newsId)
        {
            var resList = new Collection<PlayListTagMapping>();
            foreach (var tag in tags)
            {
                resList.Add(new PlayListTagMapping() { ObjectId = newsId, TagId = tag.Id });
            }
            return resList;
        } 

        private ICollection<PlayListFileMapping> PrepareMappingCollection(IEnumerable<File> files, int playListId)
        {
            var resList = new Collection<PlayListFileMapping>();
            foreach (var file in files)
            {
                resList.Add(new PlayListFileMapping(){FileId =  file.Id, ObjectId = playListId});
            }
            return resList;
        } 
    }
}