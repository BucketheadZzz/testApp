using System.Linq;
using System.Web;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface IFileService
    {

        IQueryable<File> GetAll();
        File GetById(int id);

        int Add(HttpPostedFileWrapper picture);

        void Delete(int id);

        void Delete(int[] ids);
    }
}
