using System.Collections.Generic;
using System.Linq;
using TestApp.Models;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface INewsService
    {

  
 
        void Add(NewsModel item);

        void Update(NewsModel item);

        void Delete(int id);

        NewsModel GetById(int id);

        IQueryable<News> GetAll();

        IList<NewsModel> GetModels();

        IList<NewsModel> GetModelsByTag(string tag);
    }
}
