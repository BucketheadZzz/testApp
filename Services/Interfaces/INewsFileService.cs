using System.Collections.Generic;
using System.Web;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface INewsFileService
    {
        void AddFileMappingToNews(IList<HttpPostedFileWrapper> files, int newsId);

        void RemoveMappingByNewsId(int newsId);

        void RemoveMappingByNewsIdAndFileId(int newsId, int fileId);

        IList<File> GetFilesByNewsId(int newsId);
    }
}
