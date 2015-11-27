using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApp.DAL;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class FileMappingService<T>: IFileMappingService<T> where T: class
    {
        private readonly IRepository<T> _repositoryFileMapping;

        public FileMappingService(IRepository<T> repositoryFileMapping)
        {
            _repositoryFileMapping = repositoryFileMapping;
        }

        public void AddMapping(IEnumerable<T> listMapping)
        {
            _repositoryFileMapping.Insert(listMapping);
        }

        public void RemoveMapping(T entity)
        {
            _repositoryFileMapping.Delete(entity);

        }

 
    }
}