using System.Collections.Generic;
using System.Web;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface IFileMappingService<T>
    {
        void AddMapping(IEnumerable<T> listMapping);
        void RemoveMapping(T entity);
    }
}