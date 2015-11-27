using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApp.DAL;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class PlaylistFileService: IPlaylistFileService
    {
        private readonly IFileService _fileService;
        private readonly IRepository<PlayListFileMapping> _repositoryFileMapping;

        public PlaylistFileService(IFileService fileService, IRepository<PlayListFileMapping> repositoryFileMapping)
        {
            _fileService = fileService;
            _repositoryFileMapping = repositoryFileMapping;
        }

        public void AddMapping(IList<HttpPostedFileWrapper> files, int playlistId)
        {
            var listMapping = files.Select(file => _fileService.Add(file)).Select(addedId => new PlayListFileMapping { FileId = addedId, PlaylistId = playlistId }).ToList();

            _repositoryFileMapping.Insert(listMapping);

        }

        public void RemoveMapping(int playlistId)
        {
            var removedList = _repositoryFileMapping.Table.Where(x => x.PlaylistId == playlistId);

            _repositoryFileMapping.Delete(removedList);

            _fileService.Delete(removedList.Select(x => x.FileId).ToArray());
        }

        public void RemoveMapping(int playlistId, int fileId)
        {
            var removedItem = _repositoryFileMapping.Table.SingleOrDefault(x => x.FileId == fileId && x.PlaylistId == playlistId);
            if (removedItem != null)
            {
                _repositoryFileMapping.Delete(removedItem);

                _fileService.Delete(fileId);
            }
        }

        public IList<File> GetFilesByPlayListId(int playlistId)
        {
            return (from file in _fileService.GetAll()
                join fileMapping in _repositoryFileMapping.Table on file.Id equals fileMapping.FileId
                where fileMapping.PlaylistId == playlistId
                select new File
                {
                    ContentType = file.ContentType,
                    BinaryData = file.BinaryData,
                    FileName = file.FileName,
                    Id = file.Id
                }).ToList();
        }

        public File GetFile(int fileId)
        {
            return _fileService.GetById(fileId);
        }
    }
}