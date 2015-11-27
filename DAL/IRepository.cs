﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TestApp.Models.Domain;

namespace TestApp.DAL
{
    public interface IRepository<T> where T: class
    {
        T GetById(object id);

        T Insert(T ent);

        IEnumerable<T> Insert(IEnumerable<T> ent);

        void Update(T ent);

        void Update(IEnumerable<T> ent);

        void Delete(T ent);

        void Delete(IEnumerable<T> ent);

     
        IQueryable<T> Table { get; }
 
    }
}
