using System.Collections.Generic;
using System.Web;
using TestApp.Models.Domain;

namespace TestApp.Services.Interfaces
{
    public interface IPlaylistFileService
    {
        void AddMapping(IList<HttpPostedFileWrapper> files, int fileId);
        void RemoveMapping(int playListId);
        void RemoveMapping(int playListId, int fileId);

        IList<File> GetFilesByPlayListId(int playlistId);

    }
}