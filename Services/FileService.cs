using System.Collections.Generic;
using System.Linq;
using System.Web;
using TestApp.DAL;
using TestApp.Models.Domain;
using TestApp.Services.Interfaces;

namespace TestApp.Services
{
    public class FileService<T> : IFileService<T> where T: class
    {
        private readonly IRepository<File> _fileContext;
        private readonly IRepository<T> _repositoryFileMapping;

        public FileService(IRepository<File> fileContext, IRepository<T> repositoryFileMapping)
        {
            _fileContext = fileContext;
            _repositoryFileMapping = repositoryFileMapping;
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


        public void AddMapping(IEnumerable<T> listMapping)
        {
            _repositoryFileMapping.Insert(listMapping);
        }

        public void RemoveMapping(T entity)
        {
            _repositoryFileMapping.Delete(entity);

        }
    }
}