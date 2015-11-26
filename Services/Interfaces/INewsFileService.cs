using System.Collections.Generic;
using System.Web;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface INewsFileService
    {
        void AddMapping(IList<HttpPostedFileWrapper> files, int newsId);

        void RemoveMapping(int newsId);

        void RemoveMapping(int newsId, int fileId);

        IList<File> GetFilesByNewsId(int newsId);
    }
}
