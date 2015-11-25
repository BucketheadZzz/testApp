using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Ninject;
using TestApp.Models.Domain;
using TestApp.Services;
using TestApp.Services.Interfaces;

namespace TestApp
{
    public class NiDependencyResolver: IDependencyResolver
    {
        private readonly IKernel _kernel;

        public NiDependencyResolver(IKernel kernel)
        {
            _kernel = kernel;
            AddBindings();
        }
        public object GetService(Type serviceType)
        {
            return _kernel.TryGet(serviceType);
        }

        public IEnumerable<object> GetServices(Type serviceType)
        {
            return _kernel.GetAll(serviceType);
        }

        private void AddBindings()
        {
            _kernel.Bind<NewsContext>().To<NewsContext>();
            _kernel.Bind<UserRolesContext>().To<UserRolesContext>();
            _kernel.Bind<NewsTagsContext>().To<NewsTagsContext>();
            _kernel.Bind<FileContext>().To<FileContext>();

            _kernel.Bind<IUserService>().To<UserService>();
            _kernel.Bind<INewsService>().To<NewsService>();
            _kernel.Bind<ITagService>().To<TagService>();
            _kernel.Bind<INewsTagService>().To<NewsTagService>();
            _kernel.Bind<IFileService>().To<FileService>();
            _kernel.Bind<INewsFileService>().To<NewsFileService>();

        }
    }
}