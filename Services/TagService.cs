using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class TagService : ITagService
    {
        private readonly NewsTagsContext _newsTagsContext;

        public TagService(NewsTagsContext newsTagsContext)
        {
            _newsTagsContext = newsTagsContext;
        }

    

        public int AddTag(string tagName)
        {
            if (!IsTagAlreadyExit(tagName))
            {

                var addedTag = _newsTagsContext.NewsTags.Add(new NewsTag() { Name = tagName });
                _newsTagsContext.SaveChanges();

                return addedTag.Id;
            }
            return GetTagIdByName(tagName);
        }

        public int GetTagIdByName(string tagName)
        {
            var tag = _newsTagsContext.NewsTags.SingleOrDefault(x => x.Name == tagName);

            return tag != null ? tag.Id : -1;
        }

    

        public void RemoveTag(string tagName)
        {
            var removedTag = _newsTagsContext.NewsTags.SingleOrDefault(x => x.Name == tagName);
            if (removedTag != null)
            {
                RemoveMappingByTagId(removedTag.Id);
                _newsTagsContext.NewsTags.Remove(removedTag);
                _newsTagsContext.SaveChanges();
            }
        }



        public bool IsTagAlreadyExit(string tag)
        {
            return _newsTagsContext.NewsTags.Any(x => x.Name == tag);
        }

        public void RemoveMappingByTagId(int tagId)
        {
            var tags = _newsTagsContext.NewsTagMappings.Where(x => x.NewsTagId == tagId);
            _newsTagsContext.NewsTagMappings.RemoveRange(tags);
            _newsTagsContext.SaveChanges();
        }

     
    }
}