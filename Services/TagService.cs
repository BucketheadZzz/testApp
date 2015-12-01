using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Common;
using System.Linq;
using DotNetOpenAuth.OAuth.ChannelElements;
using TestApp.DAL;
using TestApp.Models;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class TagService<T> : ITagService<T> where T : class, IBaseTagMappingEntity, new()
    {
        private readonly IRepository<Tag> _tagsContext;
        private readonly IRepository<T> _tagsMappingContext;

        public TagService(IRepository<Tag> tagsContext, IRepository<T> tagsMappingContext)
        {
            _tagsContext = tagsContext;
            _tagsMappingContext = tagsMappingContext;
        }


        public Tag Add(string tagName)
        {
            var existingTag = _tagsContext.Table.SingleOrDefault(x => x.Name == tagName);
            if (existingTag != null)
            {
                return existingTag;
            }
                
            var addedTag = _tagsContext.Insert(new Tag { Name = tagName });
       
            return addedTag;
        }

        public IEnumerable<Tag> Add(IEnumerable<string> tags)
        {
            return tags.Select(Add).ToList();
        }


        public void Delete(string tagName)
        {
            var removedTag = _tagsContext.Table.SingleOrDefault(x => x.Name == tagName);
            if (removedTag == null) return;

            _tagsContext.Delete(removedTag);
        }


        public void AddMapping(IEnumerable<T> tags)
        {
            var objId = tags.First().ObjectId;
            var addedIds = tags.Select(x => x.TagId).ToList();

            var tagsToDelete = (from mapping in _tagsMappingContext.Table join allTags in _tagsContext.Table on mapping.TagId equals allTags.Id
                join existTag in GetExistingObjTags(objId).Where(x => !addedIds.Contains(x.Id))
                    on allTags.Id equals existTag.Id
                select mapping);
           
            if (tagsToDelete.Any())
            {
                _tagsMappingContext.Delete(tagsToDelete);
            }
            var listMapping = (from tag in tags
                               where !AlreadyMapped(tag.ObjectId, objId)
                               select tag).ToList();

            _tagsMappingContext.Insert(listMapping);
        }

        public void RemoveMapping(int objId)
        {
            var removedList = _tagsMappingContext.Table.Where(x => x.ObjectId == objId);
            _tagsMappingContext.Delete(removedList);
        }

        private IQueryable<Tag> GetExistingObjTags(int objId)
        {
            return from tag in _tagsContext.Table
                   join tagMapping in _tagsMappingContext.Table
                       on tag.Id equals tagMapping.TagId
                   where tagMapping.ObjectId == objId
                   select tag;
        }

        public IList<Tag> GetTagsByMapping(int objId)
        {
            return GetExistingObjTags(objId).ToList();
        }

        public bool AlreadyMapped(int tagId, int objId)
        {
            return _tagsMappingContext.Table.Any(x => x.ObjectId == objId && x.TagId == tagId);
        }

        public IList<TagWidgetModel> TagsWidget()
        {
            return (from tag in _tagsContext.Table
                    join tagMap in _tagsMappingContext.Table
                        on tag.Id equals tagMap.TagId
                    group tag by tag.Name into pg
                    let tagNumber = pg.Count()
                    select new TagWidgetModel { TagName = pg.Key, Count = tagNumber }).ToList();
        }

        public ICollection<T> PrepareTagMappingCollection(IEnumerable<Tag> tags, int newsId)
        {
            var resList = new Collection<T>();
            foreach (var tag in tags)
            {
                resList.Add(new T() { ObjectId = newsId, TagId = tag.Id });
            }
            return resList;
        } 
    }
}