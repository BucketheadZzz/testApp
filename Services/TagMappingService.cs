using System;
using System.Collections.Generic;
using System.Linq;
using TestApp.DAL;
using TestApp.Models;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class TagMappingService<T> : ITagMappingService<T> where T : class, IBaseTagMappingEntity
    {

        private readonly IRepository<T> _tagsMappingContext;
        private readonly ITagService _tagService;


        public TagMappingService(ITagService tagService, IRepository<T> tagsMappingContext)
        {
            _tagService = tagService;
            _tagsMappingContext = tagsMappingContext;
        }


        public void AddMapping(IEnumerable<T> tags)
        {

            var enumerable = tags as T[] ?? tags.ToArray();
            var objId = enumerable.First().ObjectId;
            var addedIds = tags.Select(x => x.TagId).ToList();
            
            //var tagsToDelete = (from allTags in _tagService.GetAll() join existTag in GetExistingObjTags(objId).Where(x => !addedIds.Contains(x.Id))
            //                                                   on allTags.Id equals existTag.Id select allTags).AsEnumerable();

            //if (tagsToDelete.Any())
            //{
            //    _tagsMappingContext.Delete(tagsToDelete);
            //}

            var listMapping = (from tag in enumerable
                               where !AlreadyMapped(tag.Id, objId)
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
            return from tag in _tagService.GetAll()
                join tagMapping in _tagsMappingContext.Table
                    on tag.Id equals tagMapping.Id
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


        //public IList<TagWidgetModel> GetNewsTagsList()
        //{
        //    return (from tag in _tagService.GetAll()
        //            join tagMap in _newsTagsMappingContext.Table
        //                on tag.Id equals tagMap.TagId
        //            group tag by tag.Name into pg
        //            let tagNumber = pg.Count()
        //            select new TagWidgetModel { TagName = pg.Key, Count = tagNumber }).ToList();

        //}
    }
}