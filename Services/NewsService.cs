using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using TestApp.Models;

namespace TestApp.Services
{
    public class NewsService : INewsService
    {
        private readonly NewsContext _newsContext;
        public NewsService(NewsContext newsContext)
        {
            _newsContext = newsContext;
        }

        public void Add(News item)
        {
            _newsContext.News.Add(item);
            _newsContext.SaveChanges();
        }

        public void Update(News item)
        {
            _newsContext.News.Attach(item);
            var entry = _newsContext.Entry(item);
            entry.State = EntityState.Modified;
            _newsContext.SaveChanges();
        }

      
        public void Delete(int id)
        {
            var removedItem = GetById(id);
            if (removedItem != null)
            {
                _newsContext.News.Remove(removedItem);
                _newsContext.SaveChanges();  
            }
          
        }

        public News GetById(int id)
        {
            return _newsContext.News.SingleOrDefault(x => x.Id == id);
        }

        public IList<News> List()
        {
            return _newsContext.News.ToList();
        }
    }
}