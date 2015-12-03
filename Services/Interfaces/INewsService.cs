using System.Collections.Generic;
using System.Linq;
using TestApp.Models;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface INewsService
    {
        void Add(News item);

        void Update(News item);

        void Delete(int id);

        News GetById(int id);

        IList<News> GetNews();

        IList<News> GetNewsByTag(string tag);

    }
}
