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
            return enity;
        }

        public IList<News> GetNews()
        {
            return _newsContext.Table.ToList();
        }

        public IList<News> GetNewsByTag(string tag)
        {
            return
                (from news in _newsContext.Table
                    where news.NewsTagMapping.Count(x => x.Tag.Name == tag) > 0
                    select news).ToList();
        }

        public IList<NewsModel> GetModels22()
        {
            return  new List<NewsModel>();
        }
    }
}