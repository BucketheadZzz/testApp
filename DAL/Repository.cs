using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace TestApp.DAL
{
    public class Repository<T> : IRepository<T> where T : class
    {
        private readonly IDbContext _dbContext;
        private IDbSet<T> _entities;

        public Repository(IDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        protected virtual IDbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                {
                    _entities = _dbContext.Set<T>();
                }

                return _entities;
            }
        }

        public virtual IQueryable<T> Table
        {
            get
            {
                return this.Entities;
            }
        }

        public T GetById(object id)
        {
            return Entities.Find(id);
        }

        public T Insert(T ent)
        {
            if (ent == null)
            {
                throw new ArgumentNullException("ent");
            }

            Entities.Add(ent);
            _dbContext.SaveChanges();
            return ent;
        }

        public IEnumerable<T> Insert(IEnumerable<T> ent)
        {
            if (ent == null)
            {
                throw new ArgumentNullException("ent");
            }

            var resList = new List<T>();
            foreach (var entity in ent)
            {
                Entities.Add(entity);
                resList.Add(entity);
            }

            _dbContext.SaveChanges();

            return resList;
        }

        public void Update(T ent)
        {
            if (ent == null)
                throw new ArgumentNullException("ent");

            var entry = _dbContext.Entry(ent);
            entry.State = EntityState.Modified;
            _dbContext.SaveChanges();
        }

        public void Update(IEnumerable<T> ent)
        {
            if (ent == null)
                throw new ArgumentNullException("ent");

            foreach (var entity in ent)
            {
                _dbContext.Entry(entity).State = EntityState.Modified;
            }

            _dbContext.SaveChanges();
        }

        public void Delete(T ent)
        {
            if (ent == null)
            {
                throw new ArgumentNullException("ent");
            }
            if (_dbContext.Entry(ent).State != EntityState.Detached)
            {
                Entities.Attach(ent);
            }
          

            Entities.Remove(ent);

            _dbContext.SaveChanges();
        }

        public void Delete(IEnumerable<T> ent)
        {
            if (ent == null)
                throw new ArgumentNullException("ent");

            foreach (var entity in ent)
            {
                Entities.Remove(entity);
            }

            _dbContext.SaveChanges();
        }


    }
}