using System;
using System.Collections.Generic;
using System.Linq;
using TestApp.DAL;
using TestApp.Models;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class NewsTagService: INewsTagService
    {

        private readonly IRepository<NewsTagMapping> _newsTagsMappingContext;
        private readonly ITagService _tagService;
  

        public NewsTagService( ITagService tagService,  IRepository<NewsTagMapping> newsTagsMappingContext)
        {
            _tagService = tagService;
            _newsTagsMappingContext = newsTagsMappingContext;
        }


        public void SaveTagsMapping(string tags, int newsId)
        {
            var splitedTags = tags.Split(',');

            var tagsToDelete = GetExistingNewsTags(newsId).Where(existTag => !splitedTags.Contains(existTag.Name)).
                Select(tag => _newsTagsMappingContext.Table.SingleOrDefault(x => x.NewsId == newsId && x.TagId == tag.Id)).
                Where(deletedMapping => deletedMapping != null).
                ToList();;

            if (tagsToDelete.Any())
            {
                _newsTagsMappingContext.Delete(tagsToDelete);
            }

            var listMapping = (from tag in splitedTags 
                               select _tagService.Add(tag) into addedId
                               where !AlreadyMapped(addedId, newsId) 
                               select new NewsTagMapping {NewsId = newsId, TagId = addedId}).ToList();


            _newsTagsMappingContext.Insert(listMapping);
        }

        public void RemoveMapping(int newsId)
        {
            var removedList = _newsTagsMappingContext.Table.Where(x => x.NewsId == newsId);
            _newsTagsMappingContext.Delete(removedList);
        }


        public void RemoveMappingByTagId(int tagId)
        {
            var tags = _newsTagsMappingContext.Table.Where(x => x.TagId == tagId);
            _newsTagsMappingContext.Delete(tags);

        }


        public IList<TagWidgetModel> GetNewsTagsList()
        {
            return (from tag in _tagService.GetAll()
                join tagMap in _newsTagsMappingContext.Table
                    on tag.Id equals tagMap.TagId
                group tag by tag.Name into pg
                let tagNumber = pg.Count()
                select new TagWidgetModel {TagName = pg.Key, Count = tagNumber}).ToList();

        }


        private IEnumerable<Tag> GetExistingNewsTags(int newsId)
        {
            return (from tag in _tagService.GetAll()
                    join tagMapping in _newsTagsMappingContext.Table
                        on tag.Id equals tagMapping.TagId
                    where tagMapping.NewsId == newsId
                    select tag).ToList();
        }


        public string GetTagsByNewsId(int newsId)
        {
            var tagList = (from tag in _tagService.GetAll()
                           join tagMapping in _newsTagsMappingContext.Table
                               on tag.Id equals tagMapping.TagId
                           where tagMapping.NewsId == newsId
                           select tag.Name).ToList();

            return tagList.Count > 0 ? string.Join(",", tagList) : String.Empty;
        }

        public bool AlreadyMapped(int tagId, int newsId)
        {
            return _newsTagsMappingContext.Table.Any(x => x.NewsId == newsId && x.TagId == tagId);
        }

      
    }
}