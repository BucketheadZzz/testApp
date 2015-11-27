using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using System.Web;
using TestApp.Models.Domain;

namespace TestApp.DAL
{
    public class ObjectContext : DbContext, IDbContext
    {

        public ObjectContext()
            : base("DefaultConnection")
        {
            Database.SetInitializer<ObjectContext>(null);
        }


        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public new DbEntityEntry Entry(object ent)
        {
            return base.Entry(ent);
        }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
          

            var typesToRegister = Assembly.GetExecutingAssembly().GetTypes()
            .Where(type => !String.IsNullOrEmpty(type.Namespace))
            .Where(type => type.BaseType != null && type.BaseType.IsGenericType &&
                type.BaseType.GetGenericTypeDefinition() == typeof(EntityTypeConfiguration<>));
            foreach (var type in typesToRegister)
            {
                dynamic configurationInstance = Activator.CreateInstance(type);
                modelBuilder.Configurations.Add(configurationInstance);
            }
            modelBuilder.Entity<NewsTagMapping>().Property(x => x.ObjectId).HasColumnName("NewsId");
            modelBuilder.Entity<PlayListTagMapping>().Property(x => x.ObjectId).HasColumnName("PlaylistId");

            //...or do it manually below. For example,
            //modelBuilder.Configurations.Add(new LanguageMap());


            base.OnModelCreating(modelBuilder);
        }
    }
}