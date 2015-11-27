using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data.Entity;
using System.Linq;
using System.Runtime.InteropServices.ComTypes;
using AutoMapper;
using TestApp.DAL;
using TestApp.Extensions;
using TestApp.Models;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class NewsService : INewsService
    {
        private readonly IRepository<News> _newsContext;
        private readonly ITagMappingService<NewsTagMapping> _newsMappingService;
        private readonly IFileMappingService<NewsFileMapping> _fileMappingService; 
        private readonly IFileService _fileService;
        private readonly ITagService _tagService;

        public NewsService( IRepository<News> newsContext, IFileMappingService<NewsFileMapping> fileMappingService, IFileService fileService, ITagService tagService, ITagMappingService<NewsTagMapping> newsMappingService)
        {
        
            _newsContext = newsContext;
            _fileMappingService = fileMappingService;
            _fileService = fileService;
      
            _tagService = tagService;
            _newsMappingService = newsMappingService;
        }

        public IQueryable<News> GetAll()
        {
            return _newsContext.Table;
        }

        public void Add(NewsModel item)
        {
            var entity = item.ToEntity();

            _newsContext.Insert(entity);

            if (!String.IsNullOrEmpty(item.Tags))
            {
                var tagMapping = PrepareTagMappingCollection(_tagService.Add(item.Tags.Split(',')), entity.Id);
                _newsMappingService.AddMapping(tagMapping);
            }
            if (item.Files.Count > 0 & item.Files[0] != null)
            {
                var fileMapping = PrepareFileMappingCollection(_fileService.Add(item.Files), entity.Id);
              _fileMappingService.AddMapping(fileMapping);
            }
        }

        public void Update(NewsModel item)
        {
            var entity = item.ToEntity();

            _newsContext.Update(entity);

            if (!String.IsNullOrEmpty(item.Tags))
            {
                var tagMapping = PrepareTagMappingCollection(_tagService.Add(item.Tags.Split(',')), entity.Id);
                _newsMappingService.AddMapping(tagMapping);
            }
            if (item.Files.Count > 0 & item.Files[0] != null)
            {
                var mapping = PrepareFileMappingCollection(_fileService.Add(item.Files), entity.Id);
                _fileMappingService.AddMapping(mapping);
            }
        }


        public void Delete(int id)
        {
            var removedItem = _newsContext.GetById(id);
            if (removedItem != null)
            {
                //_newsTagService.RemoveMapping(id);
                //_fileService.Delete()
                _newsContext.Delete(removedItem);
            }

        }

        public NewsModel GetById(int id)
        {
            var enity = _newsContext.Table.SingleOrDefault(x => x.Id == id);
            if (enity != null)
            {
                var model = enity.ToModel();
                model.Tags = String.Join(",", _newsMappingService.GetTagsByMapping(id).Select(x => x.Name));

                return model;
            }
            return new NewsModel();
        }

        public IList<NewsModel> List()
        {
            var temp = _newsContext.Table.ToList();

            return _newsContext.Table.Select(x => new NewsModel()
            {
                
                Title = x.Title,
                ShortDescrpition = x.ShortDescrpition,
                Id = x.Id,
                Created = x.Created

            }).ToList();
        }

        public IList<NewsModel> GetNewsByTag(string tag)
        {
            return
                (from news in _newsContext.Table
                    where news.NewsTagMapping.Count(x => x.Tag.Name == tag) > 0
                    select news).ToListModel();
        }

        private ICollection<NewsTagMapping> PrepareTagMappingCollection(IEnumerable<Tag> tags, int newsId)
        {
            var resList = new Collection<NewsTagMapping>();
            foreach (var tag in tags)
            {
                resList.Add(new NewsTagMapping(){ObjectId =  newsId, TagId = tag.Id});
            }
            return resList;
        } 

        private ICollection<NewsFileMapping> PrepareFileMappingCollection(IEnumerable<File> files, int playListId)
        {
            var resList = new Collection<NewsFileMapping>();
            foreach (var file in files)
            {
                resList.Add(new NewsFileMapping() { FileId = file.Id, ObjectId = playListId });
            }
            return resList;
        } 
    }
}