using System.Linq;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface ITagService
    {
        IQueryable<Tag> GetAll();
 
        int Add(string tagName);

        int GetTagIdByName(string tagName);

        void Delete(string tagName);

        bool IsTagAlreadyExit(string tag);


    }
}
