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

            builder.RegisterGeneric(typeof(FileService<>)).As(typeof(IFileService<>)).InstancePerLifetimeScope();

            builder.RegisterGeneric(typeof(TagService<>)).As(typeof(ITagService<>)).InstancePerLifetimeScope();

            var servicesAssembly = typeof(MvcApplication).Assembly;

            builder.RegisterAssemblyTypes(servicesAssembly)
                   .Where(t => t.Name.EndsWith("Service") && t.IsClass)
                   .As(t => t.GetInterfaces()
                   .Where(i => i.Name.EndsWith("Service")))
                   .InstancePerDependency();

            var container = builder.Build();

            // Set MVC DI resolver to use our Autofac container
            DependencyResolver.SetResolver(new AutofacDependencyResolver(container));
        }
    }
}