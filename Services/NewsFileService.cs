using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApp.DAL;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class NewsFileService: INewsFileService
    {
        private readonly IFileService _fileService;
        private readonly IRepository<NewsFileMapping> _repositoryFileMapping; 

        public NewsFileService(IFileService fileService, IRepository<NewsFileMapping> repositoryFileMapping)
        {
            _fileService = fileService;
            _repositoryFileMapping = repositoryFileMapping;
        }

        public void AddMapping(IList<HttpPostedFileWrapper> files, int newsId)
        {
            var listMapping = files.Select(file => _fileService.Add(file)).Select(addedId => new NewsFileMapping {FileId = addedId, NewsId = newsId}).ToList();

            _repositoryFileMapping.Insert(listMapping);

        }

        public void RemoveMapping(int newsId)
        {
            var removedList = _repositoryFileMapping.Table.Where(x => x.NewsId == newsId);

            _repositoryFileMapping.Delete(removedList);

            _fileService.Delete(removedList.Select(x => x.FileId).ToArray());
        }

        public void RemoveMapping(int newsId, int fileId)
        {
            var removedItem = _repositoryFileMapping.Table.SingleOrDefault(x => x.FileId == fileId && x.NewsId == newsId);
            if (removedItem != null)
            {
                _repositoryFileMapping.Delete(removedItem);

                _fileService.Delete(fileId);
            }
        }

        public IList<File> GetFilesByNewsId(int newsId)
        {
            return (from file in _fileService.GetAll()
                join fileMapping in _repositoryFileMapping.Table on file.Id equals fileMapping.FileId
                where fileMapping.NewsId == newsId
                select new File
                {
                    ContentType = file.ContentType,
                    BinaryData = file.BinaryData,
                    FileName = file.FileName,
                    Id = file.Id
                }).ToList();
        }
    }
}