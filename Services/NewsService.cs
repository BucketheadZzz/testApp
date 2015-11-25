using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TestApp.Extensions;
using TestApp.Models;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class NewsService : INewsService
    {
        private readonly NewsContext _newsContext;
        private readonly INewsTagService _newsTagService;
        private readonly INewsFileService _fileService;

        public NewsService(NewsContext newsContext, INewsTagService newsTagService, INewsFileService fileService)
        {
            _newsContext = newsContext;
            _newsTagService = newsTagService;
            _fileService = fileService;
        }

        public void Add(NewsModel item)
        {
            var entity = item.ToEntity();
            _newsContext.News.Add(entity);
            _newsContext.SaveChanges();

            if (!String.IsNullOrEmpty(item.Tags))
            {
                _newsTagService.SaveTagsMappingToNews(item.Tags, entity.Id);
            }
            if (item.Files.Count > 0)
            {
               _fileService.AddFileMappingToNews(item.Files,entity.Id);
            }
        }

        public void Update(NewsModel item)
        {
            var entity = item.ToEntity();

            _newsContext.News.Attach(entity);
            var entry = _newsContext.Entry(entity);
            entry.State = EntityState.Modified;
            _newsContext.SaveChanges();

            if (!String.IsNullOrEmpty(item.Tags))
            {
                _newsTagService.SaveTagsMappingToNews(item.Tags, entity.Id);
            }
            if (item.Files.Count > 0)
            {
                _fileService.AddFileMappingToNews(item.Files, entity.Id);
            }
        }


        public void Delete(int id)
        {
            var removedItem = GetById(id);
            if (removedItem != null)
            {
                _newsTagService.RemoveMappingByNewsId(id);
                _fileService.RemoveMappingByNewsId(id);
                var entity = removedItem.ToEntity();
                _newsContext.News.Remove(entity);
                _newsContext.SaveChanges();
            }

        }

        public NewsModel GetById(int id)
        {
            var enity = _newsContext.News.SingleOrDefault(x => x.Id == id);
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
            return _newsContext.News.Select(x => x).ToListModel();
        }

        public IList<NewsModel> GetNewsByTag(string tag)
        {
            return
                (from news in _newsContext.News
                    where news.NewsTagMapping.Count(x => x.NewsTag.Name == tag) > 0
                    select news).ToListModel();
        }
    }
}