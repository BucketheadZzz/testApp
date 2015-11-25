using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class NewsFileService: INewsFileService
    {
        private readonly IFileService _fileService;
        private readonly FileContext _context;

        public NewsFileService(IFileService fileService, FileContext context)
        {
            _fileService = fileService;
            _context = context;
        }

        public void AddFileMappingToNews(IList<HttpPostedFileWrapper> files, int newsId)
        {
            var listMapping = files.Select(file => _fileService.Add(file)).Select(addedId => new NewsFileMapping {FileId = addedId, NewsId = newsId}).ToList();

            _context.NewsFilesMappings.AddRange(listMapping);
            _context.SaveChanges();
        }

        public void RemoveMappingByNewsId(int newsId)
        {
            var removedList = _context.NewsFilesMappings.Where(x => x.NewsId == newsId);

            _context.NewsFilesMappings.RemoveRange(removedList);
            _context.SaveChanges();

            _fileService.Delete(removedList.Select(x => x.FileId).ToArray());
        }

        public void RemoveMappingByNewsIdAndFileId(int newsId, int fileId)
        {
            var removedItem = _context.NewsFilesMappings.SingleOrDefault(x => x.FileId == fileId && x.NewsId == newsId);
            if (removedItem != null)
            {
                _context.NewsFilesMappings.Remove(removedItem);
                _context.SaveChanges();

                _fileService.Delete(fileId);
            }
        }

        public IList<File> GetFilesByNewsId(int newsId)
        {
            return (from file in _context.Files
                join fileMapping in _context.NewsFilesMappings on file.Id equals fileMapping.FileId
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