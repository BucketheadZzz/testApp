using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;
using TestApp.Models;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class NewsTagService: INewsTagService
    {

        private readonly NewsTagsContext _newsTagsContext;
        private readonly ITagService _tagService;

        public NewsTagService(NewsTagsContext newsTagsContext, ITagService tagService)
        {
            _newsTagsContext = newsTagsContext;
            _tagService = tagService;
        }


        public void SaveTagsMappingToNews(string tags, int newsId)
        {
            var splitedTags = tags.Split(',');

            var existingTags = GetExistingNewsTags(newsId);

            var tagsToDelete = existingTags.Where(existTag => !splitedTags.Contains(existTag.Name)).
                Select(tag => _newsTagsContext.NewsTagMappings.SingleOrDefault(x => x.NewsId == newsId && x.NewsTagId == tag.Id)).
                Where(deletedMapping => deletedMapping != null).
                ToList();

            if (tagsToDelete.Any())
            {
                _newsTagsContext.NewsTagMappings.RemoveRange(tagsToDelete);
                _newsTagsContext.SaveChanges();
            }

            var listMapping = new List<NewsTag_Mapping>();
            foreach (var tag in splitedTags)
            {
                var addedId = _tagService.AddTag(tag);
                if (!IsAlreadyMapped(addedId, newsId))
                {
                    listMapping.Add(new NewsTag_Mapping(){NewsId =  newsId, NewsTagId = addedId});
                }
            }
     

            _newsTagsContext.NewsTagMappings.AddRange(listMapping);
            _newsTagsContext.SaveChanges();
        }

        public void RemoveMappingByNewsId(int newsId)
        {
            var removedList = _newsTagsContext.NewsTagMappings.Where(x => x.NewsId == newsId);

            _newsTagsContext.NewsTagMappings.RemoveRange(removedList);
            _newsTagsContext.SaveChanges();

        }

        public IList<TagWidgetModel> GetNewsTagsList()
        {
            return (from tag in _newsTagsContext.NewsTags
                join tagMap in _newsTagsContext.NewsTagMappings
                    on tag.Id equals tagMap.NewsTagId
                group tag by tag.Name into pg
                let tagNumber = pg.Count()
                select new TagWidgetModel() {TagName = pg.Key, Count = tagNumber}).ToList();

        }


        private IList<NewsTag> GetExistingNewsTags(int newsId)
        {
            return (from tag in _newsTagsContext.NewsTags
                    join tagMapping in _newsTagsContext.NewsTagMappings
                        on tag.Id equals tagMapping.NewsTagId
                    where tagMapping.NewsId == newsId
                    select tag).ToList();
        }


        public string GetTagsByNewsId(int newsId)
        {
            var tagList = (from tag in _newsTagsContext.NewsTags
                           join tagMapping in _newsTagsContext.NewsTagMappings
                               on tag.Id equals tagMapping.NewsTagId
                           where tagMapping.NewsId == newsId
                           select tag.Name).ToList();

            return tagList.Count > 0 ? string.Join(",", tagList) : "";
        }

        public bool IsAlreadyMapped(int tagId, int newsId)
        {
            return _newsTagsContext.NewsTagMappings.Any(x => x.NewsId == newsId && x.NewsTagId == tagId);
        }
    }
}