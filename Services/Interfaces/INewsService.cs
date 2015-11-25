using System.Collections.Generic;
using TestApp.Models;

namespace TestApp.Services.Interfaces
{
    public interface INewsService
    {
        void Add(NewsModel item);

        void Update(NewsModel item);

        void Delete(int id);

        NewsModel GetById(int id);

        IList<NewsModel> List();

        IList<NewsModel> GetNewsByTag(string tag);
    }
}
