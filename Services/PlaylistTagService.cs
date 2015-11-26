using System;
using System.Collections.Generic;
using System.Linq;
using TestApp.DAL;
using TestApp.Models;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class PlaylistTagService: IPlaylistTagService
    {

        private readonly IRepository<PlayListTagMapping> _listTagsMappingContext;
        private readonly ITagService _tagService;


        public PlaylistTagService(ITagService tagService, IRepository<PlayListTagMapping> listTagsMappingContext)
        {
            _tagService = tagService;
            _listTagsMappingContext = listTagsMappingContext;
    
        }


        public void SaveMapping(string tags, int playlistId)
        {
            var splitedTags = tags.Split(',');

            var tagsToDelete = GetExistingTags(playlistId).Where(existTag => !splitedTags.Contains(existTag.Name)).
                Select(tag => _listTagsMappingContext.Table.SingleOrDefault(x => x.PlaylistId == playlistId && x.TagId == tag.Id)).
                Where(deletedMapping => deletedMapping != null).
                ToList();;

            if (tagsToDelete.Any())
            {
                _listTagsMappingContext.Delete(tagsToDelete);
            }

            var listMapping = (from tag in splitedTags 
                               select _tagService.Add(tag) into addedId
                               where !AlreadyMapped(addedId, playlistId) 
                               select new PlayListTagMapping { PlaylistId = playlistId, TagId = addedId}).ToList();


            _listTagsMappingContext.Insert(listMapping);
        }

        public void RemoveMapping(int playlistId)
        {
            var removedList = _listTagsMappingContext.Table.Where(x => x.PlaylistId == playlistId);
            _listTagsMappingContext.Delete(removedList);
        }

        public void RemoveMappingByTagId(int tagId)
        {
            var tags = _listTagsMappingContext.Table.Where(x => x.TagId == tagId);
            _listTagsMappingContext.Delete(tags);

        }

        private IEnumerable<Tag> GetExistingTags(int playlistId)
        {
            return (from tag in _tagService.GetAll()
                    join tagMapping in _listTagsMappingContext.Table
                        on tag.Id equals tagMapping.TagId
                    where tagMapping.PlaylistId == playlistId
                    select tag).ToList();
        }


        public string GetTagsByPlayListId(int playlistId)
        {
            var tagList = (from tag in _tagService.GetAll()
                           join tagMapping in _listTagsMappingContext.Table
                               on tag.Id equals tagMapping.TagId
                           where tagMapping.PlaylistId == playlistId
                           select tag.Name).ToList();

            return tagList.Count > 0 ? string.Join(",", tagList) : String.Empty;
        }

        public bool AlreadyMapped(int tagId, int playlistId)
        {
            return _listTagsMappingContext.Table.Any(x => x.PlaylistId == playlistId && x.TagId == tagId);
        }

       

    }
}