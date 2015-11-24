using System.Collections.Generic;
using TestApp.Models;

namespace TestApp.Services
{
    public interface INewsService
    {
        void Add(News item);

        void Update(News item);

        void Delete(int id);

        News GetById(int id);

        IList<News> List();
    }
}
