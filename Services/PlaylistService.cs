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
        private readonly IFileMappingService<PlayListFileMapping> _playlistFileMappingService;
        private readonly ITagMappingService<PlayListTagMapping> _playlistTagService;
        private readonly IFileService _fileService;
        private readonly ITagService _tagService;

        public PlaylistService(IRepository<Playlist> playListRepository,  IFileMappingService<PlayListFileMapping> playlistFileMappingService, IFileService fileService, IFileMappingService<PlayListFileMapping> playlistFileMappingService1, ITagMappingService<PlayListTagMapping> playlistTagService, ITagService tagService)
        {
            _playListRepository = playListRepository;
     
            _fileService = fileService;
            _playlistFileMappingService = playlistFileMappingService1;
            _playlistTagService = playlistTagService;
            _tagService = tagService;
        }

        public int Add(PlaylistModel playlist)
        {

            var item = playlist.ToEntity();
            var added = _playListRepository.Insert(item);

            if (!String.IsNullOrEmpty(playlist.Tags))
            {
                var tagMapping = PrepareTagMappingCollection(_tagService.Add(playlist.Tags.Split(',')), added.Id);
                _playlistTagService.AddMapping(tagMapping);
            }
            if (playlist.Files.Count > 0 & playlist.Files[0] != null)
            {
               var mapping = PrepareMappingCollection(_fileService.Add(playlist.Files),added.Id);
               _playlistFileMappingService.AddMapping(mapping);
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
                _playlistTagService.AddMapping(tagMapping);
            }
            if (playlist.Files.Count > 0 & playlist.Files[0] != null)
            {
                var mapping = PrepareMappingCollection(_fileService.Add(playlist.Files), playlist.Id);
                _playlistFileMappingService.AddMapping(mapping);
            }
        }

        public void Delete(int id)
        {
            var deletedItem = _playListRepository.Table.SingleOrDefault(x => x.Id == id);
            if (deletedItem != null)
            {
                _playlistTagService.RemoveMapping(id);
                _playListRepository.Delete(deletedItem);
            }
        }

        public PlaylistModel GetById(int id)
        {

            var enity = _playListRepository.GetById(id);
            if (enity != null)
            {
                var model = enity.ToModel();
                model.Tags = String.Join(",", _playlistTagService.GetTagsByMapping(id).Select(x => x.Name));

                return model;
            }
            return new PlaylistModel();
        }

        public IQueryable<Playlist> GetAll()
        {
            return _playListRepository.Table;
        }

        public IList<PlaylistModel> GetPlaylistModels()
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
                resList.Add(new PlayListFileMapping(){FileId =  file.Id, PlaylistId = playListId});
            }
            return resList;
        } 
    }
}