using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
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
        private readonly INewsTagService _newsTagService;
        private readonly INewsFileService _fileService;

        public NewsService( INewsTagService newsTagService, INewsFileService fileService, IRepository<News> newsContext)
        {
        
            _newsTagService = newsTagService;
            _fileService = fileService;
            _newsContext = newsContext;
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
                _newsTagService.SaveTagsMappingToNews(item.Tags, entity.Id);
            }
            if (item.Files.Count > 0 & item.Files[0] != null)
            {
               _fileService.AddFileMappingToNews(item.Files,entity.Id);
            }
        }

        public void Update(NewsModel item)
        {
            var entity = item.ToEntity();

            _newsContext.Update(entity);

            if (!String.IsNullOrEmpty(item.Tags))
            {
                _newsTagService.SaveTagsMappingToNews(item.Tags, entity.Id);
            }
            if (item.Files.Count > 0 & item.Files[0] != null)
            {
                _fileService.AddFileMappingToNews(item.Files, entity.Id);
            }
        }


        public void Delete(int id)
        {
            var removedItem = _newsContext.GetById(id);
            if (removedItem != null)
            {
                _newsTagService.RemoveMappingByNewsId(id);
                _fileService.RemoveMappingByNewsId(id);
                _newsContext.Delete(removedItem);
            }

        }

        public NewsModel GetById(int id)
        {
            var enity = _newsContext.Table.SingleOrDefault(x => x.Id == id);
            if (enity != null)
            {
                var model = enity.ToModel();
                model.Tags = _newsTagService.GetTagsByNewsId(enity.Id);

                return model;
            }
            return new NewsModel();
        }

        public IList<NewsModel> List()
        {
            return _newsContext.Table.Select(x => x).ToListModel();
        }

        public IList<NewsModel> GetNewsByTag(string tag)
        {
            return
                (from news in _newsContext.Table
                    where news.NewsTagMapping.Count(x => x.Tag.Name == tag) > 0
                    select news).ToListModel();
        }
    }
}