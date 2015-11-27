using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApp.DAL;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class FileService : IFileService
    {
        private readonly IRepository<File> _fileContext;

        public FileService(IRepository<File> fileContext)
        {
            _fileContext = fileContext;
        }


        public IQueryable<File> GetAll()
        {
            return _fileContext.Table;
        }

        public File GetById(int id)
        {
            return _fileContext.GetById(id);
        }

        public File Add(HttpPostedFileWrapper file)
        {
            var tempImage = new byte[file.ContentLength];
            file.InputStream.Read(tempImage, 0, file.ContentLength);

            var entity = new File { ContentType = file.ContentType, BinaryData = tempImage, FileName = file.FileName };
            var added = _fileContext.Insert(entity);

            return added;
        }

        public IEnumerable<File> Add(IEnumerable<HttpPostedFileWrapper> files)
        {
            return files.Select(Add).ToList();
        }


        public void Delete(int id)
        {
            var deletedItem = _fileContext.GetById(id);
            if (deletedItem != null)
            {
                _fileContext.Delete(deletedItem);

            }
        }

        public void Delete(int[] ids)
        {
            foreach (var id in ids)
            {
                Delete(id);
            }
        }
    }
}