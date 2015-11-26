﻿using System.Collections.Generic;
using System.Linq;
using TestApp.Models;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface INewsService
    {

        IQueryable<News> GetAll();
 
        void Add(NewsModel item);

        void Update(NewsModel item);

        void Delete(int id);

        NewsModel GetById(int id);

        IList<NewsModel> List();

        IList<NewsModel> GetNewsByTag(string tag);
    }
}
