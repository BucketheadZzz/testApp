using System.Data.Entity;
using System.Data.Entity.Infrastructure;

namespace TestApp.DAL
{
    public interface IDbContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;

        int SaveChanges();

        DbEntityEntry Entry(object ent);
    }
}