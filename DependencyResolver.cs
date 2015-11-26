using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Autofac;
using Autofac.Integration.Mvc;
using TestApp.DAL;
using TestApp.Models.Domain;
using TestApp.Services;
using TestApp.Services.Interfaces;

namespace TestApp
{
    public class AutofacAppDependencyResolver
    {
        public static void ConfigureContainer()
        {
            var builder = new ContainerBuilder();

            // Register dependencies in controllers
            builder.RegisterControllers(typeof(MvcApplication).Assembly);

            // Register dependencies in filter attributes
            builder.RegisterFilterProvider();

            // Register dependencies in custom views
            //   builder.RegisterSource(new ViewRegistrationSource());

            // Register our Data dependencies
            builder.Register<IDbContext>(c => new ObjectContext()).InstancePerLifetimeScope();
            builder.RegisterGeneric(typeof(Repository<>)).As(typeof(IRepository<>)).InstancePerLifetimeScope();
            //  builder.RegisterType<Repository<News>>().As<IRepository<News>>().InstancePerLifetimeScope();


            var servicesAssembly = typeof(MvcApplication).Assembly;

            builder.RegisterAssemblyTypes(servicesAssembly)
                   .Where(t => t.Name.EndsWith("Service") && t.IsClass)
                   .As(t => t.GetInterfaces()
                   .Where(i => i.Name.EndsWith("Service")))
                   .InstancePerDependency();


            //builder.RegisterType<UserService>().As<IUserService>().InstancePerLifetimeScope();
            //builder.RegisterType<FileService>().As<IFileService>().InstancePerLifetimeScope();
            //builder.RegisterType<TagService>().As<ITagService>().InstancePerLifetimeScope();
            //builder.RegisterType<NewsService>().As<INewsService>().InstancePerLifetimeScope();
            //builder.RegisterType<NewsFileService>().As<INewsFileService>().InstancePerLifetimeScope();
            //builder.RegisterType<NewsTagService>().As<INewsTagService>().InstancePerLifetimeScope();

            var container = builder.Build();

            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}