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
    public class NewsService : INewsService
    {
        private readonly IRepository<News> _newsContext;
        public NewsService(IRepository<News> newsContext)
        {
            _newsContext = newsContext;
        }

        public void Add(News item)
        {
            _newsContext.Insert(item);
           
        }

        public void Update(News item)
        {
            _newsContext.Update(item);
        }

        public void Delete(int id)
        {
            var removedItem = _newsContext.GetById(id);
            if (removedItem != null)
            {
                _newsContext.Delete(removedItem);
            }

        }

        public News GetById(int id)
        {
            var enity = _newsContext.Table.SingleOrDefault(x => x.Id == id);
            //var model = enity.ToModel();
            //model.Tags = string.Join(",", _tagService.GetTagsByMapping(id).Select(x => x.Name));

            //return model;
            return enity;
        }

        public IList<NewsModel> GetModels()
        {
            return _newsContext.Table.ToListModel();
        }

        public IList<NewsModel> GetModelsByTag(string tag)
        {
            return
                (from news in _newsContext.Table
                    where news.NewsTagMapping.Count(x => x.Tag.Name == tag) > 0
                    select news).ToListModel();
        }
     
    }
}